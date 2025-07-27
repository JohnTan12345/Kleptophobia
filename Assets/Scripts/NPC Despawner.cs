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
