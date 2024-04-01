using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class CharacterBehaviour : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public HealthSystem healthSystem;
    public Rigidbody rb;

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
        healthSystem = GetComponent<HealthSystem>();
        rb = GetComponent<Rigidbody>();
    }
}