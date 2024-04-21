// --------------------------------------------------
//  file :      TriggerTexting.cs
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
/// Handle text displayed unpon entering certain zones
/// </summary>
public class TriggerTexting : MonoBehaviour
{
    // --------------------------------------------------
    //  Attribute declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private string _inkJSON;
    [SerializeField] private TextMeshProUGUI _bottomText = null;

    // private variables
    private RayCasting _raycasting = null;

    // const
    private const float _typingSpeed = 0.035f;
    private const float _displayTime = 2.0f;
    private const float _dotTime = 1.0f;

    // --------------------------------------------------
    // Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find raycasting script
    /// </summary>
    private void Start()
    {
        _raycasting = GameObject.Find("FirstPersonController").GetComponent<RayCasting>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Display associated text when entering trigger zone
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(DisplayText(_inkJSON));
        gameObject.GetComponent<Collider>().enabled = false;
    }

    // --------------------------------------------------
    // Coroutines
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