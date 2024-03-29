using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    public Animator animator;
    [SerializeField] public List<PlayerActions> actions;
    public float damageAmount;
}