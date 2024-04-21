// --------------------------------------------------
//  file :      KitchenManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script for handling kitchen sequence.
// --------------------------------------------------

using System.Collections;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Object handling transitions and effects in the kitchen sequence
/// </summary>
public class KitchenManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private GameObject rainFall = null;
    [SerializeField] private Light[] lights;
    [SerializeField] private float phaseTime = 2.0f;
    [SerializeField] private int nbPhase = 5;
    [SerializeField] private float lightTransitionTime = 3f;

    // private variables
    private GlobalVolumeManager _globalVolumeManager;
    private AudioManager _audioManager; 
    private KnifeAction _knife;
    private float _audioIncRate;
    private float _initLightIntensity;    

    private Coroutine _increaseMadnessFreqCo = null;
    private Coroutine _lightTransition = null;

    // --------------------------------------------------
    // Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers 
    /// </summary>
    private void Start()
    { 
        _globalVolumeManager = transform.parent.Find("RenderingManager").GetComponent<GlobalVolumeManager>();
        _audioManager = transform.parent.Find("AudioManager").GetComponent<AudioManager>();

        _initLightIntensity = lights[0].intensity;

        rainFall.SetActive(false);       
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the light transition
    /// </summary>
    private void StopLightTransition()
    {
        StopCoroutine(_lightTransition);
    }

    // --------------------------------------------------
    // Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Refactor camera and heart beat effects periodically for SI28
    /// </summary>
    /// <returns></returns>
    private IEnumerator RefactorMadnessFreq()
    {
        _audioIncRate = -(float)(1 / nbPhase);
        for (int i = 0; i < nbPhase; i++)
        {
            // Increase heart beat
            _audioManager.ChangeVolume("SingleHeartBeating", _audioIncRate);

            // Increase heart beat rate visually
            _globalVolumeManager.RefactorKitchenEffectPeriod();

            yield return new WaitForSeconds(phaseTime);
        }
        StopKitchenSeq();
        yield return null;
    }

    // --------------------------------------------------

    /// <summary>
    /// Transition to make the light go brighter or weaker
    /// </summary>
    /// <param name="increaseLight">true ---> increase light intensity</param>
    /// <returns></returns>
    private IEnumerator LightTransition(bool increaseLight)
    {
        // Increase or decrease light intensity
        float lightRate = _initLightIntensity / lightTransitionTime;

        if (increaseLight)
        {
            while (lights[0].intensity < _initLightIntensity)
            {
                foreach (var light in lights) light.intensity += lightRate * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (lights[0].intensity > 0f)
            {
                foreach (var light in lights) light.intensity -= lightRate * Time.deltaTime;
                yield return null;
            }
        }

        StopLightTransition();
        yield return null;
    }

    // --------------------------------------------------
    // Public methods
    // --------------------------------------------------

    /// <summary>
    /// Start the sequence in the kitchen after enabling effects and specific components
    /// </summary>
    public void StartKitchenSeq()
    {
        // Enable Camera effect
        _globalVolumeManager.EnableKitchenCameraEffects();

        // Make the rain fall
        rainFall.SetActive(true);
        _audioManager.Play("Thunderstorm");

        // Move objects
        _knife.ActivateKnifeAction();
        _increaseMadnessFreqCo = StartCoroutine(RefactorMadnessFreq());
        

        // Light transition
        _lightTransition = StartCoroutine(LightTransition(false));
    }

    // --------------------------------------------------

    /// <summary>
    /// Stop the sequence in the kitchen and reset objects and effects
    /// </summary>
    public void StopKitchenSeq()
    {
        // Disable Camera effect
        _globalVolumeManager.DisableKitchenCameraEffects();

        // Stop audio
        _audioManager.Stop("SingleHeartBeating");
        _audioManager.Stop("Thunderstorm");

        // Stop the rain 
        rainFall.GetComponent<ParticleSystem>().emissionRate = 0;


        // Stop effects
        StopCoroutine(_increaseMadnessFreqCo);

        // Reset knife
        _knife.ResetKnife();

        // Light transition
        _lightTransition = StartCoroutine(LightTransition(true));
    }

    // --------------------------------------------------

    /// <summary>
    /// Play the sound of a heart beat
    /// </summary>
    public void PlayHeartBeatSound()
    {
        _audioManager.Replay("SingleHeartBeating");
    }

    // --------------------------------------------------

    /// <summary>
    /// Assign a knife action which this manager will activate and deactivate during transition
    /// </summary>
    /// <param name="knife">knife on the table</param>
    public void AssignKnife(KnifeAction knife)
    {
        _knife = knife;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------