using System.Collections;
using UnityEngine;

public class DebounceScript : MonoBehaviour
{
    public bool debounce;

    public void Debounce(float time)
    {
        debounce = true;
        StartCoroutine(SetDebounce(time));
    }

    private IEnumerator SetDebounce(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        debounce = false;
    }
}
