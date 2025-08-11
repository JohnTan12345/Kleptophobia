//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 25 July 2025
// Description: To update common variables regardless of script
//===========================================================================================================
using System.Collections.Generic;
using UnityEngine;

public interface NPCBehaviour
{
    bool Arrested { get; set; }
    bool StoleItem { get; set; }
    int Points { get; }
    List<Transform> ShelvesPoints { set; }
    List<Transform> Spawnpoints { set; }
    List<GameObject> Items { set; }
    NPCSpawner NPCSpawner { set; }
    Transform ItemSlot { set; }
}
