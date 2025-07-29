using System.Collections.Generic;
using UnityEngine;

public class RegisterVariables : MonoBehaviour
{
    public bool available = true;

    private SortedList<int, GameObject> customersInLine = new SortedList<int, GameObject>();

    public SortedList<int, GameObject> CustomersInLine
    {
        get { return customersInLine; }
        set { customersInLine = value; }
    }

    public void CheckCustomerLine()
    {
        string debugText = transform.parent + " ";

        if (customersInLine.Count == 0)
        {
            available = true;
        }
        else
        {
            available = false;
        }
        
            for (int i = 0; i < customersInLine.Count; i++)
            {
                debugText += "\nIndex:" + i + ", NPC: " + customersInLine[i] + " ";
            }

                Debug.Log(debugText);
    }
}
