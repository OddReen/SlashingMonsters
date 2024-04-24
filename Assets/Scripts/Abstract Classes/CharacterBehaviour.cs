using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{
    public enum AttackingState
    {
        None,
        Preparing,
        Attacking,
        Ending
    }
    public AttackingState atackingState = AttackingState.None;

    [Header("References")]
    public Animator animator;
    [HideInInspector] public HealthSystem healthSystem;
    [HideInInspector] public Rigidbody rb;

    [Header("Variables")]
    public float weaponDamageAmount;
    public float kickDamageAmount;
    public float stunTimeAmount;
    public float parryWindow;

    [Header("Stats")]
    public bool isStunned = false;
    public bool isDead = false;
    public bool isParrying = false;

    [Header("Actions")]
    [SerializeField] public List<CharacterActions> actions;

    public virtual void Awake()
    {
        atackingState = AttackingState.None;
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
    }
}