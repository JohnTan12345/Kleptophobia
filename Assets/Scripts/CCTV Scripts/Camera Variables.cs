//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 31 July 2025
// Description: Camera Lists for display onto monitor later
//===========================================================================================================
using System.Collections.Generic;
using UnityEngine;

public class CameraVariables : MonoBehaviour
{
    private SortedList<int, RenderTexture> cameraFeedRenderTexture = new SortedList<int, RenderTexture>();
    private SortedList<int, Transform> cameras = new SortedList<int, Transform>();
    private bool initialized = false;

    public SortedList<int, RenderTexture> CameraFeedRenderTexture { get { return cameraFeedRenderTexture; } }
    public SortedList<int, Transform> Cameras { get { return cameras; } }
    public bool Initialized { get { return initialized; } }

    void Start()
    {
        foreach (Transform camera in transform) // Get all cameras in the camera grp
        {
            cameras.Add(cameras.Count, camera); // Add to camera list
        }

        for (int i = 0; i < cameras.Keys.Count; i++)
        {
            int key = cameras.Keys[i];
            CreateRenderTexture(key); // Create render texture for the camera
        }

        initialized = true; // For other scripts to get the values when its ready
    }

    public void CreateRenderTexture(int key)
    {
        RenderTexture renderTexture = new RenderTexture(1980, 1080, 16);
        renderTexture.Create();
        cameras[key].Find("Camera").GetComponent<Camera>().targetTexture = renderTexture; // Set whatever the camera is rendering to a texture
        cameraFeedRenderTexture[key] = renderTexture; // Assign the render texture to that camera in the list
    }
}