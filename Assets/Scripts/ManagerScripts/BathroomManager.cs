// --------------------------------------------------
//  file :      BathroomManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the bathroom scene.
// --------------------------------------------------

using System.Collections;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object used to manage effects and transitions in the bathroom scene
/// </summary>
public class BathroomManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject bathroom = null;
    [SerializeField] private GameObject bedroom = null;
    [SerializeField] private GameObject bubbles = null;
    [SerializeField] private Vector3 bathroomGravity = new(-20f, -0.2f, 20f);

    // private variables
    private Coroutine _transition;

    private Vector3 _gravity;
    private float _resetGravity = 0.1f;

    private GameObject _bathroomFloor;
    private Transform _bedroomFloor;
    private GameObject _ceiling;
    private GameObject _player = null;

    private AudioManager _audioManager;
    private SwitchRenderPipelineAsset _switchRenderPipelineAsset;
    private GlobalVolumeManager _globalVolumeManager;

    // const
    private const float _SOUND_RATE = 0.05f;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and player controller
    /// </summary>
    private void Start()
    {
        GameObject managers = GameObject.Find("Managers");
        _audioManager = managers.transform.Find("AudioManager").gameObject.GetComponent<AudioManager>();

        GameObject renderingManager = managers.transform.Find("RenderingManager").gameObject;
        _switchRenderPipelineAsset = renderingManager.GetComponent<SwitchRenderPipelineAsset>();
        _globalVolumeManager = renderingManager.GetComponent<GlobalVolumeManager>();

        _bathroomFloor = bathroom.transform.Find("Walls").Find("Ground").gameObject;
        _bedroomFloor = bedroom.transform.Find("Walls").Find("Ground");
        _ceiling = bedroom.transform.Find("Walls").Find("Ceiling").gameObject;

        _player = GameObject.Find("FirstPersonController");
        _resetGravity += _player.GetComponent<Renderer>().bounds.size.y;

        _gravity = Physics.gravity;

        bubbles.SetActive(false);
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop transition coroutine and reset gravity settings
    /// </summary>
    private void StopTransition()
    {
        StopCoroutine(_transition);

        _ceiling.SetActive(true);

        Physics.gravity = _gravity;
        PlayerManager playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        playerManager.isInBathroom = false;

        // Part 2 of transition
        _transition = StartCoroutine(TransitionPart2());

        _switchRenderPipelineAsset.UseBathroomRenderSettings(false);
        _globalVolumeManager.StartTransitionInOutBathroom(false);
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the second part of the transition
    /// </summary>
    private void StopTransitionPart2()
    {
        StopCoroutine(_transition);

        // Turn volume down
        _audioManager.Stop("Underwater");

        // Stop the bubbles
        bubbles.GetComponent<ParticleSystem>().emissionRate = 0;

        // Play melody for last room
        _audioManager.Play("LastRoomSong");

        // Destroy Bathroom
        bathroom.SetActive(false);
        Destroy(bathroom);
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Wait until the player reach the bottom 
    /// </summary>
    /// <returns></returns>
    private IEnumerator Transition()
    {
        // Just wait until we fall
        while (Mathf.Abs(_player.transform.position.y - _bedroomFloor.position.y) > _resetGravity)
        {
            // Increase Underwater Volume
            _audioManager.ChangeVolume("Underwater", _SOUND_RATE * Time.deltaTime);
            yield return null;
        }

        // Reached the bottom
        StopTransition();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Decrease underwater sound volume until it cannot be heard anymore
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionPart2()
    {
        float goal = 1f;
        while (goal >= 0)
        {
            // Decrease Underwater Volume
            _audioManager.ChangeVolume("Underwater", -_SOUND_RATE * Time.deltaTime);
            goal -= _SOUND_RATE;
            yield return null;
        }

        StopTransitionPart2();
        yield return null;
    }

    // --------------------------------------------------
    // Public methods
    // --------------------------------------------------

    /// <summary>
    /// Start oniric sequence in the bathroom
    /// </summary>
    public void StartScene()
    {
        Physics.gravity = bathroomGravity;
        PlayerManager playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        playerManager.isInBathroom = true;

        _bathroomFloor.SetActive(false);
        bedroom.SetActive(true);
        _ceiling.SetActive(false);

        // Make bubbles follow the player
        bubbles.SetActive(true);
        bubbles.transform.SetParent(_player.transform);

        // Use different render settings
        _switchRenderPipelineAsset.UseBathroomRenderSettings(true);
        _globalVolumeManager.StartTransitionInOutBathroom(true);

        // Start the water sound
        _audioManager.Play("Underwater");

        // Start transition
        _transition = StartCoroutine(Transition());
    }
}
// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------