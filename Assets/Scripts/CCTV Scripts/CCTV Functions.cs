//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 31 July 2025
// Description: CCTV Movements and stuff
//===========================================================================================================
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTVFunctions : MonoBehaviour
{
    public float maxRotationDegrees = 45f;
    public float changeInRotation = 10f;
    private float currentRotation = 0f;
    public bool autoRotate = false;
    public float scrollBarValue = 0.5f;
    private bool moveLeft;

    public Scrollbar scrollbar;
    public TextMeshProUGUI autoRotateText;

    public void OnButtonPress()
    {
        autoRotate = !autoRotate; // Start/Stop the camera from rotating automatically

        if (autoRotate)
        {
            autoRotateText.text = "II"; // Change indication
            StartCoroutine(AutoRotate());
        }
        else
        {
            autoRotateText.text = ">"; // Change indication
        }
    }

    public void OnValueChanged(float value) // Change the camera rotation and where the scroll bar value should be
    {

        if (scrollbar != null)
        {
            scrollBarValue = scrollbar.value;
        }

        float newRotation = value * maxRotationDegrees * 2 - maxRotationDegrees;
        transform.Rotate(0, newRotation - currentRotation, 0, Space.World); // Rotate in the world axis
        currentRotation = newRotation;
    }

    private IEnumerator AutoRotate()
    {
        if (scrollBarValue > .5) // Rotate right if the starting scroll bar value is more than 50%
        {
            moveLeft = false;
        }
        else // Rotate left if the starting scroll bar value is less or equal to 50%
        {
            moveLeft = true;
        }

        while (autoRotate) // Auto rotation
        {
            if (moveLeft)
            {
                transform.Rotate(0, -changeInRotation * Time.deltaTime, 0, Space.World);
                currentRotation -= changeInRotation * Time.deltaTime;
                if (currentRotation < -maxRotationDegrees)
                {
                    yield return new WaitForSecondsRealtime(3f); // Pause the camera from rotating once it reaches the far left
                    moveLeft = false;
                }
            }
            else
            {
                transform.Rotate(0, changeInRotation * Time.deltaTime, 0, Space.World);
                currentRotation += changeInRotation * Time.deltaTime;
                if (currentRotation > maxRotationDegrees)
                {
                    yield return new WaitForSecondsRealtime(3f); // Pause the camera from rotating once it reaches the far right
                    moveLeft = true;
                }
            }
            scrollBarValue = (currentRotation + maxRotationDegrees) / (2 * maxRotationDegrees); // Scroll bar value at the point where the camera rotated to
            
            if (scrollbar != null)
            {
                scrollbar.value = scrollBarValue;
            }

            OnValueChanged(scrollBarValue); // To run the method since for some reason it does not run automatically when edited here
            yield return null;
        }
    }
}
