using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenUIScript : MonoBehaviour

{
    private Animator animator;
    private Button playButton;
    private Button settingsButton;
    private Button creditsButton;
    private Button backButton;
    private Button newGameButton;

    void Start()
    {
        animator = GetComponent<Animator>();
        playButton = transform.Find("Play Button").GetComponent<Button>();
        playButton.onClick.AddListener(OnPlayButtonPressed);
        settingsButton = transform.Find("Settings Button").GetComponent<Button>();
        creditsButton = transform.Find("Credits Button").GetComponent<Button>();
        backButton = transform.Find("Back Button").GetComponent<Button>();
        backButton.onClick.AddListener(OnBackButtonPressed);
        newGameButton = transform.Find("New Game Button").GetComponent<Button>();


        StartCoroutine(TitleScreen());
    }

    private IEnumerator TitleScreen()
    {

        // Group name intro
        yield return new WaitForSeconds(2);
        // Pan Down

        // Move Stuff
        animator.SetTrigger("Start");
    }

    private void OnPlayButtonPressed()
    {
        animator.SetTrigger("Play");
    }

    private void OnBackButtonPressed()
    {
        animator.SetTrigger("Back");
    }

    // When Background is in frame

    public void BackgroundInFrame()
    {
        animator.SetBool("Bg In Frame", true);
    }

}
