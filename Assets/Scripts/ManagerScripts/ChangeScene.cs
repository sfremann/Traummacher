// --------------------------------------------------
//  file :      ChangeScene.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the teleportation
//              to another scene (and the comeback).
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------


/// <summary>
/// Object handling teleportation between real scenes and oniric scenes
/// </summary>
public class ChangeScene : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private bool destroyScene = true; // true ---> destroy scene object when finished

    // private variables
    private Vector3 _originalPos;
    private Quaternion _originalRot;
    private float _originalSpeed;
    private Vector3 _originalScale;

    private GameObject _startScene;
    private GameObject _otherScene;

    private GameObject _player;
    private FirstPersonController _fpc;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find player controller
    /// </summary>
    private void Start()
    {
        _player = GameObject.Find("FirstPersonController");
        _fpc = _player.GetComponent<FirstPersonController>();
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Teleport the player from [startScene] to [otherScene]
    /// </summary>
    /// <param name="startScene">scene the player is in currently</param>
    /// <param name="otherScene">scene the player must be teleported to</param>
    /// <param name="scalePlayer">rescale factor if needed</param>
    public void TeleportToScene(GameObject startScene, GameObject otherScene, float scalePlayer = 1.0f)
    {
        // Store the info
        _startScene = startScene;
        _otherScene = otherScene;
        _originalScale = _player.transform.localScale;
        _originalSpeed = _fpc.walkSpeed;

        // Save the original position
        _originalPos = _player.transform.position;
        _originalRot = _player.transform.rotation;

        // Set the Picture Vision active and deactivate the reality
        _startScene.SetActive(false);
        _otherScene.SetActive(true);

        // Teleport the _player
        Transform spawnPoint = _otherScene.transform.Find("SpawnPoint");
        _player.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        _player.transform.localScale *= scalePlayer;
        _fpc.walkSpeed *= scalePlayer;
    }

    // --------------------------------------------------

    /// <summary>
    /// Teleport the player back
    /// </summary>
    public void TeleportBackToStartScene()
    {
        if (_otherScene)
        {
            _otherScene.SetActive(false);
            if (destroyScene) Destroy(_otherScene);
        }
        if (_startScene) _startScene.SetActive(true);

        // Teleport back the _player
        _player.transform.SetPositionAndRotation(_originalPos, _originalRot);
        _player.transform.localScale = _originalScale; 
        _fpc.walkSpeed = _originalSpeed;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
