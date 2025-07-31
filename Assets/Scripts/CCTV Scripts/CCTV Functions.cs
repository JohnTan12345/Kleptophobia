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
        autoRotate = !autoRotate;

        if (autoRotate)
        {
            autoRotateText.text = "II";
            StartCoroutine(AutoRotate());
        }
        else
        {
            autoRotateText.text = ">";
        }
    }

    public void OnValueChanged(float value)
    {

        if (scrollbar != null)
        {
            scrollBarValue = scrollbar.value;
        }

        float newRotation = value * maxRotationDegrees * 2 - maxRotationDegrees;
        transform.Rotate(0, newRotation - currentRotation, 0, Space.World);
        currentRotation = newRotation;
    }

    private IEnumerator AutoRotate()
    {
        if (scrollBarValue > .5)
        {
            moveLeft = false;
        }
        else
        {
            moveLeft = true;
        }

        while (autoRotate)
        {
            if (moveLeft)
            {
                transform.Rotate(0, -changeInRotation * Time.deltaTime, 0, Space.World);
                currentRotation -= changeInRotation * Time.deltaTime;
                if (currentRotation < -maxRotationDegrees)
                {
                    yield return new WaitForSecondsRealtime(3f);
                    moveLeft = false;
                }
            }
            else
            {
                transform.Rotate(0, changeInRotation * Time.deltaTime, 0, Space.World);
                currentRotation += changeInRotation * Time.deltaTime;
                if (currentRotation > maxRotationDegrees)
                {
                    yield return new WaitForSecondsRealtime(3f);
                    moveLeft = true;
                }
            }
            scrollBarValue = (currentRotation + maxRotationDegrees) / (2 * maxRotationDegrees);
            
            if (scrollbar != null)
            {
                scrollbar.value = scrollBarValue;
            }

            OnValueChanged(scrollBarValue);
            yield return null;
        }
    }
}
