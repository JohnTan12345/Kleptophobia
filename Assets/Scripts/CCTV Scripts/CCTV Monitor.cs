using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CCTVMonitor : MonoBehaviour
{
    public GameObject CameraGrp;

    private int cameraNumber = 0;
    private SortedList<int, RenderTexture> cameraFeedRenderTexture;
    private SortedList<int, Transform> cameras;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        cameraFeedRenderTexture = CameraGrp.GetComponent<CameraVariables>().cameraFeedRenderTexture;
        cameras = CameraGrp.GetComponent<CameraVariables>().cameras;

        if (!cameraFeedRenderTexture[cameraNumber].IsCreated())
        {
            RenderTexture renderTexture = new(1980, 1080, 16);
            renderTexture.Create();
            cameras[cameraNumber].GetComponent<Camera>().targetTexture = renderTexture;
        }

        meshRenderer.material.SetTexture("_MainTex", cameraFeedRenderTexture[cameraNumber]);
    }
}
