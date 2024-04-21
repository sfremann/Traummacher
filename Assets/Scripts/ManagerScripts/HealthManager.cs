// --------------------------------------------------
//  file :      HealthManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script that deals with health-linked
//              problematics.
// --------------------------------------------------

using System.Collections;
using UnityEngine.UI;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script managing player's health
/// </summary>
public class HealthManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private Slider healthBar = null;

    [SerializeField] private float apparitionTime = 1f;
    [SerializeField] private float waitingTime = 4f;
    [SerializeField] private float disparitionTime = 1f;

    // private variables
    private Image _background;
    private Image _fill;

    private static int _healthValue = 10;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Init health values
    /// </summary>
    private void Start()
    {
        healthBar.value = _healthValue;
        _background = healthBar.gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        _fill = healthBar.gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();

        _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, 0);
        _fill.color = new Color(_fill.color.r, _fill.color.g, _fill.color.b, 0);
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Change health value
    /// </summary>
    /// <param name="healthValue">new health value</param>
    public void ChangeHealthValue(int healthValue)
    {
        StartCoroutine(ChangeHealth(healthValue));
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Update the health
    /// </summary>
    /// <param name="healthValue">new health value</param>
    /// <returns></returns>
    IEnumerator ChangeHealth(int healthValue)
    {
        // Show the health bar
        StartCoroutine(FadeSlider(true));
        yield return new WaitForSeconds(apparitionTime);

        // Check min and max values
        if (healthValue > 0) _healthValue = Mathf.Min(_healthValue + healthValue, 10);
        else _healthValue = Mathf.Max(_healthValue + healthValue, 0);

        // Update the slider
        healthBar.value = _healthValue;

        yield return new WaitForSeconds(waitingTime);

        // Disparition
        StartCoroutine(FadeSlider(false));
        yield return new WaitForSeconds(disparitionTime);
    }

    // --------------------------------------------------

    /// <summary>
    /// Fade health bar interface
    /// </summary>
    /// <param name="apparition">true ---> appearing</param>
    /// <returns></returns>
    IEnumerator FadeSlider(bool apparition)
    {
        for(int i = 0; i < 255; i++)
        {
            _background.color = new Color(_background.color.r, _background.color.g, _background.color.b, ((apparition ? i : (254 - i)) / 255.0f));
            _fill.color = new Color(_fill.color.r, _fill.color.g, _fill.color.b, ((apparition ? i : (254 - i)) / 255.0f));
            yield return new WaitForSeconds(0.01f);
        }
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------