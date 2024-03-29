using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button b_startGame;
    [SerializeField] Button b_options;
    [SerializeField] Button b_quit;


    private void Start()
    {
        b_startGame.onClick.AddListener(StartGame);
        b_options.onClick.AddListener(Options);
        b_quit.onClick.AddListener(Quit);
    }
    private void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    private void Options()
    {

    }
    private void Quit()
    {
        Application.Quit();
    }
}
