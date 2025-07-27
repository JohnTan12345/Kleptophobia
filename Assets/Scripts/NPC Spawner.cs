using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public float NPCSpawnrate = 0f;
    public int totalGuilty = 0;
    public int totalNPC = 1;

    private GameObject GameManager;
    private GameData gameData;
    private float guiltySpawnChance = 0f;
    private int totalSpawnedGuilty = 0;
    private GameObject targetNPCToSpawn;
    private List<Transform> spawnpoints = new List<Transform> { };
    public List<GameObject> NPCList = new List<GameObject> { };

    // Lists to carry over
    private List<Transform> shelvesPoints = new List<Transform> { };
    private List<Transform> registerPoints = new List<Transform> { };

    public GameObject Innocent;
    public GameObject Careless;
    public GameObject Average;
    public GameObject Careful;

    public GameObject NPCGroup;
    public GameObject shelvesGroup;
    public GameObject registerGroup;

    void Awake()
    {
        GameManager = GameObject.FindGameObjectsWithTag("GameManager")[0];

        foreach (Transform spawnpoint in transform)
        {
            spawnpoints.Add(spawnpoint);
        };

        foreach (Transform shelf in shelvesGroup.transform)
        {
            Debug.Log(shelf.Find("Destination"));
            shelvesPoints.Add(shelf.Find("Destination"));
        };

        foreach (Transform register in registerGroup.transform)
        {
            registerPoints.Add(register.Find("Destination"));
        };

        StartCoroutine(SpawnNPC());
    }

    private IEnumerator SpawnNPC()
    {
        while (true)
        {

            if (GameManager == null)
            {
                GameManager = GameObject.FindGameObjectWithTag("GameManager");
                gameData = GameManager.GetComponent<GameData>();
            }

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

                    guiltySpawnChance = 0f;

                }
                else
                {
                    targetNPCToSpawn = Innocent;
                    guiltySpawnChance += 0.01f;
                }

                Transform targetTransform = spawnpoints[Mathf.RoundToInt(Random.Range(0f, spawnpoints.Count - 1))];
                GameObject NPC = Instantiate(targetNPCToSpawn, targetTransform.position, targetTransform.rotation);
                NPC.transform.parent = NPCGroup.transform;

                if (targetNPCToSpawn == Innocent) // Add behaviour script
                {
                    InnocentNPCBehaviour innocentNPCBehaviour = NPC.AddComponent<InnocentNPCBehaviour>();

                    // Assign values
                    innocentNPCBehaviour.spawnpoints = spawnpoints;
                    innocentNPCBehaviour.shelvesPoints = shelvesPoints;
                    innocentNPCBehaviour.registerPoints = registerPoints;
                }
                else if (targetNPCToSpawn == Careless)
                {

                }
                else if (targetNPCToSpawn == Average)
                {

                }
                else if (targetNPCToSpawn == Careful)
                {

                }

                NPCList.Add(NPC);
                DebounceScript debounce = NPC.AddComponent<DebounceScript>();
                debounce.Debounce(2f);
            }
        }
    }
    
    
}