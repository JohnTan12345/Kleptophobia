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
    private List<Transform> exitPoints;
    private Transform targetExit;

    private bool isScared = false;
    private bool isFleeing = false;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Coroutine wanderRoutine;

    public List<Transform> ExitPoints
    {
        set { exitPoints = new List<Transform>(value); }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        wanderRoutine = StartCoroutine(Wander());
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


}
