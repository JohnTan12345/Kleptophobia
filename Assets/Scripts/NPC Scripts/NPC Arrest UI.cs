//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 7 August 2025
// Description: Arrest UI
//===========================================================================================================
using UnityEngine;

public class NPCArrestUI : MonoBehaviour
{
    private NPCBehaviour NPCBehaviour; // Get the NPC script regardless of NPC type
    public bool Arrested { get { return NPCBehaviour.Arrested; } set { NPCBehaviour.Arrested = value; } } // Access the NPC Arrested variable

    void Start()
    {
        NPCBehaviour = transform.parent.GetComponent<NPCBehaviour>();
    }
    void LateUpdate()
    {
        if (gameObject.activeSelf) // GUI auto alignment
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}
