using System.Collections;
using UnityEngine;

public class TitleScreenUIScript : MonoBehaviour

{
    private Animator animator;
    private Transform optionBackground;

    void Start()
    {
        animator = GetComponent<Animator>();
        optionBackground = transform.Find("Background");
        StartCoroutine(TitleScreen());
    }

    private IEnumerator TitleScreen()
    {

        // Group name intro
        yield return new WaitForSeconds(2);
        // Pan Down

        // Move Stuff
        animator.SetTrigger("Start");
        


        Debug.Log("done");
    }
}
