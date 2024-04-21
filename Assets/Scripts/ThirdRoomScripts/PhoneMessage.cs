// --------------------------------------------------
//  file :      PhoneMessage.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script used in to display a phone
//              message.
// --------------------------------------------------

using System.Collections;
using UnityEngine;
using TMPro;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script used in to display a phone message
/// </summary>
public class PhoneMessage : MonoBehaviour
{    
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [Header("Ink JSON")] 
    [SerializeField] private string childMessage = null;
    [SerializeField] private string loverMessage = null;
    [SerializeField] private string friendMessage = null;
    [SerializeField] private string parentMessage = null;

    // private variables
    private string message = null;
    private GameObject _Fireflies = null;
    private ChoiceManager _choiceManager = null;
    private AudioManager _audioManager;
    private PlayerManager _playerManager;
    private TextMeshProUGUI _bottomText = null;
    private const float _displayTime = 2.0f;
    private const float _typingSpeed = 0.04f;
    private const float _dotTime = 1.0f;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers
    /// </summary>
    private void Start()
    {
        _Fireflies = gameObject.transform.Find("Fireflies").gameObject;

        _choiceManager = GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        _playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------


    /// <summary>
    /// The phone is being used
    /// </summary>
    /// <param name="bottomText">text to display on use</param>
    public void UsePhone(TextMeshProUGUI bottomText)
    {
        _bottomText = bottomText;

        // Disable the phone
        GetComponent<Collider>().enabled = false;
        _Fireflies.SetActive(false);

        if(_audioManager)
        {
            _audioManager.Stop("PhoneRinging");
            _audioManager.Stop("PhoneNoAnswer");
        }

        int person = _choiceManager.GetMaxIndex();

        if(person == -1) message = "You have no message in your voicebox.";
        else
        {
            switch(person)
            {
                case 0:     // LOVER
                    message = loverMessage; break;
                
                case 1:     // CHILD
                    message = childMessage; break;
                
                case 2:     // FRIEND
                    message = friendMessage; break;
                
                case 3:     // PARENT
                    message = parentMessage; break;
                
                default:
                    Debug.LogError("Error while finding the message"); break;
            }
        }

        // Start the dialogue that the object contains
        StartCoroutine(DisplayText(message));
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
        // Beginning : disable the raycasting and start the displaying
        yield return new WaitForSeconds(0.1f);
        _playerManager.FreezePlayer(true);

        _bottomText.text = "";
        foreach(char letter in textToDisplay.ToCharArray())
        {
            _bottomText.text += letter;
            if((letter == '.') || (letter == '?') || (letter == '!') || (letter == '…'))
            {
                yield return new WaitForSeconds(_dotTime);
            }
            yield return new WaitForSeconds(_typingSpeed);
        }

        // Wait the reading time
        yield return new WaitForSeconds(_displayTime);
        _bottomText.text = "";

        // Enable back the raycasting
        _playerManager.FreezePlayer(false);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------