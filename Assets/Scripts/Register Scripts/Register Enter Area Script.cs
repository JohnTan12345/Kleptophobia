using System.Collections.Generic;
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
        //try // Try to get the Innocent NPC Behaviour component
        {
            InnocentNPCBehaviour innocentNPC = other.GetComponent<InnocentNPCBehaviour>();
            if (!registerVariables.CustomersInLine.ContainsValue(other.gameObject) && innocentNPC.checkingOut && innocentNPC.TargetDestination == transform)
            {
                registerVariables.CustomersInLine.Add(registerVariables.CustomersInLine.Count, other.gameObject);
                registerVariables.CheckCustomerLine();
            }
        }
        //catch { }
        ; // Do nothing if no such component
    }
}
