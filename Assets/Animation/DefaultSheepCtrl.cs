using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultSheepCtrl : MonoBehaviour
{
    Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;
    }

    public void jump()
    {
        animator.SetTrigger("Jump");
    }

    public void Change()
    {
        animator.speed = 1;
    }
}
