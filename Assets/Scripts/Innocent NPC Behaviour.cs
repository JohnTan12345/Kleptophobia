using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InnocentNPCBehaviour : MonoBehaviour
{
    public List<Transform> shelvesPoints;
    public List<Transform> registerPoints;
    public List<Transform> spawnpoints;

    private bool reachedDestination = false;
    private int browsinglength;
    private Transform targetDestination;

    void Start()
    {
        browsinglength = Mathf.RoundToInt(Random.Range(1, shelvesPoints.Count - 1));
        StartCoroutine(ShopActivities());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetDestination.gameObject)
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
            Debug.Log(browsinglength);
            StartCoroutine(Idle());
        }
        else // Checkout
        {
            Debug.Log("Checking out");
            while (!reachedDestination)
            {
                targetDestination = registerPoints[Mathf.RoundToInt(Random.Range(0, registerPoints.Count - 1))];
                ToDestination();
                yield return null;
            }

            Debug.Log("checked Out");
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

    private IEnumerator Idle()
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        StartCoroutine(ShopActivities());
    }

    private void ToDestination()
    {
        GetComponent<NavMeshAgent>().SetDestination(targetDestination.position);
    }
}
