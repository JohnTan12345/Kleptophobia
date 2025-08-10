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
        if (!NPC.CompareTag("Player") && !NPC.GetComponent<DebounceScript>().debounce) // Check if NPC just spawned
        {
            if (NPC.GetComponent<NPCBehaviour>().StoleItem) // Check if NPC stole an item
            {
                PointsScript.escaped++;
                Debug.Log("Shoplifter Escaped!");
            }
            GetComponent<NPCSpawner>().NPCList.Remove(NPC); // Remove NPC from list to allow new NPCs to spawn
            Destroy(NPC); // Destroy NPC
        }
    }
}
