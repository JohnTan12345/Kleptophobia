//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 25 July 2025
// Description: Innocent NPC behaviour
//===========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InnocentNPCBehaviour : MonoBehaviour, NPCBehaviour
{
    private List<Transform> shelvesPoints;
    private List<Transform> registerPoints;
    private List<Transform> spawnpoints;
    private List<GameObject> items;
    private NPCSpawner nPCSpawner;
    [SerializeField]
    private bool reachedDestination = false;
    [SerializeField]
    private int browsinglength;
    private Transform targetDestination;
    private Vector3 currentTargetDestination = Vector3.zero;
    private Transform register = null;
    public bool checkingOut = false;
    private bool arrested = false;
    private NavMeshAgent navMeshAgent;
    private bool stoleItem = false;
    private int points = -2;
    private Animator animator;
    private Transform itemSlot;
    public GameObject moneyVFX;
    private Transform VFXSpawn;

    public bool Arrested { get { return arrested; } set { arrested = value; StartCoroutine(OnArrest()); } }
    public bool StoleItem { get { return stoleItem; } set { stoleItem = value; }}
    public int Points { get { return points; }}
    public NPCSpawner NPCSpawner { set { nPCSpawner = value; }}
    public Transform TargetDestination { get { return targetDestination; }}
    public List<Transform> ShelvesPoints { set { shelvesPoints = new List<Transform>(value); }}
    public List<Transform> RegisterPoints { set { registerPoints = new List<Transform>(value); }}
    public List<Transform> Spawnpoints { set { spawnpoints = new List<Transform>(value); }}
    public List<GameObject> Items {set{ items = new List<GameObject>(value); }}    
    public Transform ItemSlot { set { itemSlot = value; } }

    void Start()
    {
        VFXSpawn = transform.Find("VFX");
        animator = transform.Find("Rig").GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        browsinglength = Random.Range(1, shelvesPoints.Count);
        StartCoroutine(ShopActivities());
    }

    void OnTriggerEnter(Collider other)
    {
        if (targetDestination != null && other.gameObject == targetDestination.gameObject)
        {
            animator.ResetTrigger("Walk"); animator.ResetTrigger("Run"); animator.SetTrigger("Take");
            animator.SetTrigger("Idle");
            reachedDestination = true;
        }
    }

    private IEnumerator ShopActivities()
    {
        if (browsinglength > 0) // Browsing Shelves
        {
            int shelfnumber = Random.Range(0, shelvesPoints.Count - 1);

            while (!reachedDestination) // Go To Shelves
            {
                if (arrested) { yield break; }
                targetDestination = shelvesPoints[shelfnumber];
                ToDestination();
                yield return null;
            }

            shelvesPoints.Remove(shelvesPoints[shelfnumber]);
            reachedDestination = false;
            browsinglength--;
            StartCoroutine(Idle());
        }
        else // Checkout
        {
            bool firstInLine = false;
            while (true)
            {
                if (arrested) { yield break; }

                if (!firstInLine) // If customer is not the first in line
                {
                    reachedDestination = false;
                }
                else
                {
                    break;
                }

                while (!reachedDestination)
                {
                    if (arrested) { yield break; }

                    while (register == null) // If there is no register for the NPC to go to
                    {
                        if (arrested) { yield break; }
                        checkingOut = true; // Put here so it does not get resetted to true
                        register = GetAvailableRegister();
                        yield return null;
                    }


                    if (checkingOut)
                    {
                        RegisterVariables registerVariables = register.GetComponent<RegisterVariables>();

                        if (!registerVariables.CustomersInLine.ContainsValue(gameObject))
                        {
                            if (registerVariables.CustomersInLine.Count > 4) // Check if register is full
                            {
                                register = null;
                                continue;
                            }
                            targetDestination = register.Find("Trigger Area");
                        }
                        else
                        {
                            int i = registerVariables.CustomersInLine.Keys[registerVariables.CustomersInLine.IndexOfValue(gameObject)]; // Get where the customer is standing in line

                            if (i != 0)
                            {
                                targetDestination = register.Find(string.Format("Waiting {0}", i));
                            }
                            else
                            {
                                targetDestination = register.Find("Destination");
                                firstInLine = true;
                            }
                        }
                    }
                    ToDestination();
                    yield return null;
                }
            }
            // To add VFX for paying later
            checkingOut = false;
            reachedDestination = false;
            GameObject vfx = Instantiate(moneyVFX, VFXSpawn.position, VFXSpawn.rotation, VFXSpawn);
            ParticleSystem vfxPS = vfx.GetComponent<ParticleSystem>();
            if (vfxPS != null)
            {
                vfxPS.Play();
            }
            
            yield return StartCoroutine(Idle());

            while (!reachedDestination)
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
        yield return new WaitForSeconds(Random.Range(2f, 5f));
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

    private Transform GetAvailableRegister() // Get the first available register
    {
        Transform availableRegister = null;
        Transform shortestLineRegister = null;
        int shortestLineAmount = 5;

        for (int i = 0; i < registerPoints.Count; i++)
        {
            Transform registerPoint = registerPoints[i];
            Transform register = registerPoint.parent;
            RegisterVariables registerVariables = register.GetComponent<RegisterVariables>();

            if (registerVariables.available)
            {
                availableRegister = register; // Set as the register thats available
            }
            else
            {
                if (registerVariables.CustomersInLine.Count < shortestLineAmount) // Find the shortest line
                {
                    shortestLineAmount = registerVariables.CustomersInLine.Count;
                    shortestLineRegister = register;
                }
            }
        }

        if (availableRegister == null) // Go to the shortest line if theres no free register
        {
            availableRegister = shortestLineRegister;
        }

        return availableRegister;
    }
}
