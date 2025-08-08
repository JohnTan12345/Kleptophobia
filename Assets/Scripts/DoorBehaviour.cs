//===========================================================================================================
// Author: Foong Mun Yip Xander
// Created: 8 August 2025
// Description: Door behaviour script for opening and closing door outside security room
//===========================================================================================================


using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Transform player;      // Reference to the player transform
    public float openAngle = 90f;  // How far the door opens
    public float openSpeed = 2f;   // How fast the door opens
    public float triggerDistance = 3f; // How close the player must be to trigger the door

    private Vector3 closedRotation; 
    private Vector3 targetRotation; 
    private bool isOpen = false;    // Door state

    void Start()
    {
        // Initialize closed rotation based on the door's current rotation
        closedRotation = transform.eulerAngles;
        targetRotation = closedRotation;
    }

    void Update()
    {
        // Check the distance between the player and the door
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < triggerDistance)
        {
            if (!isOpen) // Runs if door is closed
            {
                Vector3 toPlayer = player.position - transform.position; // Vector from door to player
                float dot = Vector3.Dot(transform.forward, toPlayer); // Dot product to determine player position relative to door (positive equals in front, negative equals behind)

                if (dot > 0) // Player is in front of the door (since dot is more than 0)
                {
                    // Player is in front of the door
                    targetRotation = new Vector3(
                        closedRotation.x,
                        closedRotation.y + openAngle,
                        closedRotation.z
                    );
                }
                else // Player is behind the door (less than 0)
                {
                    // Player is behind the door
                    targetRotation = new Vector3(
                        closedRotation.x,
                        closedRotation.y - openAngle,
                        closedRotation.z
                    );
                }

                isOpen = true;
            }
        }
        else
        {
            if (isOpen) // If the player moves away, target rotation is set back to closed
            {
                targetRotation = closedRotation;
                isOpen = false;
            }
        }

        // Smooth rotation using LerpAngle for each axis
        Vector3 currentRotation = transform.eulerAngles; // Door's current rotation
        // Smoothly interpolate (lerp) the X rotation from current to target
        float newX = Mathf.LerpAngle(currentRotation.x, targetRotation.x, Time.deltaTime * openSpeed);
        // Smoothly interpolate the Y rotation from current to target
        float newY = Mathf.LerpAngle(currentRotation.y, targetRotation.y, Time.deltaTime * openSpeed);
        // Smoothly interpolate the Z rotation from current to target
        float newZ = Mathf.LerpAngle(currentRotation.z, targetRotation.z, Time.deltaTime * openSpeed);
        // Apply the new rotation to the door, which updates the door's rotation each frame so that the animation is smoother
        transform.eulerAngles = new Vector3(newX, newY, newZ);
    }
}

