using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物攻击行为
public class AttackTarget : Action
{
    public SharedGameObject target;
    public int attackPower;
    public Rigidbody rb;

    public override void OnAwake()
    {
        attackPower = GetComponent<AIController>().nowAI.AttackPower;
        rb = GetComponent<Rigidbody>();
    }

    public override TaskStatus OnUpdate()
    {
        if (Time.frameCount % 90 == 0)
        {
            transform.LookAt(new Vector3(target.Value.transform.position.x, transform.position.y, target.Value.transform.position.z));
            rb.velocity = new Vector3(0, transform.up.y * 4, 0);
            target.Value.GetComponent<Role>().Attack(AttackType.MonsterAttack, attackPower, transform.position);
            //Debug.Log("英雄生命2  " + target.Value.GetComponent<Role>().HP);
        }
        return TaskStatus.Running;
    }
}
