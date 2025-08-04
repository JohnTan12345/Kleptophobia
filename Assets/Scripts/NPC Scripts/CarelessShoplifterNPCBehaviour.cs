//===========================================================================================================
// Author: Lucas Tan
// Created: 1 August 2025
// Description: Careless shoplifter NPC behaviour
//===========================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CarelessShoplifterBehaviour : MonoBehaviour
{
    private List<Transform> shelvesPoints;
    private List<Transform> spawnpoints;
    private bool reachedDestination = false;
    private Transform targetDestination;

    [SerializeField] private Transform handSlot; // Where the item will appear in-hand
    [SerializeField] private GameObject itemPrefab; // The visual item they "shoplift"

    public List<Transform> ShelvesPoints
    {
        set { shelvesPoints = new List<Transform>(value); }
    }

    public List<Transform> Spawnpoints
    {
        set { spawnpoints = new List<Transform>(value); }
    }

    void Start()
    {
        StartCoroutine(ShopliftRoutine());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetDestination?.gameObject)
        {
            reachedDestination = true;
        }
    }

    private IEnumerator ShopliftRoutine()
    {
        // Step 1: Go to random shelf
        int shelfIndex = Random.Range(0, shelvesPoints.Count);
        targetDestination = shelvesPoints[shelfIndex];

        while (!reachedDestination)
        {
            ToDestination();
            yield return null;
        }

        reachedDestination = false;

        // Step 2: Wait and pick up item (visually)
        yield return StartCoroutine(Idle());

        if (itemPrefab && handSlot)
        {
            Instantiate(itemPrefab, handSlot.position, handSlot.rotation, handSlot);
        }

        // Step 3: Go to exit but linger near it
        int exitIndex = Random.Range(0, spawnpoints.Count);
        targetDestination = spawnpoints[exitIndex];

        while (!reachedDestination)
        {
            ToDestination();
            yield return null;
        }

        reachedDestination = false;

        // Step 4: Linger/hesitate near exit
        yield return new WaitForSeconds(Random.Range(3f, 6f));

        // Optional: Move slightly or look around here (add animation triggers)

        // Step 5: Leave (or despawn)
        Destroy(gameObject); // Or trigger an exit animation
    }

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(Random.Range(2f, 4f));
    }

    private void ToDestination()
    {
        GetComponent<NavMeshAgent>().SetDestination(targetDestination.position);
    }
}

//handSlot: Create an empty GameObject at the NPC’s hand position and assign it.

// itemPrefab: Any small item prefab like a milk carton, cereal box, etc.

// NavMeshAgent: Make sure the NPC has a NavMeshAgent and collider.

// Spawner Script: Assign shelves and spawnpoints to this NPC like the innocent one.