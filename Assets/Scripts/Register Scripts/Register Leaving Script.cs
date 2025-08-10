//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 29 July 2025
// Description: When NPCs leave the register
//===========================================================================================================
using UnityEngine;

public class RegisterLeavingScript : MonoBehaviour
{
    private RegisterVariables registerVariables;

    void Start()
    {
        registerVariables = transform.parent.GetComponent<RegisterVariables>();
    }

    void OnTriggerExit(Collider other)
    {
        InnocentNPCBehaviour innocentNPC = other.gameObject.GetComponent<InnocentNPCBehaviour>();

        if (registerVariables.CustomersInLine.ContainsValue(other.gameObject) && !innocentNPC.checkingOut) // Check if NPC is in line and is leaving
        {
            for (int i = 0; i < registerVariables.CustomersInLine.Count; i++)
            {
                if (registerVariables.CustomersInLine.ContainsKey(i + 1))
                {
                    registerVariables.CustomersInLine[i] = registerVariables.CustomersInLine[i + 1]; // Move all NPCs forward in line
                }
                else
                {
                    registerVariables.CustomersInLine.Remove(i); // Remove the leaving NPC
                }
            }
            registerVariables.CheckCustomerLine();
        }
    }
}
