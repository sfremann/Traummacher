// --------------------------------------------------
//  file :      DoorForcing.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       05/12/23
//  desc:       script used when forcing the door.
// --------------------------------------------------

using System.Collections;
using UnityEngine;
using TMPro;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script used when forcing the door
/// </summary>
public class DoorForcing : MonoBehaviour
{    
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private string message = null;

    // private variables
    private HealthManager _healthManager = null;
    private AudioManager _audioManager;
    private const float _typingSpeed = 0.035f;
    private const float _displayTime = 2.0f;
    private const float _dotTime = 1.0f;
    private RayCasting _raycasting = null;
    private TextMeshProUGUI _bottomText;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers
    /// </summary>
    private void Start()
    {
        _healthManager = GameObject.Find("HealthManager").GetComponent<HealthManager>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _raycasting = GameObject.Find("FirstPersonController").GetComponent<RayCasting>();
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Display the text
    /// </summary>
    /// <param name="textToDisplay">text to display</param>
    /// <returns></returns>
    private IEnumerator DisplayText(string textToDisplay)
    {
        // Beginning: disable the raycasting and start the displaying
        yield return new WaitForSeconds(0.1f);
        _raycasting.enabled = false;

        _bottomText.text = "";
        foreach (char letter in textToDisplay.ToCharArray())
        {
            _bottomText.text += letter;
            if ((letter == '.') || (letter == '?') || (letter == '!') || (letter == '…'))
            {
                yield return new WaitForSeconds(_dotTime);
            }
            yield return new WaitForSeconds(_typingSpeed);
        }

        // Wait the reading time
        yield return new WaitForSeconds(_displayTime);
        _bottomText.text = "";

        // Enable back the raycasting
        _raycasting.enabled = true;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Handle the forcing of a door
    /// </summary>
    /// <param name="bottomText">text to print when trying to use the door</param>
    public void UseDoor(TextMeshProUGUI bottomText)
    {
        // Disable the door
        gameObject.tag = "Untagged";

        // Save the bottomText
        _bottomText = bottomText;
        
        if(_audioManager) _audioManager.Play("ForcingSound");
        
        _healthManager.ChangeHealthValue(-2);

        // Start the dialogue that the object contains
        StartCoroutine(DisplayText(message));
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------