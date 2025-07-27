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
