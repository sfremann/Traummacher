// --------------------------------------------------
//  file :      SwitchRenderPipelineAsset.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for tweeking render settings.
// --------------------------------------------------

using UnityEngine;
using System.Collections;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling RenderPipelineAsset settings at runtime
/// </summary>
public class SwitchRenderPipelineAsset : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [Header("Global Properties")]
    [SerializeField] private Material skybox = null;
    [SerializeField] private bool fog = true;
    [SerializeField] private float fogDensity = 0.035f;
    [SerializeField] private Color fogColor;
    [SerializeField] private float ambientLightValue = 0.2f;

    [Header("ForestVision Properties")]
    [SerializeField] private Material forestVisionSkybox = null;
    [SerializeField] private float forestVisionLightValue = 1f;
    [SerializeField] private float forestVisionFogDensity = 1f;
    [SerializeField] private Color forestVisionFogColor;

    [Header("Bathroom Properties")]
    [SerializeField] private float bathroomFogDensity = 0.2f;
    [SerializeField] private Color bathroomFogColor;
    [SerializeField] private float transitionTime = 2f;

    // private variables
    private Coroutine _bathroomTransition;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Reset settings
    /// </summary>
    private void Start()
    {
        ResetRenderSettings();
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the transition in or out of the sinking sequence in the bathroom
    /// </summary>
    /// <param name="intoBathroomSeq">true ---> start sinking ; false ---> stop sinking</param>
    private void StopTransitionInBathroom(bool intoBathroomSeq)
    {
        StopCoroutine(_bathroomTransition);
        if (intoBathroomSeq)
        {
            RenderSettings.fogDensity = bathroomFogDensity;
            RenderSettings.fogColor = bathroomFogColor;
        }
        else ResetRenderSettings();
    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Transition in or out of the sinking sequence in the bathroom
    /// </summary>
    /// <param name="intoBathroomSeq">true ---> entering bathroom sequence ; false ---> exiting bathroom sequence</param>
    /// <returns></returns>
    private IEnumerator TransitionInOutBathroom(bool intoBathroomSeq)
    {
        // Transition to proper RenderSettings for Bathroom sequence
        RenderSettings.fog = true;
        float densityValue = (bathroomFogDensity - RenderSettings.fogDensity) / transitionTime;

        float rValue = (bathroomFogColor.r - RenderSettings.fogColor.r) / transitionTime;
        float gValue = (bathroomFogColor.g - RenderSettings.fogColor.g) / transitionTime;
        float bValue = (bathroomFogColor.b - RenderSettings.fogColor.b) / transitionTime;
        float aValue = (bathroomFogColor.a - RenderSettings.fogColor.a) / transitionTime;

        float coeff;

        if (intoBathroomSeq)
        {
            while (RenderSettings.fogDensity < bathroomFogDensity)
            {
                coeff = Time.deltaTime;

                RenderSettings.fogColor = new Color(RenderSettings.fogColor.r + rValue * coeff, RenderSettings.fogColor.g + gValue * coeff, RenderSettings.fogColor.b + bValue * coeff, RenderSettings.fogColor.a + aValue * coeff);

                RenderSettings.fogDensity += densityValue;

                yield return null;
            }
        }
        else
        {
            densityValue *= -1;
            rValue *= -1;
            gValue *= -1;
            bValue *= -1;
            aValue *= -1;

            while (RenderSettings.fogDensity > fogDensity)
            {
                coeff = Time.deltaTime;

                RenderSettings.fogColor = new Color(RenderSettings.fogColor.r + rValue * coeff, RenderSettings.fogColor.g + gValue * coeff, RenderSettings.fogColor.b + bValue * coeff, RenderSettings.fogColor.a + aValue * coeff);

                RenderSettings.fogDensity += densityValue;

                yield return null;
            }
        }

        StopTransitionInBathroom(intoBathroomSeq);
        yield return null;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Change render settings for the forest sequence
    /// </summary>
    public void UseForestVisionRenderSettings()
    {
        // Change RenderSettings
        RenderSettings.skybox = forestVisionSkybox;
        RenderSettings.ambientIntensity = forestVisionLightValue;
        RenderSettings.fog = true;
        RenderSettings.fogDensity = forestVisionFogDensity;
        RenderSettings.fogColor = forestVisionFogColor;
    }

    // --------------------------------------------------

    /// <summary>
    /// Change render settings for the sinking sequence in the bathroom and start transition
    /// </summary>
    /// <param name="intoBathroomSeq">true ---> entering bathroom sequence ; false ---> exiting bathroom sequence</param>
    public void UseBathroomRenderSettings(bool intoBathroomSeq)
    {
        // --- intoBathroomSeq = true -> entering bathroom sequence
        // --- intoBathroomSeq = false -> exiting bathroom sequence

        // Start transition
        _bathroomTransition = StartCoroutine(TransitionInOutBathroom(intoBathroomSeq));
    }

    // --------------------------------------------------

    /// <summary>
    /// Reset render settings
    /// </summary>
    public void ResetRenderSettings()
    {
        // Reset Skybox and light settings
        RenderSettings.skybox = skybox;
        RenderSettings.ambientIntensity = ambientLightValue;
        RenderSettings.fog = fog;
        RenderSettings.fogDensity = fogDensity;
        RenderSettings.fogColor = fogColor;
    }
}
