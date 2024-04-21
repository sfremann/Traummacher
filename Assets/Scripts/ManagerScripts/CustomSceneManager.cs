// --------------------------------------------------
//  file :      CustomSceneManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script that manage the scene and
//              their components.
// --------------------------------------------------

using UnityEngine;
using System.Linq;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script used to update game information based on character choices
/// </summary>
public class CustomSceneManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [Header("Scenes parameters")] 
    [SerializeField] private GameObject RevealScenes;

    // private variables
    private ChangingRoom _roomChanger;

    // --------------------------------------------------
    //  private methods
    // --------------------------------------------------

    /// <summary>
    /// Find manager
    /// </summary>
    private void Start()
    {
        _roomChanger = transform.parent.Find("ChangingRoomManager").GetComponent<ChangingRoom>();        
    }
    
    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Reveal special elements based on player's choices
    /// </summary>
    /// <param name="counters">index keeping choice records</param>
    public void UpdateScene(int[] counters)
    {
        // Check for flaws or wrong values
        if(counters.Length == 0)
        {
            Debug.LogError("Error with counters length or index access of reveal scenes.");
            return;
        }

        // Get max value of counters
        int maxVal = counters.Max();

        if(maxVal != 0)
        {
            // Get all the children of the reveal object (Unity order)
            int index = 0;
            foreach (Transform obj in RevealScenes.transform)
            {
                // If the object correspond the the max value, display it
                if(counters[index] == maxVal)
                {
                    // Make the object active and save its state
                    obj.gameObject.SetActive(true);
                }
                index++;
            }
        }

        // Open the next room
        _roomChanger.ChangeRoom();
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------
