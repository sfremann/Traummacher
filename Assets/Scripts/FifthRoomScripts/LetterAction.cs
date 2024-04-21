// --------------------------------------------------
//  file :      LetterAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the letter for the
//              bedroom scene.
// --------------------------------------------------

using UnityEngine;
using System.Linq;
using System;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the trigger to the end of the experience
/// </summary>
public class LetterAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private BedroomManager _bedroomManager;
    private ChoiceManager _choiceManager = null;
    private string _letter = "";
    private int _cursif = 0;
    private bool _badEnding = false;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find managers
    /// </summary>
    private void Start()
    {
        _bedroomManager = GameObject.Find("Managers").transform.Find("BedroomManager").gameObject.GetComponent<BedroomManager>();
        _choiceManager = GameObject.Find("ChoiceManager").GetComponent<ChoiceManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Create the letter's content
    /// </summary>
    private void CreateLetter()
    {
        int[] counters = _choiceManager.GetCounters();

        // Get the first 4 elements and find the index
        int[] persons = counters.Take(4).ToArray();
        int person = Array.IndexOf(persons, persons.Max());

        // Get the last 3 elements
        int[] reasons = counters.Skip(4).ToArray();
        int reason = Array.IndexOf(reasons, reasons.Max());

        // Check if choices has been made
        if(persons.Max() == 0)
        {
            _badEnding = true;
        }
        else
        {
            // LETTER ON MURDER
            if(reason == 2)
            {
                _cursif = 2;
                _letter += "Blood. Blood everywhere.\nWhat have I done?\n";

                switch(person)
                {
                    // LETTER FOR THE LOVER
                    case 0: 
                    _letter += "My soulmate. How could I've done that?\n";
                    break;

                    // LETTER FOR THE CHILD
                    case 1: 
                    _letter += "My own child. It was an accident.\n";
                    break;

                    // LETTER FOR THE FRIEND
                    case 2: 
                    _letter += "My best friend... Laying on the floor, unconscious...\n";
                    break;

                    // LETTER FOR THE PARENT
                    case 3: 
                    _letter += "The one who saw me raised me...\n";
                    break;
                }
                _letter += "I can't remember the accident. Everything was too loud. Screaming, crying, and suddenly, silence.\nA muffled sound, and then nothing.\nSilence.\nSilence.\nSilence.\nSince the accident, I seem to have memory troubles. That's why I am writing this letter.\nThis is not good. They are looking for me. I did something bad, something really bad.\nMy head hurts.\nIf you're reading this letter, please contact the police. I must be kept under surveillance...";
            }
            else
            {
                // Introduction
                switch(person)
                {
                    // LETTER FOR THE LOVER
                    case 0: 
                    _letter += "My love,\nAs I sit down to write this letter, my heart weighs heavy with emotions that are difficult to express in words. I guess my time has come.\nKnowing you, you should think about when we spoke in the kitchen, or when we were angry at each other. That's alright, you don't need to think about that. Don't blame yourself for anything.\n";

                    if(reasons.Max() == 0)
                    {
                        _letter += "\n...\n\n";
                    }
                    else
                    {
                        // Reason
                        switch(reason)
                        {
                            case 0: // sicknesss
                            _letter += "We knew it was going to happen. You were not ready for this time and neither was I, but we can't change heredity nor destiny... Death happens to all of us.\n";
                            break;
                            case 1: // accident
                            _letter += "I was not careful enough... You always told me about those kinds of things and... you were right. Laying in this bed, alone, without you, in this hospital, it's the worst feeling of my life. I realize that I don't want to die... I realize that I don't want to leave you.\n";
                            break;
                        }
                    }
                    break;

                    // LETTER FOR THE CHILD
                    case 1: 
                    _cursif = 1;
                    _letter += "Mummy, daddy,\nHi, it's me. I'm in the hospital, and I wanted to write you a letter. The doctors gave me this idea.\nI know you're busy and stuff, but I've been thinking a lot. Remember when we used to play in the park, and you would push me on the swings? Those were the best times. I miss the park here.\n";

                    if(reasons.Max() == 0)
                    {
                        _letter += "\n...\n\n";
                    }
                    else
                    {
                        // Reason
                        switch(reason)
                        {
                            case 0: // sicknesss
                            _letter += "Here, everything is very white, not like in our house, it's weird. The doctors and nurses are nice, but I wish I was at home with you. I don't like needles, but they say it's to help me get better... I can't remember how long I'm here. But we knew it since I'm born, right?\n";
                            break;

                            case 1: // accident
                            _letter += "It hurts badly. The doctor is not sure about what to do but it's going to be okay (I am saying that because you are too worried usually). I can't tell you when I'll be leaving.\n";
                            break;
                        }
                    }
                    break;

                    // LETTER FOR THE FRIEND
                    case 2: 
                    _letter += "To my partner in crime,\nDo you still think about the moment when we spoke in the kitchen? You seemed angry at the time.\nYet you kept quiet. You probably didn't know what to say, or didn't want to say anything. You've always looked after me, reassured me, but this time you remained silent.\nTalking about death is never easy, but we have to learn to. Rather, we have to try. No one can be ready to face the death of someone, but we can learn to live again.\n";

                    if(reasons.Max() == 0)
                    {
                        _letter += "\n...\n\n";
                    }
                    else
                    {
                        // Reason
                        switch(reason)
                        {
                            case 0: // sicknesss
                            _letter += "When we did that trip in the forest, a few months ago, when I first announced it to you... I felt so sorry for you. You couldn't find the words and neither could I... It was not part of the plan, huh?\n";
                            break;
                            case 1: // accident
                            _letter += "I was not careful enough... You always told me about those kinds of things and finally you were right, haha. Laying in this bed, feeling that I won't see you again hurts my friend... It hurts way more than the accident itself.\n";
                            break;
                        }
                    }
                    break;

                    // LETTER FOR THE PARENT
                    case 3:
                    _letter += "To my little one,\nDo you still think about the moment when we spoke in the kitchen? You seemed angry at the time.\nWe spoke a lot. No, we shouted a lot, haha. You didn't want to have this conversation, that's for sure...\nBut you know, that's how time works... It never lets us accept those types of things.\nI still remember when we used to walk you to school. You were so young... Oh you still are, compared to me, but it was different... You helped me face so many situations. Having you was like having a super power; I couldn't let anything stop me. It was one of the best periods of my life, for sure... But then you left to pursue your own life... And the super power left with you.\n";

                    if(reasons.Max() == 0)
                    {
                        _letter += "\n...\n\n";
                    }
                    else
                    {
                        // Reason
                        switch(reason)
                        {
                            case 0: // sicknesss
                            _letter += "When we did that trip in the forest, a few months ago, when I first announced it to you, I... I am sorry I could not find the words to reassure you...\n";
                            break;
                            case 1: // accident
                            _letter += "I am not as young and as athletic as I used to be. Time is killing me. This was expected, these sorts of things happen... Accidents happen, whether we deserve it or not.\n";
                            break;
                        }
                    }
                    break;
                }

                // Conclusion
                switch(person)
                {
                    case 0: // lover
                    _letter += "We had a lot of plans together. Leaving this crappy apartment, building a house... Having a child... I am sorry I won't be there with you anymore.\nBut, I want you to be happy. Please. Promise me it will be okay. Promise me not to forget me. Promise me to continue living your life, if not for you, for me.\nWe spend so much time together. If I could do it all over again, I wouldn't change a thing. Thank you for everything,\nI love you, eternally.\nYour soulmate.";
                    break;

                    case 1: // child
                    _letter += "I also remember the time when we made pancakes together. I made a mess, but it was fun. Can we do that again when I come home?\nSorry about the mistakes and the stains.\nHope I'll see you soon, love,\nMe";
                    break;

                    case 2: // friend
                    _letter += "We did everything together, I know you from the beginning... We spent so much time together... \nIf I could do it all over again, I'd totally go for the same ride.Thank you for everything, \nYour friend, always";
                    break;

                    case 3: // parent
                    _letter += "As I'm writing you this letter, it is now my time to leave.\nThank you for everything, it was wonderful seeing you growing up.\nRemember that I will always be there with you; I promise.\nI am proud of you.";
                    break;
                }
            }

        }
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Start the ending animation
    /// </summary>
    /// <param name="skipLetter">true ---> refused to read the letter</param>
    public void StartEndingAnimation(bool skipLetter = false)
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);

        // Create the letter if necessary
        if (!skipLetter) CreateLetter();

        // Disabling of the reticle
        GameObject.Find("Reticle").SetActive(false);

        // Start the ending scene
        _bedroomManager.StartEndingScene(skipLetter || _badEnding, skipLetter);
    }

    // --------------------------------------------------

    /// <summary>
    /// Get letter's content and return the writing style
    /// </summary>
    /// <param name="letter">letter's content</param>
    /// <returns>writing style</returns>
    public int GetLetterAndCursif(ref string letter)
    {
        letter = _letter;
        return _cursif;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------