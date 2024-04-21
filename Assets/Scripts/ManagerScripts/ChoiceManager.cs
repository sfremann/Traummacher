// --------------------------------------------------
//  file :      ChoiceManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       main file for the choice manager.
//              deals with the choices and their
//              consequences.
// --------------------------------------------------

using UnityEngine;
using System.Linq;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling player choices throughout the experience
/// </summary>
public class ChoiceManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [Header("_Counters values")] 
    public static int[] _counters = new int[7];

    // private variables
    
    private int _call = 0;

    private CustomSceneManager _sceneManager;
    private KitchenManager _kitchenManager;
    private ForestManager _forestManager;
    private BathroomManager _bathroomManager;

    private int _lastChoice = -1;

    // enum
    private enum IntObj { PictureFrame = 1, Knife = 2, Shoes = 3, FirstAidKit = 4, Letter = 5 };
    private enum CountIndex { L = 0, C, F, P, Fi, Fa, Fm };

    // const
    private const int _DEFAULT_MADNESS = 1;
    private const int _SKIP_LETTER_CHOICE = 1;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers and reset counters
    /// </summary>
    private void Start()
    {
        _sceneManager = transform.parent.Find("CustomSceneManager").GetComponent<CustomSceneManager>();
        _kitchenManager = transform.parent.Find("KitchenManager").GetComponent<KitchenManager>();
        _forestManager = transform.parent.Find("ForestManager").GetComponent<ForestManager>();
        _bathroomManager = transform.parent.Find("BathroomManager").GetComponent<BathroomManager>();
        for (int i = 0; i < 7; i++) _counters[i] = 0;
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Method called only when a choice is made (thus, it should be called 5 times ONLY)
    /// </summary>
    /// <param name="upChoices">choice code</param>
    /// <param name="choiceIndex">last choice made</param>
    public void UpdateChoices(int upChoices, int choiceIndex)
    {
        // update the index of the last choice made
        _lastChoice = choiceIndex;

        // increment the number of calls
        ++_call;

        // update choices
        for (int i = 0; i < 7; i++)
        {
            int power = (7 - 1) - i;
            int digit = (upChoices / (int)Mathf.Pow(10, power)) % 10;

            _counters[i] += digit;
        }


        // update the scene when it is needed
        switch((IntObj)_call)
        {
            // scene 1: showing hidden objects
            case IntObj.PictureFrame:
                if (_sceneManager) _sceneManager.UpdateScene(_counters);
                break;

            // scene 2: particle and showing hidden objects
            case IntObj.Knife:
                if (_kitchenManager) _kitchenManager.StartKitchenSeq();
                break;

            // scene 3: activate phone and switch in the forest
            case IntObj.Shoes:
                if (_forestManager) _forestManager.StartForestSeq();
                break;

            // scene 4: pills
            case IntObj.FirstAidKit:
                _bathroomManager.StartScene();
                break;

            // dream-like scenes
            case IntObj.Letter:
                GameObject.Find("Letter").GetComponent<LetterAction>().StartEndingAnimation(_lastChoice == _SKIP_LETTER_CHOICE);
                break;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Callable strictly from the dialogue manager - deals with "madness object"
    /// </summary>
    public void IncreaseMadness()
    {
        _counters[(int)CountIndex.Fm] += _DEFAULT_MADNESS;
    }

    // --------------------------------------------------

    /// <summary>
    /// Get the max of the index
    /// </summary>
    /// <returns></returns>
    public int GetMaxIndex()
    {
        int countMax = _counters.Max();
        int idx = -1;
        if(countMax != 0)
        {
            for(int i = 0; i < 4; i++)
            {
                if(_counters[i] == countMax) idx = i;
            }
        }
        return idx;
    }

    // --------------------------------------------------

    /// <summary>
    /// Get counters values
    /// </summary>
    /// <returns></returns>
    public int[] GetCounters()
    {
        return _counters;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
