//===========================================================================================================
// Author: Foong Mun Yip Xander
// Created: 8 August 2025
// Description: Door behaviour script for opening and closing door outside security room
//===========================================================================================================


using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Transform door;           // door
    public float openAngle = 90f;    // How far the door opens
    public float openSpeed = 2f;     // Speed of door rotation
    public bool playerNearby = false;

    private Vector3 closedRotation;  // Original rotation of the door
    private Vector3 openRotation;    // Target rotation when door is open
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
