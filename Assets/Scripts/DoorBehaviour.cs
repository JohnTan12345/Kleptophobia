//===========================================================================================================
// Author: Foong Mun Yip Xander
// Created: 8 August 2025
// Description: Door behaviour script for opening and closing door outside security room
//===========================================================================================================


using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Transform player;       // Player reference
    public float openAngle = 90f;  // How far to swing the door
    public float openSpeed = 2f;   // How fast to open
    public float triggerDistance = 3f; // Distance to trigger

    private Vector3 closedRotation; // Original rotation
    private Vector3 targetRotation; // Rotation to move towards
    private bool isOpen = false; // State of the door

    void Start()
    {
        // Store the door's starting rotation in Euler angles
        closedRotation = transform.eulerAngles;
        targetRotation = closedRotation;
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < triggerDistance) // Player is close enough to interact
        {
            if (!isOpen)
            {
                // Open the door
                targetRotation = new Vector3(
                    closedRotation.x,
                    closedRotation.y + openAngle,
                    closedRotation.z
                );
                isOpen = true;
            }
        }
        else
        {
            if (isOpen)
            {
                // Close the door
                targetRotation = closedRotation;
                isOpen = false;
            }
        }

        // Smoothly rotate towards target rotation
        transform.eulerAngles = Vector3.Lerp(
            transform.eulerAngles,
            targetRotation,
            Time.deltaTime * openSpeed
        );
    }
}

