// --------------------------------------------------
//  file :      DialogueManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       main file for the dialogue manager.
//              deals with the display of dialogue
//              throughout the game.
// --------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Handle dialogue mode
/// </summary>
public class DialogueManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private float typingTime = 0.065f;
    [SerializeField] private float displayTime = 2.0f;

    [Header("Dialogue UI")] 
    [SerializeField] private TextMeshProUGUI _dialogueText;

    [Header("Choices UI")] 
    [SerializeField] private GameObject[] _choices;

    // private variables
    private HealthManager _healthManager;
    private PlayerManager _playerManager;
    private ChoiceManager _choiceManager;
    private GlobalVolumeManager _renderManager;
    
    private TextMeshProUGUI[] _choicesText;
    private Coroutine _displayLineCoroutine;
    private static DialogueManager _instance;
    private bool _displaying = false;
    private bool _choiceToMake = false;
    private bool _exitRequired = false;
    private Story _currentStory; 
    private int[] _healthChoices;
    private int[] _countChoices;
    private string _objectName;

    private float _dotTime;

    // public variables
    public bool DialogueIsPlaying { get; private set; }
    [HideInInspector]
    public Image imageInfo;

    // --------------------------------------------------
    //  Singleton methods
    // --------------------------------------------------

    /// <summary>
    /// Get [_instance] value
    /// </summary>
    /// <returns></returns>
    public static DialogueManager GetInstance()
    {
        return _instance;
    }

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Check signe instance of the dialogue manager
    /// </summary>
    private void Awake()
    {
        if (_instance) Debug.LogWarning("Found more than one instance of the Dialogue Manager.");
        _instance = this;
    }

    // --------------------------------------------------

    /// <summary>
    /// Find manager and init information
    /// </summary>
    private void Start()
    {
        _dotTime = typingTime * 10f;

        // Get the managers
        _healthManager = transform.parent.Find("HealthManager").GetComponent<HealthManager>();
        _playerManager = transform.parent.Find("PlayerManager").GetComponent<PlayerManager>();
        _choiceManager = transform.parent.Find("ChoiceManager").GetComponent<ChoiceManager>();
        _renderManager = transform.parent.Find("RenderingManager").GetComponent<GlobalVolumeManager>();

        // Choices declarations
        int index = 0;
        DialogueIsPlaying = false;
        _choicesText = new TextMeshProUGUI[_choices.Length];
        foreach(GameObject choice in _choices)
        {
            _choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        // Disable the image
        imageInfo.enabled = false;
    }

    // --------------------------------------------------

    /// <summary>
    /// Check if in or out of dialogue mode
    /// </summary>
    private void Update()
    {
        if(_exitRequired)
        {
            if(DialogueIsPlaying == false && _displaying == false)
            {
                _playerManager.FreezePlayer(false);
                _exitRequired = false;
            }
            else if (_currentStory.currentChoices.Count == 0 && _displaying == false)
            {
                ContinueStory();
            }
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Handle the continuation of the story
    /// </summary>
    private void ContinueStory()
    {
        if (_currentStory.canContinue)
        {
            if (_displayLineCoroutine != null) StopCoroutine(_displayLineCoroutine);
            _displayLineCoroutine = StartCoroutine(DisplayText(_currentStory.Continue()));

            // hide the choice if necessary
            if (_choiceToMake == false)
            {
                for (int i = 0; i < _choices.Length; i++) _choices[i].SetActive(false);
            }
        }
        else if (!_displaying) StartCoroutine(ExitDialogueMode());
    }

    // --------------------------------------------------

    /// <summary>
    /// Enter choice dialogue mode
    /// </summary>
    /// <param name="start">true ---> get inside the dialogue mode</param>
    private void EnterChoiceMode(bool start = true)
    {
        // Start transition to enter Choice mode
        _renderManager.StartChoiceEffect(start);
    }

    // --------------------------------------------------

    /// <summary>
    /// Change image sprite
    /// </summary>
    private void ChangeImageSprite()
    {
        if (!imageInfo.enabled) // First call
        {
            imageInfo.sprite = Resources.Load<Sprite>(_objectName);
            if (imageInfo.sprite == null) Debug.LogError("Sprite not found: " + _objectName);
            imageInfo.enabled = true;
        }
        else // Second call
        {
            imageInfo.sprite = null;
            imageInfo.gameObject.SetActive(false);
        }

    }

    // --------------------------------------------------
    //  Coroutines
    // --------------------------------------------------

    /// <summary>
    /// Exiting dialogue mode
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExitDialogueMode()
    {
        DialogueIsPlaying = false;
        yield return new WaitForSeconds(displayTime);
        _dialogueText.text = "";
    }

    // --------------------------------------------------

    /// <summary>
    /// Display the =text
    /// </summary>
    /// <param name="line">text to display</param>
    /// <param name="madness">true ---> use a specific writing style</param>
    /// <returns></returns>
    private IEnumerator DisplayText(string line, bool madness = false)
    {
        yield return new WaitForSeconds(0.1f);
        _playerManager.FreezePlayer(true);

        _displaying = true;
        _dialogueText.text = "";
        foreach (char letter in line.ToCharArray())
        {
            _dialogueText.text += letter;

            if ((letter == '.') || (letter == '?') || (letter == '!') || (letter == '…'))
            {
                yield return new WaitForSeconds(_dotTime);
            }

            yield return new WaitForSeconds(madness ? (Random.Range(1.5f, 3.0f) * typingTime) : typingTime);
        }

        if (_choiceToMake && _healthChoices.Length > 0)
        {
            EnterChoiceMode();
            _choiceToMake = false;
        }
        else
        {
            yield return new WaitForSeconds(displayTime);
            _displaying = false;
        }
    }

    // --------------------------------------------------

    /// <summary>
    /// Put the selection cursor on the first choice
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(_choices[0]);
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Enter dialogue mode
    /// </summary>
    /// <param name="inkJSON">story</param>
    /// <param name="healthChoices">health values attached to each choice</param>
    /// <param name="countChoices">values of each choice</param>
    /// <param name="incMadness">true ---> increase madness and draws toward the murder ending</param>
    public void EnterDialogueMode(TextAsset inkJSON, int[] healthChoices, int[] countChoices, bool incMadness = false)
    {
        // Verify every "madness" object
        if(incMadness == true) _choiceManager.IncreaseMadness();

        _healthChoices = healthChoices;
        _countChoices = countChoices;
        _currentStory = new Story(inkJSON.text);
        _objectName = inkJSON.name;
        DialogueIsPlaying = true;
        _exitRequired = true;

        if(_countChoices != null && _countChoices.Length > 0) _choiceToMake = true;
        else _choiceToMake = false;

        ContinueStory();
    }

    // --------------------------------------------------

    /// <summary>
    /// Display choices on screen
    /// </summary>
    public void DisplayChoices()
    {
        List<Choice> currentChoices = _currentStory.currentChoices;

        // Preview the image
        ChangeImageSprite();

        if(currentChoices.Count > _choices.Length) Debug.LogError("More choices were given than the UI can support.");

        int index = 0;
        foreach(Choice choice in currentChoices)
        {
            _choices[index].SetActive(true);
            _choicesText[index].text = choice.text;
            index++;
        }
        for(int i = index; i < _choices.Length; i++) _choices[i].SetActive(false);

        // Prepare the first choice
        StartCoroutine(SelectFirstChoice());
    }

    // --------------------------------------------------

    /// <summary>
    /// Hide choices and proceed to show the rest of the story
    /// </summary>
    public void HideChoices()
    {
        ContinueStory();
    }

    // --------------------------------------------------

    /// <summary>
    /// Make a choice
    /// </summary>
    /// <param name="choiceIndex">choice value</param>
    public void MakeChoice(int choiceIndex)
    {
        // Take care of the health (if needed)
        if (_healthChoices.Length > 0) _healthManager.ChangeHealthValue(_healthChoices[choiceIndex]);

        // Take care of the counters (if needed)
        if (_countChoices.Length > 0) _choiceManager.UpdateChoices(_countChoices[choiceIndex], choiceIndex);
        _currentStory.ChooseChoiceIndex(choiceIndex);

        // Hide the image 
        imageInfo.enabled = false;
        
        // Transition and remove dialogue menu
        _renderManager.StartChoiceEffect(false);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------