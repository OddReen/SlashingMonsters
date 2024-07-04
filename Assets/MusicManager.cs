using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] GameObject battleTheme;
    [SerializeField] GameObject finalCutscene;
    [SerializeField] GameObject roofColapse;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject crumble;

    private void Awake()
    {
        instance = this;
    }
    public enum MusicState
    {
        none,
        mainMenu,
        pauseMenu,
        battleTheme,
        finalCutscene,
        roofColapse,
    }

    public void TriggerCrumble()
    {
        crumble.SetActive(true);
    }
    public void TriggerMusic(MusicState state)
    {
        switch (state)
        {
            case MusicState.none:
                battleTheme.SetActive(false);
                finalCutscene.SetActive(false);
                roofColapse.SetActive(false);
                pauseMenu.SetActive(false);
                mainMenu.SetActive(false);
                break;
            case MusicState.battleTheme:
                battleTheme.SetActive(true);
                finalCutscene.SetActive(false);
                roofColapse.SetActive(false);
                pauseMenu.SetActive(false);
                mainMenu.SetActive(false);
                break;
            case MusicState.finalCutscene:
                battleTheme.SetActive(false);
                finalCutscene.SetActive(true);
                roofColapse.SetActive(false);
                pauseMenu.SetActive(false);
                mainMenu.SetActive(false);
                break;
            case MusicState.roofColapse:
                battleTheme.SetActive(false);
                finalCutscene.SetActive(false);
                roofColapse.SetActive(true);
                pauseMenu.SetActive(false);
                mainMenu.SetActive(false);
                break;
            case MusicState.pauseMenu:
                battleTheme.SetActive(false);
                finalCutscene.SetActive(false);
                roofColapse.SetActive(false);
                pauseMenu.SetActive(true);
                mainMenu.SetActive(false);
                break;
            case MusicState.mainMenu:
                battleTheme.SetActive(false);
                finalCutscene.SetActive(false);
                roofColapse.SetActive(false);
                pauseMenu.SetActive(false);
                mainMenu.SetActive(true);
                break;
        }
    }
}
