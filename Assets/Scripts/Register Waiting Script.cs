using System.Collections.Generic;
using UnityEngine;

public class RegisterWaitingScript : MonoBehaviour
{
    private RegisterBehaviour register;
    private bool isDestination = false;
    private SortedList<GameObject, int> customersInLine;
    void Awake()
    {
        register = transform.parent.GetComponent<RegisterBehaviour>();
        customersInLine = register.CustomersInLine;

        if (gameObject.name == "Destination")
        {
            isDestination = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        bool inLine = customersInLine.ContainsKey(other.gameObject);
        if (inLine && customersInLine[other.gameObject] == 0)
        {
            for (int i = 0; i < customersInLine.Keys.Count; i++)
            {
                GameObject customers = customersInLine.Keys[i];
                customersInLine[customers]--;
            }

            customersInLine.Remove(other.gameObject);
        }
    }
}
