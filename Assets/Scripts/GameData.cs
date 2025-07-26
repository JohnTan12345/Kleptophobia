using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    static GameObject GMInstance;

    // global data
    public List<GameObject> NPClist = new List<GameObject>();

    void Awake()
    {
        if (GMInstance != null && GMInstance != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            GMInstance = gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }
}
