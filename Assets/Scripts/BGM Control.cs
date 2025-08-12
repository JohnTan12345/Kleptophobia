using UnityEngine;

public class BGMControl : MonoBehaviour
{
    public AudioSource BGM;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BGM.volume = 0;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BGM.volume = 1;
        }
    }
}
