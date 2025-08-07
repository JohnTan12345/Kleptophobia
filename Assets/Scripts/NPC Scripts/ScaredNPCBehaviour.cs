//===========================================================================================================
// Author: Chua Yi Xuan Rayner
// Created: 29 July 2025
// Description: Scared NPC Behaviour
//===========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScaredNPCBehaviour : MonoBehaviour
{
    private List<Transform> shelvesPoints;
    private List<Transform> exitPoints;

    private Transform targetDestination;
    private Transform targetExit;
    private int browsingLength;
    private bool reachedDestination = false;
    private bool isScared = false;

    private NavMeshAgent agent;
    private Rigidbody rb;

    private Coroutine browseRoutine;

    public List<Transform> ShelvesPoints
    {
        set { shelvesPoints = new List<Transform>(value); }
    }

    public List<Transform> ExitPoints
    {
        set { exitPoints = new List<Transform>(value); }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        browsingLength = Mathf.RoundToInt(Random.Range(1, shelvesPoints.Count));
        browseRoutine = StartCoroutine(BrowseShelves());
    }

     void OnTriggerEnter(Collider other)
    {
        if (targetDestination != null && other.gameObject == targetDestination.gameObject)
        {
            reachedDestination = true;
        } 
        if (other.CompareTag("Player") && !isScared)
        {
            GetScared();
        }
    }

    private IEnumerator BrowseShelves()
    {
        while (browsingLength > 0 && !isScared)
        {
            int shelfIndex = Random.Range(0, shelvesPoints.Count);
            targetDestination = shelvesPoints[shelfIndex];
            agent.SetDestination(targetDestination.position);
            reachedDestination = false;

            while (!reachedDestination && !isScared)
            {
                yield return null;
            }

            shelvesPoints.RemoveAt(shelfIndex);
            browsingLength--;
            yield return new WaitForSeconds(Random.Range(2f, 4f));
        }

        if (!isScared)
        {
            StartCoroutine(Wander());
        }
    }

    void Update()
    {
        // Flee is triggered after the jump, so no need to check here
    }

    private IEnumerator Wander()
    {
        while (!isScared)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            // Optional roaming or idle logic
        }
    }

    public void GetScared()
    {
        if (!isScared)
        {
            isScared = true;

            if (browseRoutine != null)
                StopCoroutine(browseRoutine);

            StartCoroutine(DelayedFear());
        }
    }

    private IEnumerator DelayedFear()
    {
        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

        JumpScare();

        // Brief pause to let jump resolve
        yield return new WaitForSeconds(0.75f);

        StartCoroutine(Flee());

    }

    private void JumpScare()
    {
        if (rb != null)
        {
            agent.isStopped = true; // Pause pathfinding temporarily
            rb.AddForce(Vector3.up * 7f, ForceMode.Impulse); // Scared jump
        }
    }

    private IEnumerator Flee()
    {
        targetExit = GetNearestExit();

        if (targetExit != null)
        {
            agent.isStopped = false;
            agent.SetDestination(targetExit.position);

            while (Vector3.Distance(transform.position, targetExit.position) > 1.5f)
            {
                yield return null;
            }
        }

        Debug.Log($"{gameObject.name} has fled!");
        Destroy(gameObject); // or gameObject.SetActive(false);
    }

    private Transform GetNearestExit()
    {
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform exit in exitPoints)
        {
            float dist = Vector3.Distance(transform.position, exit.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = exit;
            }
        }

        return nearest;
    }







}
