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
        foreach (Transform camera in transform)
        {
            cameras.Add(cameras.Count, camera);
        }

        for (int i = 0; i < cameras.Keys.Count; i++)
        {
            int key = cameras.Keys[i];
            CreateRenderTexture(key);
        }

        initialized = true;
    }

    public void CreateRenderTexture(int key)
    {
        RenderTexture renderTexture = new RenderTexture(1980, 1080, 16);
        renderTexture.Create();
        cameras[key].Find("Camera").GetComponent<Camera>().targetTexture = renderTexture;
        cameraFeedRenderTexture[key] = renderTexture;
    }
}