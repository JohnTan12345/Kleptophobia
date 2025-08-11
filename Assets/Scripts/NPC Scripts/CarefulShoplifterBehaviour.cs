//===========================================================================================================
// Author: Foong Mun Yip Xander
// Created: 1 August 2025
// Description: Careful Shoplifter NPC behaviour. Intended Behaviour: steal something and leave immediately
//===========================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarefulShoplifterBehaviour : MonoBehaviour, NPCBehaviour
{

    private List<Transform> shelvesPoints;
    private List<Transform> spawnpoints;
    private List<GameObject> items;
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
    private Collider[] customersNearby = new Collider[10];
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
        npcLayer = LayerMask.GetMask("NPCs"); // for use to check for only NPCs later
        navMeshAgent = GetComponent<NavMeshAgent>();
        browsinglength = Random.Range(1, shelvesPoints.Count);
        StartCoroutine(ShopActivities());
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

    private IEnumerator ShopActivities()
    {
        if (browsinglength > 0 && !stoleItem) // Browsing Shelves and checking if he actually stole an item
        {
            int shelfnumber = Random.Range(0, shelvesPoints.Count - 1);

            while (!reachedDestination) // Go To Shelves
            {
                targetDestination = shelvesPoints[shelfnumber];
                ToDestination();
                yield return null;
            }

            shelvesPoints.Remove(shelvesPoints[shelfnumber]); // Remove shelf from list after reaching
            reachedDestination = false;
            browsinglength--;
            StartCoroutine(Idle());
        }
        else
        {
            targetDestination = spawnpoints[Random.Range(0, spawnpoints.Count - 1)];
            while (!reachedDestination) // Go home
            {
                ToDestination();
                yield return null;
            }
        }
    }

    private bool AreNPCsNearby() // Check if there are any NPCs
    {
        int customersNearbyInt = Physics.OverlapSphereNonAlloc(transform.position, npcDetectionRadius, customersNearby, npcLayer);
        return customersNearbyInt > 1; // If more than 1 (self), others are nearby
    }

    private IEnumerator Idle() // Doing absolutely nothing but stand around there
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        if (stoleItem || Random.Range(0f, 1f) < .5f) // Check if he already stole an item
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
        if (AreNPCsNearby()) // If there are people when its stealing
        {
            animator.ResetTrigger("Idle"); animator.ResetTrigger("Walk"); animator.ResetTrigger("Run");
            animator.SetTrigger("Take");
            Debug.Log(string.Format("Careful Shoplifter {0} stole something", gameObject));
            StoleItem = true;
            Instantiate(items[Random.Range(0, items.Count - 1)], itemSlot.position, itemSlot.rotation, itemSlot);
            yield return new WaitForSeconds(Random.Range(3f, 4f));
        }
        StartCoroutine(ShopActivities());
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
