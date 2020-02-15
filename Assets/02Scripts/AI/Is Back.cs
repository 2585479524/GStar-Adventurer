using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测怪物是否被击退
public class IsBack : Conditional
{
    public bool isBack;
    public Animator Animator;

    public override void OnAwake()
    {
        Animator = transform.GetChild(0).GetComponent<Animator>();
    }
    public override TaskStatus OnUpdate()
    {
        isBack = GetComponent<AIController>().isBack;

        if (isBack)
        {
            Animator.SetFloat("Speed", 0);
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
