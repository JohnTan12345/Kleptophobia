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
    private List<GameObject> items;
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
    private Animator animator;
    private Transform itemSlot;

    public bool Arrested { get { return arrested; } set { arrested = value; StartCoroutine(OnArrest()); } }
    public bool StoleItem {get { return stoleItem; } set { stoleItem = value; }}
    public int Points {get { return points; }}
    public List<Transform> ShelvesPoints {set { shelvesPoints = new List<Transform>(value); }}
    public List<Transform> Spawnpoints { set { spawnpoints = new List<Transform>(value); }}
    public List<GameObject> Items {set{ items = new List<GameObject>(value); }}    
    public NPCSpawner NPCSpawner { set { nPCSpawner = value; } }
    public Transform ItemSlot {set { itemSlot = value; }}
    void Start()
    {
        animator = transform.Find("Rig").GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        browsingLength = Random.Range(1, shelvesPoints.Count);
        StartCoroutine(BrowseShelves());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetDestination.gameObject)
        {
            animator.ResetTrigger("Walk"); animator.ResetTrigger("Run"); animator.SetTrigger("Take");
            animator.SetTrigger("Idle");
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
            targetDestination = spawnpoints[Random.Range(0, spawnpoints.Count - 1)];
            while (!reachedDestination) // Go home
            {
                if (arrested) { yield break; }
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
        animator.ResetTrigger("Idle"); animator.ResetTrigger("Walk"); animator.ResetTrigger("Run");
        animator.SetTrigger("Take");
        Debug.Log(string.Format("Careless Shoplifter {0} stole something", gameObject));
        StoleItem = true;
        Instantiate(items[Random.Range(0, items.Count - 1)], itemSlot.position, itemSlot.rotation, itemSlot);
        yield return new WaitForSeconds(Random.Range(3f, 4f));
        StartCoroutine(BrowseShelves());

        //if (itemPrefab && handSlot)
        //{
        //    Instantiate(itemPrefab, handSlot.position, handSlot.rotation, handSlot);
        //} For use later...
    }

    private void ToDestination() // Go to destination point
    {
        if (arrested) { return; }
        if (targetDestination.position != currentTargetDestination)
        {
        animator.ResetTrigger("Idle"); animator.ResetTrigger("Take"); animator.ResetTrigger("Run");
        animator.SetTrigger("Walk");
        currentTargetDestination = targetDestination.position;
        navMeshAgent.SetDestination(targetDestination.position);
        }
    }
    
    private IEnumerator OnArrest()
    {
        animator.ResetTrigger("Idle"); animator.ResetTrigger("Walk"); animator.ResetTrigger("Take"); animator.ResetTrigger("Run"); // Stop all animations
        animator.SetTrigger("Arrested");
        navMeshAgent.enabled = false; // Stop the NPC completely
        PointsScript.ModifyPoints(stoleItem, points); // Add or remove points
        yield return new WaitForSeconds(6f);
        nPCSpawner.NPCList.Remove(gameObject); // To allow new NPCs to spawn
        Destroy(gameObject);
    }
}