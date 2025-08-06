using UnityEngine;

public class Interaction : MonoBehaviour
{
    public Transform raySpawner;
    public float maxRange;
    private RaycastHit raycastHit;
    private GameObject onScreen;
    private GameObject arrestUIGameObject;
    private NPCArrestUI arrestUI;

    void Update()
    {
        if (Physics.Raycast(raySpawner.position, raySpawner.forward, out raycastHit, maxRange))
        {
            GameObject hitObject = raycastHit.collider.gameObject;
            if (hitObject != onScreen)
            {
                ResetVariables();
                onScreen = hitObject;

                try
                {
                    arrestUIGameObject = hitObject.transform.Find("Canvas").gameObject;
                    arrestUI = arrestUIGameObject.GetComponent<NPCArrestUI>();
                    arrestUIGameObject.SetActive(true);
                } catch {}
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
