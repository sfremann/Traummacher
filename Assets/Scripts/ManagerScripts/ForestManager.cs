// --------------------------------------------------
//  file :      ForestManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for handling forest sequence.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling transitions and effects in the forest sequence
/// </summary>
public class ForestManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject realRoom = null;
    [SerializeField] private GameObject visionRoom = null;
    [SerializeField] private bool openBathroomDirectly = false;

    // private variables
    private SwitchRenderPipelineAsset _renderManager = null;
    private ChangeScene _changingSceneManager = null;
    private ChangingRoom _changingRoomManager = null;
    private GameObject _phone = null;
    private AudioManager _audioManager = null;
    private GlobalVolumeManager _globalVolumeManager = null;

    private PlayerManager _playerManager = null;

    // const
    private const float _FOREST_SOUND_TRANS_TIME = 0.5f;
    private const float _FOREST_PLAYER_RESCALE_FACT = 0.33f; // Need to rescale to match terrain properties

    // --------------------------------------------------
    // Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and player controller
    /// </summary>
    private void Start()
    {
        _renderManager = transform.parent.Find("RenderingManager").GetComponent<SwitchRenderPipelineAsset>();
        _changingSceneManager = transform.parent.Find("ChangingSceneManager").GetComponent<ChangeScene>();
        _changingRoomManager = transform.parent.Find("ChangingRoomManager").gameObject.GetComponent<ChangingRoom>();
        _audioManager = transform.parent.Find("AudioManager").gameObject.GetComponent<AudioManager>();
        _globalVolumeManager = transform.parent.Find("RenderingManager").GetComponent<GlobalVolumeManager>();

        // Use 3D player manager
        _playerManager = transform.parent.Find("PlayerManager").gameObject.GetComponent<PlayerManager>();
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Teleport the player back to the appartment and reset sound and visual effects
    /// </summary>
    public void ExitForest()
    {
        //Start the sound of the corridor
        if (_audioManager) _audioManager.Replay("LightSound");

        // Update indoor status in player controller/manager
        if (_playerManager) _playerManager.isIndoor = true;

        // Teleport back to reality
        _renderManager.ResetRenderSettings();
        _changingSceneManager.TeleportBackToStartScene();
        if (openBathroomDirectly) _changingRoomManager.ChangeRoom();

        // Activate the phone
        _phone.GetComponent<Collider>().enabled = true;
        _phone.transform.Find("Fireflies").gameObject.SetActive(true);
        _audioManager.Play("PhoneRinging");
        _audioManager.Play("LightBuzzing");

        // No more need for this script for the rest of the game
        GetComponent<ForestManager>().enabled = false;
    }

    // --------------------------------------------------

    /// <summary>
    /// Start transition to exit the forest
    /// </summary
    public void StopForestSeq()
    {
        _globalVolumeManager.StartTransitionInOutForest(false);
    }

    // --------------------------------------------------

    /// <summary>
    /// Start transition to enter the forest
    /// </summary>
    public void StartForestSeq()
    {
        // Stop the sound of the corridor
        if(_audioManager) _audioManager.Stop("LightSound");

        // Update indoor status in player controller/manager
        if (_playerManager) _playerManager.isIndoor = false;

        // Teleport to forest
        _renderManager.UseForestVisionRenderSettings();        
        _changingSceneManager.TeleportToScene(realRoom, visionRoom, _FOREST_PLAYER_RESCALE_FACT);
        _globalVolumeManager.StartTransitionInOutForest(true);
        if(_audioManager) _audioManager.SoundTransition("ForestSounds", _FOREST_SOUND_TRANS_TIME, false);
    }

    // --------------------------------------------------

    /// <summary>
    /// Assign [_phone] object
    /// </summary>
    /// <param name="phone">object with the PhoneAction component</param>
    public void AssignPhone(GameObject phone)
    {
        _phone = phone;

        if(!openBathroomDirectly) _phone.GetComponent<PhoneAction>().SetOpenDoor(!openBathroomDirectly);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------