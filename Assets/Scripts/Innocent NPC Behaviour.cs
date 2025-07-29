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
    private Transform register = null;
    public bool checkingOut = false;

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
            bool firstInLine = false;
            while (true)
            {
                if (!firstInLine)
                {
                    reachedDestination = false;
                }
                else
                {
                    break;
                }

                while (!reachedDestination)
                {

                    while (register == null)
                    {
                        checkingOut = true;
                        register = GetAvailableRegister();
                        yield return null;
                    }


                    if (checkingOut)
                    {
                        RegisterVariables registerVariables = register.GetComponent<RegisterVariables>();

                        if (!registerVariables.CustomersInLine.ContainsValue(gameObject))
                        {
                            if (registerVariables.CustomersInLine.Count > 4)
                            {
                                register = null;
                                continue;
                            }
                            targetDestination = register.Find("Trigger Area");
                        }
                        else
                        {
                            int i = registerVariables.CustomersInLine.Keys[registerVariables.CustomersInLine.IndexOfValue(gameObject)];

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
                        Debug.Log(string.Format("{0}\ntarget: {1}", gameObject, targetDestination));
                    }
                    ToDestination();
                    yield return null;
                }
            }
            checkingOut = false;
            reachedDestination = false;
            yield return StartCoroutine(Idle());

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

        for (int i = 0; i < registerPoints.Count; i++)
        {
            Transform registerPoint = registerPoints[i];
            Transform register = registerPoint.parent;
            RegisterVariables registerVariables = register.GetComponent<RegisterVariables>();

            if (registerVariables.available)
            {
                availableRegister = register;
            }
            else
            {
                if (registerVariables.CustomersInLine.Count < shortestLineAmount)
                {
                    shortestLineAmount = registerVariables.CustomersInLine.Count;
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
