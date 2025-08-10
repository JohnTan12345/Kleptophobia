//===========================================================================================================
// Author: Lucas Tan
// Created: 1 August 2025
// Description: Careless shoplifter NPC behaviour. Intended behaviour: act like a normal customer but with the chance to steal
//===========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarelessShoplifterBehaviour : MonoBehaviour, NPCBehaviour
{
    private List<Transform> shelvesPoints;
    private List<Transform> spawnpoints;
    private NPCSpawner nPCSpawner;
    private int browsingLength;
    private bool reachedDestination = false;
    private Transform targetDestination;
    private Vector3 currentTargetDestination = Vector3.zero;

    [SerializeField] private Transform handSlot; // Where the item will appear in-hand
    [SerializeField] private GameObject itemPrefab; // The visual item they "shoplift"

    private bool arrested = false;
    private NavMeshAgent navMeshAgent;
    private bool stoleItem = false;
    private int points = 2;
    private float trackedTime = 0;

    public bool Arrested { get { return arrested; } set { arrested = value; StartCoroutine(OnArrest()); } }
    public bool StoleItem {get { return stoleItem; } set { stoleItem = value; }}
    public int Points {get { return points; }}
    public List<Transform> ShelvesPoints {set { shelvesPoints = new List<Transform>(value); }}
    public List<Transform> Spawnpoints { set { spawnpoints = new List<Transform>(value); }}
    public NPCSpawner NPCSpawner { set { nPCSpawner = value; } }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        browsingLength = Random.Range(1, shelvesPoints.Count);
        StartCoroutine(BrowseShelves());
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
        if (stoleItem) // Start timer after NPC steals item
        {
            trackedTime += Time.deltaTime;
        }
    }

    private IEnumerator BrowseShelves()
    {
        if (browsingLength > 0) // Browsing Shelves
        {
            int shelfIndex = Random.Range(0, shelvesPoints.Count);
            targetDestination = shelvesPoints[shelfIndex];

            while (!reachedDestination)
            {
                if (arrested) { yield break; }
                ToDestination();
                yield return null;
            }

            shelvesPoints.Remove(shelvesPoints[shelfIndex]); // Remove shelf from list after reaching
            reachedDestination = false;
            browsingLength--;
            if (!stoleItem && Random.Range(0f, 1f) < 0.5f) // Check if he already stole an item
            {
                StartCoroutine(Stealing());
            }
            else
            {
                StartCoroutine(Idle());
            }
        }
        else
        {
            while (!reachedDestination) // Go home
            {
                if (arrested) { yield break; }

                targetDestination = spawnpoints[Random.Range(0, spawnpoints.Count - 1)];
                ToDestination();
                yield return null;
            }
        }

    }

    private IEnumerator Idle() // Doing absolutely nothing but stand around there
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
        StartCoroutine(BrowseShelves());
    }

    private IEnumerator Stealing()
    {
        Debug.Log(string.Format("Careless Shoplifter {0} stole something", gameObject));
        StoleItem = true;
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        StartCoroutine(BrowseShelves());

        //if (itemPrefab && handSlot)
        //{
        //    Instantiate(itemPrefab, handSlot.position, handSlot.rotation, handSlot);
        //} For use later...
    }

    private void ToDestination() // Go to destination point
    {
        if (arrested) { return; }
        if (targetDestination.position != currentTargetDestination) // Check if the new destination point is actually new
        {
            currentTargetDestination = targetDestination.position;
            navMeshAgent.SetDestination(targetDestination.position);
        }
    }
    
    private IEnumerator OnArrest()
    {
        navMeshAgent.enabled = false; // Stop the NPC completely
        PointsScript.ModifyPoints(stoleItem, points, Mathf.RoundToInt(trackedTime)); // Add or remove points
        yield return new WaitForSeconds(2f);
        nPCSpawner.NPCList.Remove(gameObject); // To allow new NPCs to spawn
        Destroy(gameObject);
    }
}