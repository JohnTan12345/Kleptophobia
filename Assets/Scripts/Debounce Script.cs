//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 25 July 2025
// Description: Stops NPCs from instantly despawning for now
//===========================================================================================================

using System.Collections;
using UnityEngine;

public class DebounceScript : MonoBehaviour
{
    public bool debounce = true;

    public void Debounce(float time)
    {
        StartCoroutine(SetDebounce(time));
    }

    private IEnumerator SetDebounce(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        debounce = false;
    }
}
