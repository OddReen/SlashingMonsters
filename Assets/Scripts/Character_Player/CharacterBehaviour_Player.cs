using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour_Player : CharacterBehaviour
{
    [SerializeField] public bool isRootAnimating = false;
    [SerializeField] public bool canInteract = false;

    [Header("GameObject References")]
    [SerializeField] public GameObject menu;
    [SerializeField] public GameObject interactUI;

    [Header("Script References"), HideInInspector]
    public Character_Movement player_Movement;

    public override void Awake()
    {
        base.Awake();
        player_Movement = GetComponent<Character_Movement>();

        CharacterActions[] player = GetComponents<CharacterActions>();
        for (int i = 0; i < player.Length; i++)
        {
            actions.Add(player[i]);
        }
    }
    private void Update()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            actions[i].Action();
        }
    }
    private void LateUpdate()
    {
        animator.transform.position = transform.position;
    }
}
