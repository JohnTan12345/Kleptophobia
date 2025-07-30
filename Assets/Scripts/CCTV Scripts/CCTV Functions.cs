using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTVFunctions : MonoBehaviour
{
    public float maxRotationDegrees = 0f;
    private float changeInRotation = 10f;
    [SerializeField]
    private float currentRotation = 0f;
    private bool autoRotate = false;
    private bool moveLeft;

    private Scrollbar scrollbar;
    private TextMeshProUGUI AutoRotateText;

    void Start()
    {
        Transform Canvas = transform.Find("Canvas");
        Button AutoRotateButton = Canvas.Find("Rotate Button").GetComponent<Button>();
        AutoRotateText = Canvas.Find("Rotate Button").Find("Text").GetComponent<TextMeshProUGUI>();
        scrollbar = Canvas.Find("Camera Angle Slider").GetComponent<Scrollbar>();
        AutoRotateButton.onClick.AddListener(OnButtonPress);
        scrollbar.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnButtonPress()
    {
        autoRotate = !autoRotate;

        if (autoRotate)
        {
            AutoRotateText.text = "II";
            StartCoroutine(AutoRotate());
        }
        else
        {
            AutoRotateText.text = ">";
        }
    }

    private void OnValueChanged(float value)
    {
        float newRotation = value * maxRotationDegrees * 2 - maxRotationDegrees;
        transform.Rotate(0, newRotation - currentRotation, 0, Space.World);
        currentRotation = newRotation;
    }

    private IEnumerator AutoRotate()
    {
        if (currentRotation > maxRotationDegrees / 2)
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
            scrollbar.value = (currentRotation + maxRotationDegrees) / (2 * maxRotationDegrees);
            OnValueChanged(scrollbar.value);
            yield return null;
        }
    }
}
