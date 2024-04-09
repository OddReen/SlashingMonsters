using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Test : MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform _transform;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        animator = GetComponent<Animator>();
        _transform.localScale = Vector3.one * 2;
    }
}
