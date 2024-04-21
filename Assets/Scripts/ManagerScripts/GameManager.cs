// --------------------------------------------------
//  file :      GameManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       06/11/23
//  desc:       script used to manage the beginning of
//              the game, and delete the player prefs.
// --------------------------------------------------

using UnityEngine;
using UnityEngine.Video;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Manager of the game and application (quiting...)
/// </summary>
public class GameManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // sérialized attributes
    [SerializeField] private GameObject exitMenu = null;

    // private attributes
    private static bool _start = true;
    private static bool _hasBeenFreezed = false;
    private PlayerManager _playerManager = null;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Reset player prefs
    /// </summary>
    private void Awake()
    {
        if (_start)
        {
            DontDestroyOnLoad(this.gameObject);
            PlayerPrefs.DeleteAll();
            _start = false;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Find manager
    /// </summary>
    private void Start()
    {
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Handle game behavior
    /// </summary>
    private void Update()
    {
        if(_hasBeenFreezed == false) 
        {
            _playerManager.FreezePlayer(true);
            _hasBeenFreezed = true;
        }

        // Activate/ Deactivate exit menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(exitMenu.activeInHierarchy) Application.Quit();
            else
            {
                _playerManager.FreezePlayer(true);
                exitMenu.SetActive(!exitMenu.activeInHierarchy);
            }
        }
        if(exitMenu.activeInHierarchy && Input.GetKeyDown(KeyCode.Return))
        {
            exitMenu.SetActive(!exitMenu.activeInHierarchy);
            _playerManager.FreezePlayer(false);
        }
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------