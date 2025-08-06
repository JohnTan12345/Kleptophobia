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
        //try // Find innocent NPC behaviour component
        {
            InnocentNPCBehaviour innocentNPC = other.gameObject.GetComponent<InnocentNPCBehaviour>();

            if (registerVariables.CustomersInLine.ContainsValue(other.gameObject) && !innocentNPC.checkingOut)
            {
                for (int i = 0; i < registerVariables.CustomersInLine.Count; i++)
                {
                    if (registerVariables.CustomersInLine.ContainsKey(i + 1))
                    {
                        registerVariables.CustomersInLine[i] = registerVariables.CustomersInLine[i + 1];
                    }
                    else
                    {
                        registerVariables.CustomersInLine.Remove(i);
                    }
                }
                registerVariables.CheckCustomerLine();
            }
        }
        //catch {}; // Do nothing if component not found
    }
}
