//===========================================================================================================
// Author: Chua Yi Xuan Rayner
// Created: 29 July 2025
// Description: Scared NPC Behaviour
//===========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScaredNPCBehaviour : MonoBehaviour, NPCBehaviour
{
    private List<Transform> shelvesPoints;
    private List<Transform> spawnpoints;
    private NPCSpawner nPCSpawner;
    private Transform targetDestination;
    private Vector3 currentTargetDestination = Vector3.zero;
    private int browsingLength;
    private bool reachedDestination = false;
    private bool isScared = false;
    private bool isFleeing = false;
    private bool arrested = false;
    private NavMeshAgent navMeshAgent;
    private bool stoleItem = false;
    private int points = 2;
    private Collider[] playerNearby = new Collider[1];
    private float playerNearbyTime;
    private float trackedTime = 0;

    public bool Arrested { get { return arrested; } set { arrested = value; StartCoroutine(OnArrest()); } }
    public bool StoleItem { get { return stoleItem; } set { stoleItem = value; } }
    public int Points { get { return points; } }
    public NPCSpawner NPCSpawner {set { nPCSpawner = value; }}
    public List<Transform> ShelvesPoints { set { shelvesPoints = new List<Transform>(value); }}
    public List<Transform> Spawnpoints { set { spawnpoints = new List<Transform>(value); }}

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        browsingLength = Random.Range(1, shelvesPoints.Count);
        StartCoroutine(BrowseShelves());
    }

    void OnTriggerEnter(Collider other)
    {
        if (targetDestination != null && other.gameObject == targetDestination.gameObject)
        {
            reachedDestination = true;
        }
    }

    private IEnumerator BrowseShelves()
    {
        if (browsingLength > 0)
        {
            int shelfIndex = Random.Range(0, shelvesPoints.Count);
            targetDestination = shelvesPoints[shelfIndex];

            while (!reachedDestination)
            {
                if (arrested || isFleeing) { yield break; }
                ToDestination();
                yield return null;
            }

            reachedDestination = false;

            shelvesPoints.RemoveAt(shelfIndex);
            browsingLength--;
            if (Random.Range(0f, 1f) < 0.5f && !stoleItem)
            {
                StartCoroutine(Idle());
            }
            else
            {
                StartCoroutine(Stealing());
            }
        }
        else
        {
            while (!reachedDestination)
            {
                if (arrested || isFleeing) { yield break; }

                targetDestination = spawnpoints[Random.Range(0, spawnpoints.Count - 1)];
                ToDestination();
                yield return null;
            }
        }
    }

    private void GetScared()
    {
        if (!isScared)
        {
            isScared = true;
        }
        navMeshAgent.speed += 2f;
        StartCoroutine(BrowseShelves());
    }

    void Update()
    {
        if (isScared && !isFleeing)
        {
            int playerNearbyInt = Physics.OverlapSphereNonAlloc(transform.position, 6f, playerNearby, LayerMask.GetMask("Player"));

            if (playerNearbyInt > 0)
            {
                playerNearbyTime += Time.deltaTime;
                if (playerNearbyTime > 2f)
                {
                    isFleeing = true;
                    StartCoroutine(Flee());
                }
            }
            else
            {
                playerNearbyTime = 0;
            }
        }

        if (stoleItem)
        {
            trackedTime += Time.deltaTime;
        }
    }

    private IEnumerator Flee()
    {
        Transform targetExit = GetNearestExit();

        if (targetExit != null)
        {
            targetDestination = targetExit;
            navMeshAgent.speed = 10f;
            ToDestination();
            yield return null;
        }
    }

    private Transform GetNearestExit()
    {
        Transform nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Transform exit in spawnpoints)
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

    private IEnumerator Idle() // Planned for animations
    {
        float timeIdle;
        if (isScared)
        {
            timeIdle = 1f;
        }
        else
        {
            timeIdle = Random.Range(2f, 5f);
        }
        yield return new WaitForSeconds(timeIdle);
        StartCoroutine(BrowseShelves());
    }

    private IEnumerator Stealing()
    {
        Debug.Log(string.Format("Scared Shoplifter {0} stole something", gameObject));
        StoleItem = true;
        browsingLength = browsingLength > 2 ? 2 : browsingLength;
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        GetScared();
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