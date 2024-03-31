using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Actions")]
    [SerializeField] public List<PlayerActions> actions;

    [Header("Variables")]
    public float weaponDamageAmount;
    public float kickDamageAmount;
    public float stunTimeAmount;

    [Header("Stats")]
    public bool isStunned = false;
    public bool isDead = false;
}