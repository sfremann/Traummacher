// --------------------------------------------------
//  file :      Usable.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script used in every usable object
//              of the scene.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// script used in every usable object of the scene
/// </summary>
public class Usable : MonoBehaviour
{    
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [Header("Ink JSON")] 
    [SerializeField] private TextAsset _inkJSON = null;
    [SerializeField] private int[] _healthChoices = null;
    [SerializeField] private int[] _countChoices = null;

    // private variables
    private GameObject _obj = null;
    private GameObject _Fireflies = null;

    // enum
    private enum SavingParam {objNotActive = 1, colNotActive = 2};

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Init usable object
    /// </summary>
    private void Start()
    {
        // Instance of the gameObject
        _obj = gameObject;

        // Get the saved value of the object state
        int objState = PlayerPrefs.GetInt(_obj.name);

        // If the object must be not active
        if (objState == (int)SavingParam.objNotActive) _obj.SetActive(false);

        // If the object's collider must be disabled
        else if (objState == (int)SavingParam.colNotActive) _obj.GetComponent<Collider>().enabled = false;

        // Get the fireflies
        _Fireflies = gameObject.transform.Find("Fireflies").gameObject;
        if (_Fireflies) _Fireflies.SetActive(true);
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Upon use of the object
    /// </summary>
    public void UseThisObject()
    {
        // If it is a "madness" object
        if(_obj.name == "mainDoor")
        {
            // Start the dialogue that the object contains
            DialogueManager.GetInstance().EnterDialogueMode(_inkJSON, _healthChoices, _countChoices, true);

            // Make the object disapears and save its state
            _obj.SetActive(false);
        }

        // If it is a "main" object
        else
        {
            // Disable the collider and save its state
            _obj.GetComponent<Collider>().enabled = false;
            
            // Start the dialogue that the object contains
            DialogueManager.GetInstance().EnterDialogueMode(_inkJSON, _healthChoices, _countChoices);
        } 

        // Deactivate the fireflies
        if(_Fireflies) _Fireflies.SetActive(false);
    } 
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------