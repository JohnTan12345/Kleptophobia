using UnityEngine;

public class NPCArrestUI : MonoBehaviour
{
    private NPCBehaviour NPCBehaviour;
    public bool Arrested { get { return NPCBehaviour.Arrested; } set { NPCBehaviour.Arrested = value; } }

    void Start()
    {
        NPCBehaviour = transform.parent.GetComponent<NPCBehaviour>();
    }
    void LateUpdate()
    {
        if (gameObject.activeSelf)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
