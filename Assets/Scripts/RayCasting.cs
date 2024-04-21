// --------------------------------------------------
//  file :      RayCasting.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       17/10/23
//  desc:       script handling raycasting.
// --------------------------------------------------

using UnityEngine;
using TMPro;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

/// <summary>
/// Script handling raycasting
/// </summary>
public class RayCasting : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    // serialized variables
    [SerializeField] private TextMeshProUGUI infoText = null;
    [SerializeField] private TextMeshProUGUI bottomText = null;
    [SerializeField] private float _rayCastDistance = 1.5f;
    [SerializeField] private Sprite _cursorOff, _cursorUsable;
    
    // private variables
    private Camera _playerCamera;
    private RaycastHit _hit;
    private Ray _ray;
    private FirstPersonController _player;
    private AudioManager _audioManager;

    // --------------------------------------------------
    //  Public methods
    // --------------------------------------------------

    /// <summary>
    /// Init components
    /// </summary>
    private void Start()
    {
        _playerCamera = Camera.main;
        _player = GetComponent<FirstPersonController>();
        infoText.GetComponent<TextMeshProUGUI>();
        bottomText.GetComponent<TextMeshProUGUI>();
        _audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // --------------------------------------------------

    /// <summary>
    /// Raycast
    /// </summary>
    private void FixedUpdate()
    {
        // Raycasting creation
        _ray = _playerCamera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(_ray.origin, _ray.direction, out _hit))
        {
            // Check if the object is usable
            if (_hit.collider.CompareTag("usable") && (_hit.distance < _rayCastDistance))
            {
                // If so, indicates that you can use it...
                UpdateCursor(_cursorUsable);

                // And launch the appropriate function
                if(Input.GetMouseButtonDown(0))
                {
                    if(_hit.transform.name == "Phone")
                    {
                        PhoneMessage phone = _hit.collider.GetComponent<PhoneMessage>();
                        if(phone) phone.UsePhone(bottomText);
                    }
                    if(_hit.transform.name == "DoorWorld")
                    {
                        DoorForcing door = _hit.collider.GetComponent<DoorForcing>();
                        if(door) door.UseDoor(bottomText);
                    }
                    if( (_hit.transform.name == "ring") || (_hit.transform.name == "toy") || 
                    (_hit.transform.name == "sheet") || (_hit.transform.name == "blanket"))
                    {
                        InfoText infoObj = _hit.collider.GetComponent<InfoText>();
                        if(infoObj) infoObj.UseThisObject(bottomText);
                    }
                    else
                    {
                        Usable Obj = _hit.collider.GetComponent<Usable>();
                        if(_audioManager) _audioManager.Play("UseSound");
                        if(Obj) Obj.UseThisObject();  //use the object
                    }
                }
            } 
            else UpdateCursor(_cursorOff);
        }

        // If no raycasting, reinitialisation of the infoText
        else UpdateCursor(_cursorOff);
    }

    // --------------------------------------------------

    /// <summary>
    /// Update the cursor's sprite
    /// </summary>
    /// <param name="cursor"></param>
    void UpdateCursor(Sprite cursor)
    {
        _player.updateCrosshair(cursor);
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------