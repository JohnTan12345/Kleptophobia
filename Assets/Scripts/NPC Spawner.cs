using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public float NPCSpawnrate = 0f;
    public int totalGuilty = 0;

    private float guiltySpawnChance = 0f;
    private int totalSpawnedGuilty = 0;
    private GameObject targetNPCToSpawn;
    private List<GameObject> NPCList = new List<GameObject> {};

    [SerializeField]
    static GameObject Innocent;
    [SerializeField]
    static GameObject Careless;
    [SerializeField]
    static GameObject Average;
    [SerializeField]
    static GameObject Careful;

    void Awake()
    {
        StartCoroutine(SpawnNPC());
    }

    private IEnumerator SpawnNPC()
    {
        yield return new WaitForSeconds(Random.Range(0f, NPCSpawnrate));


        if (totalSpawnedGuilty < totalGuilty && Random.Range(0f, 1f) < guiltySpawnChance)
            {
                float i = Random.Range(0f, 3f); // Change based on max types of guilty NPCs

                switch (i) // Add based on max types of guilty NPCs
                {
                    case < 1f:
                        targetNPCToSpawn = Careless;
                        break;
                    case < 2f:
                        targetNPCToSpawn = Average;
                        break;
                    case < 3f:
                        targetNPCToSpawn = Careful;
                        break;
                }
            }
            else
            {
                targetNPCToSpawn = Innocent;
            }

        Instantiate(targetNPCToSpawn);
    }
}
