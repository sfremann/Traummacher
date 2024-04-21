// --------------------------------------------------
//  file :      PlayerManager.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       main file for the player specific
//              actions, like freeze for instance.
// --------------------------------------------------

using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling character controller behavior
/// </summary>
public class PlayerManager : MonoBehaviour
{
    // --------------------------------------------------
    //  Variable declaration
    // --------------------------------------------------

    // private variables
    private FirstPersonController _fpc;

    // public variables
    public GameObject fpc;
    public AudioManager audioManager;

    [HideInInspector]
    public bool isIndoor = true;
    [HideInInspector]
    public bool isInBathroom = false;   
    
    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    /// <summary>
    /// Get controller
    /// </summary>
    private void Start()
    {
        _fpc = fpc.GetComponent<FirstPersonController>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Update the behavior depending on the room
    /// </summary>
    private void Update()
    {
        // in the bathroom, no headbob
        _fpc.enableHeadBob = !isInBathroom;

        // Sound management
        if (_fpc.isWalking && !isInBathroom)
        {
            if (audioManager)
            {
                if(isIndoor) audioManager.Play("IndoorFootsteps");
                else audioManager.Play("OutdoorFootsteps");
            }
        }
        else
        {
            if (audioManager)
            {
                if (isIndoor) audioManager.Stop("IndoorFootsteps");
                else audioManager.Stop("OutdoorFootsteps");
            }
        }
    }

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Freeze/Unfreeze the player
    /// </summary>
    /// <param name="freeze">true ---> freeze the player</param>
    public void FreezePlayer(bool freeze)
    {
        _fpc.playerCanMove = !freeze;
        _fpc.cameraCanMove = !freeze;
        GameObject.Find("FirstPersonController").GetComponent<Rigidbody>().useGravity = !freeze;
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------