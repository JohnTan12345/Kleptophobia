using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public float NPCSpawnrate = 0f;
    public int totalGuilty = 0;
    public int totalNPC = 50;

    private float guiltySpawnChance = 0f;
    private int totalSpawnedGuilty = 0;
    private GameObject targetNPCToSpawn;
    private List<GameObject> NPCList = new List<GameObject> {};
    private List<Transform> spawnpoints = new List<Transform> {};

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
        foreach (Transform spawnpoint in transform)
        {
            spawnpoints.Add(spawnpoint);
        };
        StartCoroutine(SpawnNPC());
    }

    private IEnumerator SpawnNPC()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, NPCSpawnrate));

            if (NPCList.Count < totalNPC)
            {
                if (totalSpawnedGuilty < totalGuilty && Random.Range(0f, 1f) < guiltySpawnChance)
                {

                    switch (Random.Range(0f, 3f)) // Change and add based on max types of guilty NPCs
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

                    guiltySpawnChance += 0.01f;
                }
                else
                {
                    targetNPCToSpawn = Innocent;
                }

                
                GameObject NPC = Instantiate(targetNPCToSpawn);
            }
        }
    }
}