// --------------------------------------------------
//  file :      PhoneAction.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script dealing with the phone for the
//              corridor scene.
// --------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling the interaction with the phone in the corridor
/// </summary>
public class PhoneAction : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // private variables
    private ChangingRoom _changingRoomManager;
    private bool _openBathroom = true;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Find manager
    /// </summary>
    private void Start()
    {
        _changingRoomManager = GameObject.Find("Managers").transform.Find("ChangingRoomManager").gameObject.GetComponent<ChangingRoom>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Open the bathroom when interacted with, except if the bathroom is already opened
    /// </summary>
    /// <param name="other">player's collider</param>
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<Collider>().enabled = false;
        transform.Find("Fireflies").gameObject.SetActive(false);

        // Open next room
        if (_openBathroom) _changingRoomManager.ChangeRoom();
    }

    // --------------------------------------------------

    /// <summary>
    /// Set whether the interaction with the phone should open the bathroom or not
    /// </summary>
    /// <param name="openBathroom">true ---> interacting with the phone will open the bathroom</param>
    public void SetOpenDoor(bool openBathroom)
    {
        _openBathroom = openBathroom;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------