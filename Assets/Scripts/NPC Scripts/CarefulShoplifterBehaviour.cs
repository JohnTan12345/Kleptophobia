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
    private List<Transform> registerPoints;
    private List<Transform> spawnpoints;
    public NPCSpawner NPCSpawner;
    private bool reachedDestination = false;
    private int browsinglength;
    private Transform targetDestination;
    private Vector3 currentTargetDestination = Vector3.zero;
    public float npcDetectionRadius = 5f;
    public LayerMask npcLayer;
    private bool arrested = false;
    private NavMeshAgent navMeshAgent;

    public bool Arrested { get { return arrested; } set { arrested = value; StartCoroutine(OnArrest()); } }

    public List<Transform> ShelvesPoints
    {
        set
        {
            shelvesPoints = new List<Transform>(value);
        }
    }

    public List<Transform> RegisterPoints
    {
        set
        {
            registerPoints = new List<Transform>(value);
        }
    }

    public List<Transform> Spawnpoints
    {
        set
        {
            spawnpoints = new List<Transform>(value);
        }
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        browsinglength = Mathf.RoundToInt(Random.Range(1, shelvesPoints.Count));
        StartCoroutine(ShopActivities());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetDestination.gameObject)
        {
            reachedDestination = true;
        }
    }

    private IEnumerator ShopActivities()
    {
        if (browsinglength > 0) // Browsing Shelves
        {
            int shelfnumber = Mathf.RoundToInt(Random.Range(0, shelvesPoints.Count - 1));

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
            while (AreNPCsNearby()) // Wait until area is clear
            {
                yield return new WaitForSeconds(1f);
            }

            // Steal logic here
            Debug.Log(name + " is stealing an item!");

            yield return new WaitForSeconds(Random.Range(1f, 2f)); // Simulate stealing time

            while (!reachedDestination)
            {
                targetDestination = spawnpoints[Mathf.RoundToInt(Random.Range(0, spawnpoints.Count - 1))];
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
        yield return new WaitForSeconds(2f);
        NPCSpawner.NPCList.Remove(gameObject);
        Destroy(gameObject);
    }
}
