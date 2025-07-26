using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnocentNPCBehaviour : MonoBehaviour
{
    public List<Transform> shelvesPoints;
    public List<Transform> registerPoints;
    public List<Transform> spawnpoints;

    void Start()
    {
        StartCoroutine(GoToShelf());
    }

    private IEnumerator GoToShelf()
    {
        bool browsing = true;
        while (browsing)
        {
            int shelfnumber = Mathf.RoundToInt(Random.Range(0, shelvesPoints.Count));
            
            yield return null;
        }
        
    }
}
