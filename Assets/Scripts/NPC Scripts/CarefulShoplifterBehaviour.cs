//===========================================================================================================
// Author: Foong Mun Yip Xander
// Created: 1 August 2025
// Description: Careful Shoplifter NPC behaviour
//===========================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarefulShoplifterBehaviour : MonoBehaviour, NPCBehaviour
{

    private List<Transform> shelvesPoints;
    private List<Transform> spawnpoints;
    private NPCSpawner nPCSpawner;
    private bool reachedDestination = false;
    private int browsinglength;
    private Transform targetDestination;
    private Vector3 currentTargetDestination = Vector3.zero;
    public float npcDetectionRadius = 5f;
    public LayerMask npcLayer;
    private bool arrested = false;
    private NavMeshAgent navMeshAgent;
    private bool stoleItem = false;
    private int points = 5;
    private float trackedTime = 0;

    public bool Arrested { get { return arrested; } set { arrested = value; StartCoroutine(OnArrest()); } }
    public bool StoleItem {get { return stoleItem; } set { stoleItem = value; }}
    public int Points {get { return points; }}
    public List<Transform> ShelvesPoints {set { shelvesPoints = new List<Transform>(value); }}
    public List<Transform> Spawnpoints { set { spawnpoints = new List<Transform>(value); }}
    public NPCSpawner NPCSpawner {set { nPCSpawner = value; }}

    void Start()
    {
        npcLayer = LayerMask.GetMask("NPCs");
        navMeshAgent = GetComponent<NavMeshAgent>();
        browsinglength = Random.Range(1, shelvesPoints.Count);
        StartCoroutine(ShopActivities());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetDestination.gameObject)
        {
            reachedDestination = true;
        }
    }

    void Update()
    {
        if (stoleItem)
        {
            trackedTime += Time.deltaTime;
        }
    }

    private IEnumerator ShopActivities()
    {
        if (browsinglength > 0 && !stoleItem) // Browsing Shelves
        {
            int shelfnumber = Random.Range(0, shelvesPoints.Count - 1);

            while (!reachedDestination) // Go To Shelves
            {
                targetDestination = shelvesPoints[shelfnumber];
                ToDestination();
                yield return null;
            }

            shelvesPoints.Remove(shelvesPoints[shelfnumber]);
            reachedDestination = false;
            browsinglength--;
            StartCoroutine(Idle());
        }
        else // Attempt to Steal if NPCs are not around
        {
            while (!reachedDestination)
            {
                targetDestination = spawnpoints[Random.Range(0, spawnpoints.Count - 1)];
                ToDestination();
                yield return null;
            }
        }
    }

    private bool AreNPCsNearby()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, npcDetectionRadius, npcLayer);
        return hitColliders.Length > 1; // If more than 1 (self), others are nearby
    }

    private IEnumerator Idle() // For animations or waiting
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        if (Random.Range(0f, 1f) < .5f)
        {
            StartCoroutine(ShopActivities());
        }
        else
        {
            StartCoroutine(Stealing());
        }
        
    }

    private IEnumerator Stealing()
    {
        Debug.Log(string.Format("Careful Shoplifter {0} stole something", gameObject));
        if (AreNPCsNearby()) // Wait until there is people
        {
            StoleItem = true;
            yield return new WaitForSeconds(Random.Range(1f, 2f));
        }
        StartCoroutine(ShopActivities());
    }

    private void ToDestination() // Go to destination point
    {
        if (arrested) { return; }
        if (targetDestination.position != currentTargetDestination)
        {
            currentTargetDestination = targetDestination.position;
            navMeshAgent.SetDestination(targetDestination.position);
        }
    }
    
    private IEnumerator OnArrest()
    {
        navMeshAgent.enabled = false;
        PointsScript.ModifyPoints(stoleItem, points, Mathf.RoundToInt(trackedTime));
        yield return new WaitForSeconds(2f);
        nPCSpawner.NPCList.Remove(gameObject);
        Destroy(gameObject);
    }
}
