// --------------------------------------------------
//  file :      MenuManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       23/11/23
//  desc:       manager of the main menu.
// --------------------------------------------------

using UnityEngine.SceneManagement;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the menu
/// </summary>
public class MenuManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private int gameSceneID = 1;

    // private variables
    private GameObject _aboutWindow;
    private GameObject _buttonPlay;
    private GameObject _buttonExit;
    private GameObject _buttonAbout;
    private GameObject _fullMenu;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------
    
    /// <summary>
    /// Set the menu buttons
    /// </summary>
    private void Start()
    {
        // Get the about window and disable it
        _aboutWindow = GameObject.Find("AboutWindow");
        if(_aboutWindow) _aboutWindow.SetActive(false);

        // Get the full menu
        _fullMenu = GameObject.Find("TO_DESTROY");
        
        // Get the button of the menu
        _buttonPlay = GameObject.Find("Play");
        _buttonExit = GameObject.Find("Exit");
        _buttonAbout = GameObject.Find("About");
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Load the scene
    /// </summary>
    public void Play()
	{
        // Loads the scene with index 'gameSceneID' in the editor(index indicated in the list of scenes to compile in File-> Build Settings)
        SceneManager.LoadScene(gameSceneID);

        // Destroy the menu
        Destroy(_fullMenu);
	}

    // --------------------------------------------------

    /// <summary>
    /// Display an "About" section
    /// </summary>
    public void About()
	{
        if(_aboutWindow) _aboutWindow.SetActive(true);

        if(_buttonPlay) _buttonPlay.SetActive(false);
        if(_buttonAbout) _buttonAbout.SetActive(false);
        if(_buttonExit) _buttonExit.SetActive(false);
	}

    // --------------------------------------------------

    /// <summary>
    /// Quit the "About" section
    /// </summary>
    public void QuitAbout()
	{
        if(_aboutWindow) _aboutWindow.SetActive(false);
        
        if(_buttonPlay) _buttonPlay.SetActive(true);
        if(_buttonAbout) _buttonAbout.SetActive(true);
        if(_buttonExit) _buttonExit.SetActive(true);
	}

    // --------------------------------------------------

    /// <summary>
    /// Quit the game
    /// </summary>
    public void Exit()
	{
		Application.Quit();
	}
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------