//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 25 July 2025
// Description: Spawns NPCs
//===========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public float NPCSpawnrate = 0f;
    public int totalGuilty = 0;
    public int totalNPC = 1;
    private float guiltySpawnChance = 0f;
    private int totalSpawnedGuilty = 0;

    private int totalNPCSpawned = 0;

    private GameObject targetNPCToSpawn;
    private string targetNPCType;
    private List<Transform> spawnpoints = new List<Transform> { };
    public List<GameObject> NPCList = new List<GameObject> { };
    public List<GameObject> PossibleItems = new List<GameObject> { };

    // Lists to carry over
    private List<Transform> shelvesPoints = new List<Transform> { };
    private List<Transform> registerPoints = new List<Transform> { };

    public GameObject[] NPCPrefabs = {};

    public GameObject NPCGroup;
    public GameObject shelvesGroup;
    public GameObject registerGroup;

    // For Testing Purposes
    [Header("Testing Parameters")]
    public bool testing = false;
    public bool spawnInnocents = true;
    public string NPCOption = "";
    [SerializeField]
    private bool spawnedNPC = false;

    void Awake()
    {
        // Add all points into the list
        foreach (Transform spawnpoint in transform)
        {
            spawnpoints.Add(spawnpoint);
        }
        ;

        foreach (Transform shelf in shelvesGroup.transform) // May change to tag based
        {
            shelvesPoints.Add(shelf.Find("Destination"));
        };

        foreach (Transform register in registerGroup.transform) // May change to tag based
        {
            registerPoints.Add(register.Find("Destination"));
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
                if (!testing)
                {
                    if (totalSpawnedGuilty < totalGuilty && Random.Range(0f, 1f) < guiltySpawnChance)
                    {

                        switch (Random.Range(1, 4)) // Change and add based on max types of guilty NPCs
                        {
                            case 1:
                                targetNPCType = "careless";
                                break;
                            case 2:
                                targetNPCType = "scared";
                                break;
                            case 3:
                                targetNPCType = "careful";
                                break;
                        }

                        guiltySpawnChance = 0f;

                    }
                    else
                    {
                        targetNPCType = "innocent";
                        guiltySpawnChance += 0.05f;
                    }
                }
                else
                {
                    if (!spawnedNPC)
                    {
                        targetNPCType = NPCOption.ToLower();

                        spawnedNPC = true;
                    }
                    else if (spawnInnocents == true)
                    {
                        targetNPCType = "innocent";
                    }
                }

                // Spawn NPC
                targetNPCToSpawn = NPCPrefabs[Random.Range(0, NPCPrefabs.Length - 1)];
                Transform targetTransform = spawnpoints[Mathf.RoundToInt(Random.Range(0f, spawnpoints.Count - 1))];
                GameObject NPC = Instantiate(targetNPCToSpawn, targetTransform.position, targetTransform.rotation);
                if (NPC.tag != "Customer")
                {
                    NPC.tag = "Customer";
                }
                NPC.transform.parent = NPCGroup.transform;

                if (targetNPCType == "innocent") // Add behaviour script
                {
                    InnocentNPCBehaviour innocentNPCBehaviour = NPC.AddComponent<InnocentNPCBehaviour>();
                    innocentNPCBehaviour.RegisterPoints = registerPoints;
                }
                else if (targetNPCType == "careless")
                {
                    NPC.AddComponent<CarelessShoplifterBehaviour>();
                }
                else if (targetNPCType == "scared")
                {
                    NPC.AddComponent<ScaredNPCBehaviour>();
                }
                else if (targetNPCType == "careful")
                {
                    NPC.AddComponent<CarefulShoplifterBehaviour>();
                }

                NPCBehaviour nPCBehaviour = NPC.GetComponent<NPCBehaviour>(); // Get behaviour script
                nPCBehaviour.ShelvesPoints = shelvesPoints;
                nPCBehaviour.Spawnpoints = spawnpoints;
                nPCBehaviour.NPCSpawner = this;
                nPCBehaviour.Items = PossibleItems;
                nPCBehaviour.ItemSlot = NPC.GetComponent<NPCVariable>().ItemSlot.transform;

                NPCList.Add(NPC);
                DebounceScript debounce = NPC.AddComponent<DebounceScript>(); // Add in a "recently spawned" check that disables itself after a certain time
                NPC.name = totalNPCSpawned++ + NPC.name;
                debounce.Debounce(2f);
            }
        }
    }
    
    
}