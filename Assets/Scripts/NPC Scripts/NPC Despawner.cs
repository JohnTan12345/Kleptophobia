//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 26 July 2025
// Description: Removes NPCs
//===========================================================================================================
using UnityEngine;

public class NPCDespawner : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        GameObject NPC = other.gameObject;
        if (!NPC.CompareTag("Player") && !NPC.GetComponent<DebounceScript>().debounce)
        {
            GetComponent<NPCSpawner>().NPCList.Remove(NPC);
            Destroy(NPC);
        }
    }
}
