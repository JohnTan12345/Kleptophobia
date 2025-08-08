//===========================================================================================================
// Author: Tan Hong Yan John
// Created: 6 August 2025
// Description: Title Screen Functions
//===========================================================================================================
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenUIScript : MonoBehaviour

{
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        transform.Find("Play Button").GetComponent<Button>().onClick.AddListener(OnPlayButtonPressed);
        transform.Find("Credits Button").GetComponent<Button>().onClick.AddListener(OnCreditsButtonPressed);
        transform.Find("Back Button").GetComponent<Button>().onClick.AddListener(OnBackButtonPressed);
        transform.Find("Back Button (1)").GetComponent<Button>().onClick.AddListener(OnBackButtonPressed);
        transform.Find("New Game Button").GetComponent<Button>().onClick.AddListener(OnNewGamePressed);
    }

    public void TitleScreen()
    {
        transform.Find("Dimmer").gameObject.SetActive(false);
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

    private void OnCreditsButtonPressed()
    {
        animator.SetTrigger("Credits");
    }

    private void OnNewGamePressed()
    {
        animator.SetTrigger("New Game");
    }

    // Events

    public void BackgroundInFrame()
    {
        animator.SetBool("Bg In Frame", true);
    }

    public void SwitchScenes()
    {
        SceneManager.LoadScene(1);
    }

}
