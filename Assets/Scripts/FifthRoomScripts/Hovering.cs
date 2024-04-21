// --------------------------------------------------
//  file :      PlayerPathPointExit.cs
//  authors:    Victor Billaud, Sarah Fremann
//  date:       10/11/23
//  desc:       script used to manage the exit of a 
//              player path point (decision not made 
//              by the guide)
// --------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// --------------------------------------------------
//  BEGINNING OF THE CLASS
// --------------------------------------------------

public class Hovering : MonoBehaviour
{
    // --------------------------------------------------
    //  Attributes Declaration
    // --------------------------------------------------

    public bool hovering = true;
    public bool rotating = true;
    public bool randomSpeed = true;

    public float hoverAmplitude = 1.0f;
    public float hoverSpeed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float orbitRadius = 0.01f;
    public Axis hoverAxis = Axis.Y;

    public enum Axis
    {
        X,
        Y,
        Z
    }

    private float _originValue;

    // --------------------------------------------------
    //  Private methods
    // --------------------------------------------------

    private void Start()
    {
        _originValue = GetAxisValue();
        rotationSpeed = (randomSpeed ? Random.Range(0.1f, 2.0f) : rotationSpeed);
        hoverSpeed = (randomSpeed ? Random.Range(0.1f, 2.0f) : hoverSpeed);
    }

    // --------------------------------------------------

    private void Update()
    {
        if (hovering) UpdateHover();
        if (rotating) RotateAroundAxisWithOffset();
    }

    // --------------------------------------------------

    private void UpdateHover()
    {
        float hoverValue = _originValue + hoverAmplitude * Mathf.Sin(Time.time * hoverSpeed);

        // Appliquez la nouvelle position à l'objet
        Vector3 newPosition = transform.position;

        switch (hoverAxis)
        {
            case Axis.X:
                newPosition.x = hoverValue;
                break;
            case Axis.Y:
                newPosition.y = hoverValue;
                break;
            case Axis.Z:
                newPosition.z = hoverValue;
                break;
        }

        transform.position = newPosition;
    }

    // --------------------------------------------------

    private void RotateAroundAxisWithOffset()
    {
        // Calcule la nouvelle position de rotation
        float angle = Time.time * rotationSpeed;
        float x = Mathf.Cos(angle) * orbitRadius;
        float z = Mathf.Sin(angle) * orbitRadius;

        // Applique la nouvelle position avec l'offset à l'objet
        Vector3 newPosition = transform.position + new Vector3(x, 0, z);
        transform.position = newPosition;
    }
    
    // --------------------------------------------------

    private float GetAxisValue()
    {
        switch (hoverAxis)
        {
            case Axis.X:
                return transform.position.x;
            case Axis.Y:
                return transform.position.y;
            case Axis.Z:
                return transform.position.z;
            default:
                return 0f;
        }
    }
}

// --------------------------------------------------
//  END OF THE FILE
// --------------------------------------------------