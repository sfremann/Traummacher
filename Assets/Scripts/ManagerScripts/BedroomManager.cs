// --------------------------------------------------
//  file :      BedroomManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for handling ending sequence.
// --------------------------------------------------

using System.Collections;
using TMPro;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object used to manage effects and transitions in the bedroom scene
/// </summary>
public class BedroomManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private ParticleSystem dancingLights = null;

    [SerializeField] private TextMeshProUGUI credits = null;

    [SerializeField] private TextMeshProUGUI letterText = null;
    [SerializeField] private TMP_FontAsset cursif_child;
    [SerializeField] private TMP_FontAsset cursif_simple;
    [SerializeField] private TMP_FontAsset cursif_crazy;

    [SerializeField] private float typingTime = 0.1f;
    [SerializeField] private float displayTime = 2.0f;
    [SerializeField] private float transitionToCreditsTime = 4f;

    // private variables
    private GlobalVolumeManager _globalVolumeManager;
    private bool _playAlternativeEnding = false;
    private float _dotTime;

    private Coroutine _transitionToCredits;

    // --------------------------------------------------
    // Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers
    /// </summary>
    private void Start()
    {
        _dotTime = typingTime * 10f;

        _globalVolumeManager = transform.parent.Find("RenderingManager").GetComponent<GlobalVolumeManager>();
        _globalVolumeManager.AssignDancingLights(dancingLights);
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop transition, it is the end of the experience
    /// </summary>
    private void EndGame()
    {
        StopCoroutine(_transitionToCredits);
        GetComponent<BedroomManager>().enabled = false;
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Make the letter fade in little by little
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayLetter(string textToDisplay)
    {
        // Beginning : disable the raycasting and start the displaying
        yield return new WaitForSeconds(0.1f);

        letterText.text = "";
        foreach (char letter in textToDisplay)
        {
            letterText.text += letter;

            if ((letter == '.') || (letter == '?') || (letter == '!') || (letter == '…'))
            {
                yield return new WaitForSeconds(_dotTime);
            }

            //to-do : work on the madness typing speed
            yield return new WaitForSeconds(typingTime);
        }

        // Call transition to credits roll
        _transitionToCredits = StartCoroutine(TransitionToCreditsRoll());

        // Wait the reading time
        yield return new WaitForSeconds(displayTime);
    }

    // --------------------------------------------------

    /// <summary>
    /// Make the letter fade out little by little
    /// </summary>
    /// <returns></returns>
    private IEnumerator TransitionToCreditsRoll()
    {
        Color endColor = _playAlternativeEnding ? Color.black : Color.white;

        float rColorVal = (endColor.r - letterText.color.r) / transitionToCreditsTime;
        float gColorVal = (endColor.g - letterText.color.g) / transitionToCreditsTime;
        float bColorVal = (endColor.b - letterText.color.b) / transitionToCreditsTime;
        float aColorVal = (endColor.a - letterText.color.a) / transitionToCreditsTime;

        float coeff;

        if (_playAlternativeEnding)
        {
            while (letterText.color.r > endColor.r)
            {
                coeff = Time.deltaTime;

                letterText.color = new Color(letterText.color.r + rColorVal * coeff, letterText.color.g + gColorVal * coeff, letterText.color.b + bColorVal * coeff, letterText.color.a + aColorVal * coeff);

                yield return null;
            }
        }
        else
        {
            while (letterText.color.r < endColor.r)
            {
                coeff = Time.deltaTime;

                letterText.color = new Color(letterText.color.r + rColorVal * coeff, letterText.color.g + gColorVal * coeff, letterText.color.b + bColorVal * coeff, letterText.color.a + aColorVal * coeff);

                yield return null;
            }
        }

        CreditsRoll();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Make the credits fade in little by little
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisplayCredits()
    {
        credits.enabled = true;
        credits.color = new Color(credits.color.r, credits.color.g, credits.color.b, 0f);

        float finalColorVal = Color.white.a;
        float colorVal = finalColorVal / transitionToCreditsTime;

        while (credits.color.a < finalColorVal)
        {
            credits.color = new Color(credits.color.r, credits.color.g, credits.color.b, credits.color.a + colorVal * Time.deltaTime);
            yield return null;
        }

        EndGame();
        yield return null;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Start the final sequence of the experience
    /// </summary>
    /// <param name="playAlternativeEnding">true ---> fade to black instead of white</param>
    /// <param name="skipLetter">false ---> do not display the letter</param>
    public void StartEndingScene(bool playAlternativeEnding, bool skipLetter)
    {
        _playAlternativeEnding = playAlternativeEnding;
        _globalVolumeManager.StartTransitionToEnding(_playAlternativeEnding, skipLetter);
    }

    // --------------------------------------------------

    /// <summary>
    /// Start reading the letter
    /// </summary>
    public void ReadTheLetter()
    {
        // Declare the letter
        string letter = "";

        // Select the cursif
        int cursif = GameObject.Find("Letter").GetComponent<LetterAction>().GetLetterAndCursif(ref letter);
        letterText.enabled = true;
        
        switch(cursif)
        {
            case 0:
                letterText.font = cursif_simple;
                break;
            
            case 1:
                letterText.font = cursif_child;
                break;

            case 2:
                letterText.font = cursif_crazy;
                break;
        }

        // Change the color of the text
        if(_playAlternativeEnding == true)
        {
            letter = "You chose not to remember.\nTherefore, you forget. Eternally.";
            letterText.color = Color.white;
        }

        // Display the letter
        StartCoroutine(DisplayLetter(letter));
    }

    // --------------------------------------------------

    /// <summary>
    /// Start transition to display the credits
    /// </summary>
    public void CreditsRoll()
    {
        // Self-explanatory

        if (_playAlternativeEnding == true) credits.color = Color.white;

        if (_transitionToCredits != null) StopCoroutine(_transitionToCredits);

        // Destroy the bedroom
        GameObject bedroom = dancingLights.transform.parent.gameObject;
        bedroom.SetActive(false);
        Destroy(bedroom);

        _transitionToCredits = StartCoroutine(DisplayCredits());
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------