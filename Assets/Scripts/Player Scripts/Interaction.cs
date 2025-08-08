//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 7 August 2025
// Description: Interaction Mechanics
//===========================================================================================================
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform raySpawner;
    public float maxRange;
    private RaycastHit raycastHit;
    private GameObject onScreen;
    private GameObject arrestUIGameObject;
    private NPCArrestUI arrestUI;
    private Transform idealCameraPosition;

    void Update()
    {
        if (Physics.Raycast(raySpawner.position, raySpawner.forward, out raycastHit, maxRange))
        {
            GameObject hitObject = raycastHit.collider.gameObject;
            Debug.Log(hitObject.name);
            if (hitObject != onScreen)
            {
                ResetVariables();
                onScreen = hitObject;

                if (hitObject.transform.Find("Canvas").TryGetComponent<NPCArrestUI>(out arrestUI))
                {
                    arrestUIGameObject = hitObject.transform.Find("Canvas").gameObject;
                    arrestUIGameObject.SetActive(true);
                }
                else if (hitObject.CompareTag("Monitor"))
                {
                    idealCameraPosition = hitObject.transform.parent.Find("Camera Position");
                }
            }
        }
        else
        {
            ResetVariables();
        }
    }

    void OnInteract()
    {
        if (arrestUI != null && !arrestUI.Arrested)
        {
            arrestUI.Arrested = true;
        }
    }

    private void ResetVariables()
    {
        if (arrestUIGameObject != null)
        {
            arrestUIGameObject.SetActive(false);
        }

        arrestUIGameObject = null;
        arrestUI = null;
        onScreen = null;
    }
}
