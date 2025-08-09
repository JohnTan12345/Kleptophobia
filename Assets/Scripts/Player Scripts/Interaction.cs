//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 7 August 2025
// Description: Interaction Mechanics
//===========================================================================================================
using StarterAssets;
using Unity.VisualScripting;
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
    private bool usingCams = false;
    private Camera playerCamera;
    private FirstPersonController firstPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Transform playerFollowCamera;
    private CanvasGroup playerUI;

    // Camera Original Settings
    private Transform cameraParent;
    private Vector3 cameraPosition;
    private Quaternion cameraRotation;

    void Start()
    {
        firstPersonController = GetComponent<FirstPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        playerCamera = Camera.main;
        playerUI = playerCamera.transform.Find("Player GUI Canvas").GetComponent<CanvasGroup>();
        playerFollowCamera = playerCamera.transform.parent.Find("PlayerFollowCamera");
    }

    void Update()
    {
        if (Physics.Raycast(raySpawner.position, raySpawner.forward, out raycastHit, maxRange))
        {
            GameObject hitObject = raycastHit.collider.gameObject;
            if (hitObject != onScreen)
            {
                ResetVariables();
                onScreen = hitObject;

                if (hitObject.transform.Find("Canvas") != null && hitObject.transform.Find("Canvas").TryGetComponent<NPCArrestUI>(out arrestUI))
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
        if (usingCams)
        {
            ExitCameras();
        }
        else if (onScreen != null && !usingCams && onScreen.CompareTag("Monitor"))
        {
            UseCameras();
        }
        else if (arrestUI != null && !arrestUI.Arrested)
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

    private void UseCameras()
    {
        cameraParent = playerCamera.transform.parent;
        cameraPosition = playerCamera.transform.position;
        cameraRotation = playerCamera.transform.rotation;

        SwitchPlayerStates(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerUI.alpha = 0f;
        playerUI.interactable = false;
        playerUI.blocksRaycasts = false;

        playerCamera.transform.parent = null;
        playerCamera.transform.position = idealCameraPosition.position;
        playerCamera.transform.rotation = idealCameraPosition.rotation;
    }

    private void ExitCameras()
    {
        playerCamera.transform.position = cameraPosition;
        playerCamera.transform.rotation = cameraRotation;
        playerCamera.transform.parent = cameraParent;

        SwitchPlayerStates(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerUI.alpha = 1f;
        playerUI.interactable = true;
        playerUI.blocksRaycasts = true;
    }

    private void SwitchPlayerStates(bool state)
    {
        playerFollowCamera.gameObject.SetActive(state);
        firstPersonController.enabled = state;
        starterAssetsInputs.cursorInputForLook = state;
        starterAssetsInputs.cursorLocked = state;

        usingCams = !state;
    }

}
