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
        bool inLine = customersInLine.ContainsKey(other.gameObject);
        if (!inLine)
        {
            CustomersInLine.Add(other.gameObject, customersInLine.Count);
        }
    }

    void OnTriggerExit(Collider other)
    {
        bool inLine = customersInLine.ContainsKey(other.gameObject);
        if (!inLine)
        {
            CustomersInLine.Remove(other.gameObject);

            foreach(GameObject customers in customersInLine.Keys)
            {
                customersInLine[customers]--;
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
