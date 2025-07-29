//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 25 July 2025
// Description: Innocent NPC behaviour
//===========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InnocentNPCBehaviour : MonoBehaviour
{
    private List<Transform> shelvesPoints;
    private List<Transform> registerPoints;
    private List<Transform> spawnpoints;
    private bool reachedDestination = false;
    private int browsinglength;
    private Transform targetDestination;
    public bool checkingOut;

    public Transform TargetDestination { get { return targetDestination; } }

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
        browsinglength = Mathf.RoundToInt(Random.Range(1, shelvesPoints.Count));
        StartCoroutine(ShopActivities());
    }

    void OnTriggerEnter(Collider other)
    {
        if (targetDestination != null && other.gameObject == targetDestination.gameObject)
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
        else // Checkout
        {
            Transform register = null;
            SortedList<GameObject, int> customersInLine = null;
            checkingOut = true;
            bool firstInLine = false;
            Transform registerDestination = null;

            while (!reachedDestination || !firstInLine) // Update to queue up eventually
            {

                if (checkingOut && (register == null || customersInLine.Count > 5))
                {
                    Debug.Log("E");
                    register = GetAvailableRegister();

                    if (register == null)
                    {
                        yield return new WaitForSecondsRealtime(.2f);
                        continue;
                    }

                    customersInLine = register.GetComponent<RegisterBehaviour>().CustomersInLine;
                    registerDestination = register.Find("Destination");
                }

                if (!firstInLine && targetDestination == registerDestination)
                    {
                        firstInLine = true;
                    }

                customersInLine.TryGetValue(gameObject, out int i);
                if (!customersInLine.ContainsKey(gameObject))
                {
                    targetDestination = register;
                    ToDestination();
                }
                else if (customersInLine.Count == 0 || i == 0)
                {
                    targetDestination = registerDestination;
                    ToDestination();
                }
                else if (customersInLine.Count > 0 && i != 0)
                {
                    targetDestination = register.Find(string.Format("Waiting {0}", i));
                    ToDestination();
                }
                yield return null;
            }

            reachedDestination = false;
            yield return StartCoroutine(Idle());
            checkingOut = false;

            while (!reachedDestination)
            {
                targetDestination = spawnpoints[Mathf.RoundToInt(Random.Range(0, spawnpoints.Count - 1))];
                ToDestination();
                yield return null;
            }
        }
    }

    private IEnumerator Idle() // Planned for animations
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        StartCoroutine(ShopActivities());
    }

    private void ToDestination() // Go to destination point
    {
        GetComponent<NavMeshAgent>().SetDestination(targetDestination.position);
    }

    private Transform GetAvailableRegister() // Get the first available register
    {
        Transform availableRegister = null;
        Transform shortestLineRegister = null;
        int shortestLineAmount = 5;

        foreach (Transform registerPoint in registerPoints)
        {
            Transform register = registerPoint.parent;
            RegisterBehaviour registerBehaviour = register.GetComponent<RegisterBehaviour>();
            if (registerBehaviour.available)
            {
                registerBehaviour.available = false;
                availableRegister = register;
            }
            else
            {
                if (registerBehaviour.CustomersInLine.Count < shortestLineAmount)
                {
                    shortestLineAmount = registerBehaviour.CustomersInLine.Count;
                    shortestLineRegister = register;
                }
            }
        }

        if (availableRegister == null)
        {
            availableRegister = shortestLineRegister;
        }

        return availableRegister;
    }
}
