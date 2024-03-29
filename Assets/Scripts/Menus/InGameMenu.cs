using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] Button b_resume;
    [SerializeField] Button b_mainMenu;
    [SerializeField] Button b_options;
    [SerializeField] Button b_quit;
    void Start()
    {
        b_resume.onClick.AddListener(Resume);
        b_mainMenu.onClick.AddListener(MainMenu);
        b_options.onClick.AddListener(Options);
        b_quit.onClick.AddListener(Quit);
    }
    private void Resume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        Player_Input.Instance.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    private void Options()
    {

    }
    private void Quit()
    {
        Application.Quit();
    }
}
