using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player_Menu : PlayerActions
{
    public override void Action()
    {
        if (Player_Input.Instance.isMenuing)
        {
            if (characterBehaviour_Player.menu.activeSelf)
            {
                characterBehaviour_Player.menu.SetActive(false);
                Time.timeScale = 1f;
                Player_Input.Instance.enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                characterBehaviour_Player.menu.SetActive(true);
                Time.timeScale = 0f;
                Player_Input.Instance.enabled = false;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }
}
