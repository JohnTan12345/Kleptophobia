//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 29 July 2025
// Description: Register Variables
//===========================================================================================================
using UnityEngine;

public class RegisterEnterAreaScript : MonoBehaviour
{
    private RegisterVariables registerVariables;

    void Start()
    {
        registerVariables = transform.parent.GetComponent<RegisterVariables>();
    }

    void OnTriggerEnter(Collider other)
    {

        InnocentNPCBehaviour innocentNPC = other.GetComponent<InnocentNPCBehaviour>(); // Check if the NPC is innocent
        if (!registerVariables.CustomersInLine.ContainsValue(other.gameObject) && innocentNPC.checkingOut && innocentNPC.TargetDestination == transform)
        {
            registerVariables.CustomersInLine.Add(registerVariables.CustomersInLine.Count, other.gameObject);
            registerVariables.CheckCustomerLine();
        }
    }
}
