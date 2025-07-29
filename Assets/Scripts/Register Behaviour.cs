//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 27 July 2025
// Description: Register Functions
//===========================================================================================================

using System.Collections.Generic;
using UnityEngine;

public class RegisterBehaviour : MonoBehaviour
{
    public bool available = true;
    private SortedList<GameObject, int> customersInLine = new SortedList<GameObject, int>(new SortGameObjectsByName());

    public SortedList<GameObject, int> CustomersInLine
    {
        get { return customersInLine; }
        set { customersInLine = value; UpdateAvailability(); }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<InnocentNPCBehaviour>() != null)
        {
            bool inLine = customersInLine.ContainsKey(other.gameObject);
            bool targetRegister = other.gameObject.GetComponent<InnocentNPCBehaviour>().TargetDestination;
            if (!inLine && other.gameObject.GetComponent<InnocentNPCBehaviour>().checkingOut && targetRegister == transform)
            {
                CustomersInLine.Add(other.gameObject, customersInLine.Count);
            }
        }
    }

    private void UpdateAvailability()
    {
        if (customersInLine.Count > 0)
        {
            available = false;
        }
        else
        {
            available = true;
        }
    }
    
}
