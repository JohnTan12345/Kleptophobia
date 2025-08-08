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
    public bool playerNearby = false; // Whether the player is nearby to trigger door opening

    private Vector3 closedRotation;  // Original rotation of the door
    private Vector3 openRotation;    // Target rotation when door is open
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Save the closed rotation
        closedRotation = door.eulerAngles;

        // Calculate what the open rotation will be
        openRotation = new Vector3(
            closedRotation.x,
            closedRotation.y + openAngle,
            closedRotation.z
        );
    }

    // Update is called once per frame
    void Update()
    {
        // Smoothly rotate door depending on whether player is nearby
        if (playerNearby)
        {
            door.eulerAngles = Vector3.Lerp(
                door.eulerAngles,
                openRotation,
                Time.deltaTime * openSpeed
            );
        }
        else
        {
            door.eulerAngles = Vector3.Lerp(
                door.eulerAngles,
                closedRotation,
                Time.deltaTime * openSpeed
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object entering the trigger is the player
        {
            playerNearby = true; // Set playerNearby to true to open the door
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the object exiting the trigger is the player
        {
            playerNearby = false; // Set playerNearby to false to close the door
        }
    }
}
