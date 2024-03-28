using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    [SerializeField] Animator animator;

    public static Fade Instance;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("FadeIn", true);
        Instance = this;
    }
    public void FadeIn()
    {
        animator.SetBool("FadeIn", true);
    }
    public void FadeOut()
    {
        animator.SetBool("FadeIn", false);
    }
}
