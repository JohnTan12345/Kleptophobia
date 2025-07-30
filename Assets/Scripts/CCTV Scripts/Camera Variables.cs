using System.Collections.Generic;
using UnityEngine;

public class CameraVariables : MonoBehaviour
{
    public SortedList<int, RenderTexture> cameraFeedRenderTexture = new SortedList<int, RenderTexture>();
    public SortedList<int, Transform> cameras = new SortedList<int, Transform>();

    void Start()
    {
        foreach (Transform camera in transform)
        {
            cameras.Add(cameras.Count, camera);
        }

        foreach (int i in cameras.Keys)
        {
            RenderTexture renderTexture = new RenderTexture(1980, 1080, 16);
            renderTexture.Create();
            cameras[i].Find("Camera").GetComponent<Camera>().targetTexture = renderTexture;
            cameraFeedRenderTexture.Add(i, renderTexture);
        }
    }
}
