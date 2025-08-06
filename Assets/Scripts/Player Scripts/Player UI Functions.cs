//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 4 August 2025
// Description: UI for stamina and others
//===========================================================================================================
using System.Collections;
using StarterAssets;
using UnityEngine;

public class PlayerUIFunctions : MonoBehaviour
{
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } set { currentStamina = value; StaminaUpdate(); } }
    public float maxStamina = 100;
    public float staminaUsageRate = 1;

    private Transform staminaBar;
    private FirstPersonController firstPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    void Start()
    {
        starterAssetsInputs = transform.parent.parent.Find("PlayerCapsule").GetComponent<StarterAssetsInputs>();
        firstPersonController = transform.parent.parent.Find("PlayerCapsule").GetComponent<FirstPersonController>();
        staminaBar = transform.Find("Stamina Bg").Find("Stamina Bar Guideline").Find("Stamina Bar");
        currentStamina = maxStamina;
        StartCoroutine(ReduceStamina());
    }
    private void StaminaUpdate()
    {
        staminaBar.localScale = new Vector3(currentStamina / maxStamina, 1f, 1f);
    }

    private IEnumerator ReduceStamina()
    {
        while (true)
        {

            while (starterAssetsInputs.sprint)
            {

                while (starterAssetsInputs.move != Vector2.zero)
                {
                    if (CurrentStamina > 0)
                    {
                        CurrentStamina -= staminaUsageRate * Time.deltaTime;
                    }
                    else
                    {
                        firstPersonController._hasStamina = false;
                    }

                    if (!starterAssetsInputs.sprint)
                    {
                        break;
                    }
                    yield return null;
                }

                yield return null;
            }

            while (!starterAssetsInputs.sprint)
            {
                if (CurrentStamina < maxStamina)
                {
                    CurrentStamina += staminaUsageRate * Time.deltaTime;
                    firstPersonController._hasStamina = currentStamina > 0;
                }
                else
                {
                    break;
                }
                yield return null;
            }

            yield return null;
        }
    }
}
