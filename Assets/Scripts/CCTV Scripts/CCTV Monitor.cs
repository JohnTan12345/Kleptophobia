//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 25 July 2025
// Description: Monitor functions
//===========================================================================================================
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CCTVMonitor : MonoBehaviour
{
    // UI Variables
    private Transform canvas;
    private TextMeshProUGUI autoRotateText;
    private Scrollbar scrollbar;
    private Button autoRotateButton;
    private TextMeshProUGUI cameraName;

    // Camera Variables
    public GameObject cameraGrp;
    private CCTVFunctions CCTVFunctions = null;
    private CameraVariables cameraVariables;
    private SortedList<int, RenderTexture> cameraFeedRenderTexture;
    private SortedList<int, Transform> cameras;

    // Monitor Variables
    public int startCameraNumber = 0;
    private int cameraNumber = 0;
    private MeshRenderer meshRenderer;
    static List<int> UsedCameras = new List<int>();

    void Start()
    {
        // UI Mapping
        canvas = transform.Find("Canvas");
        autoRotateButton = canvas.Find("Rotate Button").GetComponent<Button>();
        autoRotateText = canvas.Find("Rotate Button").Find("Text").GetComponent<TextMeshProUGUI>();
        scrollbar = canvas.Find("Camera Angle Slider").GetComponent<Scrollbar>();
        cameraName = canvas.Find("Camera Name Bg").Find("Camera Name").GetComponent<TextMeshProUGUI>();
        transform.Find("Canvas").Find("Prev").GetComponent<Button>().onClick.AddListener(PrevCamera);
        transform.Find("Canvas").Find("Next").GetComponent<Button>().onClick.AddListener(NextCamera);

        canvas.GetComponent<Canvas>().worldCamera = Camera.main;

        // Get Monitor Screen's Mesh Renderer
        meshRenderer = GetComponent<MeshRenderer>();

        // Get Cameras
        cameraVariables = cameraGrp.GetComponent<CameraVariables>();

        cameraNumber = startCameraNumber;
        StartCoroutine(ChangeCameraNumber(startCameraNumber));
    }

    // Display Camera Feed
    private void DisplayCameraFeed()
    {
        if (!cameraFeedRenderTexture[cameraNumber].IsCreated())
        {
            cameraVariables.CreateRenderTexture(cameraNumber);
        }

        meshRenderer.material.SetTexture("_BaseMap", cameraFeedRenderTexture[cameraNumber]);
    }

// Change Camera Functions

    private void NextCamera()
    {
        bool foundNewCamera = false;
        int NewCamera = cameraNumber;

        while (!foundNewCamera)
        {
            NewCamera++;

            if (NewCamera > cameras.Count - 1)
            {
                NewCamera = 0;
            }

            if (!UsedCameras.Contains(NewCamera))
            {
                StartCoroutine(ChangeCameraNumber(NewCamera));
                foundNewCamera = true;
            }
        }
    }

    private void PrevCamera()
    {
        bool foundNewCamera = false;
        int NewCamera = cameraNumber;

        while (!foundNewCamera)
        {
            NewCamera--;

            if (NewCamera < 0)
            {
                NewCamera = cameras.Count - 1;
            }

            Debug.Log(!UsedCameras.Contains(NewCamera));

            if (!UsedCameras.Contains(NewCamera))
            {
                StartCoroutine(ChangeCameraNumber(NewCamera));
                foundNewCamera = true;
            }
        }
    }

    private IEnumerator ChangeCameraNumber(int newCamera)
    {
        while (!cameraVariables.Initialized)
        {
            yield return null;
        }

        if (cameraFeedRenderTexture == null || cameras == null)
        {
            cameraFeedRenderTexture = cameraVariables.CameraFeedRenderTexture;
            cameras = cameraVariables.Cameras;
        }

        if (CCTVFunctions != null)
        {
            CCTVFunctions.scrollbar = null;
            CCTVFunctions.autoRotateText = null;
            autoRotateButton.onClick.RemoveAllListeners();
            scrollbar.onValueChanged.RemoveAllListeners();
        }

        CCTVFunctions = cameras[newCamera].GetComponent<CCTVFunctions>();

        //Assign Scrollbar variables to CCTVFunctions
        CCTVFunctions.scrollbar = scrollbar;
        scrollbar.value = CCTVFunctions.scrollBarValue;
        scrollbar.onValueChanged.AddListener(CCTVFunctions.OnValueChanged);

        // Assign autoRotate variables to CCTVFunctions
        CCTVFunctions.autoRotateText = autoRotateText;
        autoRotateButton.onClick.AddListener(CCTVFunctions.OnButtonPress);
        cameraName.text = string.Format("{0}: {1}",newCamera ,cameras[newCamera].name);

        if (CCTVFunctions.autoRotate)
        {
            autoRotateText.text = "II";
        }
        else
        {
            autoRotateText.text = ">";
        }

        UsedCameras.Remove(cameraNumber);
        UsedCameras.Add(newCamera);
        cameraNumber = newCamera;
        DisplayCameraFeed();
    }
}