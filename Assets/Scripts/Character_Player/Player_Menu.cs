using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Menu : CharacterActions
{
    public override void UpdateAction()
    {
        if (Player_Input.Instance.isMenuing)
        {
            if (!Player_Input.Instance.isInGameMenu) // Go to menu
            {
                characterBehaviour_Player.menu.SetActive(true);
                Time.timeScale = 0f;
                Player_Input.Instance.isInGameMenu = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else // Get out of menu
            {
                characterBehaviour_Player.menu.SetActive(false);
                Time.timeScale = 1f;
                Player_Input.Instance.isInGameMenu = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
