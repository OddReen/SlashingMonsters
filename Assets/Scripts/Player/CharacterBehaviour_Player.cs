using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour_Player : CharacterBehaviour
{
    [SerializeField] public bool isRootAnimating = false;
    [SerializeField] public bool canInteract = false;

    [Header("GameObject References")]
    [SerializeField] public GameObject menu;
    [SerializeField] public GameObject weaponSlot;
    [SerializeField] public GameObject interactUI;

    [Header("Script References")]
    public HealthSystem healthSystem;
    public Rigidbody rb;
    public Player_Movement player_Movement;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
        player_Movement = GetComponent<Player_Movement>();

        PlayerActions[] player = GetComponents<PlayerActions>();
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
