// --------------------------------------------------
//  file :      InfoText.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script used in every usable object
//              of the scene.
// --------------------------------------------------

using System.Collections;
using UnityEngine;
using TMPro;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the display of information for usable objects
/// </summary>
public class InfoText : MonoBehaviour
{    
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [Header("Ink JSON")] 
    [SerializeField] private string _text = null;

    // private variables
    private RayCasting _raycasting = null;
    private GameObject _Fireflies = null;
    private TextMeshProUGUI _bottomText = null;
    private const float _displayTime = 2.0f;
    private const float _typingSpeed = 0.04f;
    private const float _dotTime = 1.0f;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Init info object
    /// </summary>
    private void Start()
    {
        _Fireflies = gameObject.transform.Find("Fireflies").gameObject;
        if (_Fireflies) _Fireflies.SetActive(true);
        _raycasting = GameObject.Find("FirstPersonController").GetComponent<RayCasting>();
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Use an object
    /// </summary>
    /// <param name="bottomText">text to display</param>
    public void UseThisObject(TextMeshProUGUI bottomText)
    {
        // Store the text to write in
        _bottomText = bottomText;

        // Play the text
        StartCoroutine(DisplayText(_text));

        // Disable the fireflies
        if(_Fireflies) _Fireflies.SetActive(false);

        // Disable object collider
        GetComponent<Collider>().enabled = false;
    } 

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Display the indo text
    /// </summary>
    /// <param name="textToDisplay">text to display</param>
    /// <returns></returns>
    private IEnumerator DisplayText(string textToDisplay)
    {
        // Beginning : disable the raycasting and start the displaying
        yield return new WaitForSeconds(0.1f);
        _raycasting.enabled = false;

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
        _raycasting.enabled = true;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------