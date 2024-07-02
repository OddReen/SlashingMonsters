using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{
    public enum AtackingState
    {
        None,
        Preparing,
        Attacking,
        Ending
    }
    public AtackingState atackingState = AtackingState.None;

    [Header("References")]
    public Animator animator;
    [HideInInspector] public HealthSystem healthSystem;
    [HideInInspector] public Rigidbody rb;

    [Header("Variables")]
    public float weaponDamageAmount;
    public float kickDamageAmount;
    public float parryStunAmount;
    public float parryWindow;

    [Header("Stats")]
    public bool isStunned = false;
    public bool isDead = false;
    public bool isParrying = false;

    [Header("Actions")]
    [SerializeField] public List<CharacterActions> actions;

    public virtual void Awake()
    {
        atackingState = AtackingState.None;
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
    }
}