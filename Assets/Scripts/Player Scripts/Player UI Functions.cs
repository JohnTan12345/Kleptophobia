//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 4 August 2025
// Description: UI for stamina and others
//===========================================================================================================
using System.Collections;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIFunctions : MonoBehaviour
{
    private float currentStamina;
    public float CurrentStamina { get { return currentStamina; } set { currentStamina = value; StaminaUpdate(); } }
    public float maxStamina = 100;
    public float staminaUsageRate = 1;

    private Transform staminaBar;
    private FirstPersonController firstPersonController;
    private StarterAssetsInputs starterAssetsInputs;
    private Transform endCredits;
    private TextMeshProUGUI totalArrestedText;
    private TextMeshProUGUI shopliftersArrestedText;
    private TextMeshProUGUI innocentsArrestedText;
    private TextMeshProUGUI escapedText;
    private TextMeshProUGUI totalPointsText;
    private Button returnButton;

    void Start()
    {
        endCredits = transform.Find("End Credits Bg");
        totalArrestedText = endCredits.Find("Total Arrested").GetComponent<TextMeshProUGUI>();
        shopliftersArrestedText = endCredits.Find("Shoplifters").GetComponent<TextMeshProUGUI>();
        innocentsArrestedText = endCredits.Find("Innocents").GetComponent<TextMeshProUGUI>();
        escapedText = endCredits.Find("Escaped").GetComponent<TextMeshProUGUI>();
        totalPointsText = endCredits.Find("Total Points").GetComponent<TextMeshProUGUI>();
        returnButton = endCredits.Find("Button").GetComponent<Button>();
        returnButton.onClick.AddListener(ReturnToTitle);
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

    private void ReturnToTitle()
    {
        PointsScript.ResetPoints();
        SceneManager.LoadScene(0);
    }

    public void OnGameEnd()
    {
        totalArrestedText.text = string.Format("{0}", PointsScript.innocents + PointsScript.shoplifters);
        shopliftersArrestedText.text = string.Format("{0}", PointsScript.shoplifters);
        innocentsArrestedText.text = string.Format("{0}", PointsScript.innocents);
        escapedText.text = string.Format("{0}", PointsScript.escaped);
        totalPointsText.text = string.Format("{0}", PointsScript.points);
        endCredits.gameObject.SetActive(true);
    }
}
