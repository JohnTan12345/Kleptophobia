//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 25 July 2025
// Description: Points System
//===========================================================================================================
using UnityEngine;

public class PointsScript : MonoBehaviour
{
    public static int points = 0;
    public static int innocents = 0;
    public static int shoplifters = 0;
    public static int escaped = 0;

    public static void ModifyPoints(bool StoleItem, int value, int time = 0)
    {
        int pointChange;
        if (!StoleItem || value < 0)
        {
            pointChange = -2;
            innocents++;
        }
        else
        {
            pointChange = value;
            shoplifters++;
        }

        if (pointChange >= 0)
        {
            switch (time)
            {
                case < 10:
                    pointChange += 2;
                    break;
                case < 20:
                    pointChange += 1;
                    break;
            }
        }

        points += pointChange;
        Debug.Log("Points: " + points + ", Time: " + time);
    }
}
