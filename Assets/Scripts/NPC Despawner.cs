using UnityEngine;

public class NPCDespawner : MonoBehaviour
{
    private DebounceScript debounce;

    public GameObject Spawnpoints;

    void OnTriggerEnter(Collider other)
    {
        GameObject NPC = other.gameObject;
        if (!(NPC.CompareTag("Player") && NPC.GetComponent<DebounceScript>().debounce))
        {
            Spawnpoints.GetComponent<NPCSpawner>().NPCList.Remove(NPC);
            Destroy(NPC);
        }
    }
}
