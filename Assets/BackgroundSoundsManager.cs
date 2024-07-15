using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundsManager : MonoBehaviour
{
    public static BackgroundSoundsManager instance;

    [SerializeField] GameObject currentBackgroundSound;

    [SerializeField] GameObject battleTheme;
    [SerializeField] GameObject finalCutscene;
    [SerializeField] GameObject roofColapse;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject crumble;
    [SerializeField] GameObject defaultBackground;

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

                currentBackgroundSound.SetActive(false);
                defaultBackground.SetActive(true);
                currentBackgroundSound = defaultBackground;

                break;
            case MusicState.battleTheme:

                currentBackgroundSound.SetActive(false);
                battleTheme.SetActive(true);
                currentBackgroundSound = battleTheme;

                break;
            case MusicState.finalCutscene:

                currentBackgroundSound.SetActive(false);
                finalCutscene.SetActive(true);
                currentBackgroundSound = finalCutscene;

                break;
            case MusicState.roofColapse:

                currentBackgroundSound.SetActive(false);
                roofColapse.SetActive(true);
                currentBackgroundSound = roofColapse;

                break;
            case MusicState.pauseMenu:

                currentBackgroundSound.SetActive(false);
                pauseMenu.SetActive(true);
                currentBackgroundSound = pauseMenu;

                break;
            case MusicState.mainMenu:

                currentBackgroundSound.SetActive(false);
                mainMenu.SetActive(true);
                currentBackgroundSound = mainMenu;

                break;
        }
    }
}
