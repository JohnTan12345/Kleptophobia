//===========================================================================================================
// Author: Foong Mun Yip Xander
// Created: 6 August 2025
// Description: Store Time script
//===========================================================================================================

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StoreTime : MonoBehaviour
{
    public TMP_Text timeText; // UI Text to display the time
    public float realTimeMinuteLength = 60f; // 60 seconds = 1 in-game hour

    private float timer = 0f;
    private int gameHour = 9; // Start at 9 AM
    private int gameMinute = 0;
    private bool dayEnded = false;

    void Update()
    {
        if (dayEnded) return;

        // Advance time based on real time passed
        timer += Time.deltaTime;

        if (timer >= realTimeMinuteLength / 60f) // Advance game minute
        {
            gameMinute++; // Increment game minute
            timer = 0f;

            if (gameMinute >= 60)
            {
                gameMinute = 0;
                gameHour++; // Advance game hour

                if (gameHour >= 17) // 5 PM
                {
                    dayEnded = true;
                    EndDay();
                }
            }

            UpdateTimeUI();
        }
    }

    void UpdateTimeUI()
    {
        if (timeText != null)
        {
            string ampm = gameHour >= 12 ? "PM" : "AM";
            int displayHour = gameHour > 12 ? gameHour - 12 : gameHour;
            if (displayHour == 0) displayHour = 12;

            timeText.text = string.Format("{0:D2}:{1:D2} {2}", displayHour, gameMinute, ampm);
        }
    }

    void EndDay()
    {
        Debug.Log("Day ended");
        
    }
}
