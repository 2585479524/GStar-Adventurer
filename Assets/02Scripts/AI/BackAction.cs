using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物被击退动作
public class BackAction : Action
{
    public Rigidbody rb;
    public Vector3 attackPos;

    public override void OnAwake()
    {
        rb = GetComponent<Rigidbody>();
        attackPos = GetComponent<AIController>().attackPosition;
    }

    public override void OnStart()
    {
        Vector3 Dir = attackPos - transform.position;

        //是否为爆炸击退
        if (GetComponent<AIController>().isBoomBack)
        {
            transform.forward = Dir.normalized;
            Vector3 ve = transform.forward * -3;
            ve.y = 10;
            rb.velocity = ve;
        }
        else if(GetComponent<AIController>().isTrapBack)
        {
            Vector3 ve = new Vector3(rb.velocity.x, 5, rb.velocity.z);
            rb.velocity = ve;
        }
        else
        {
            transform.forward = Dir.normalized;
            Vector3 ve = transform.forward * -2;
            ve.y = 8;
            rb.velocity = ve;
        }
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
