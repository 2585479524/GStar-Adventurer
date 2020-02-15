using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测怪物是否可攻击
public class CanAttackTarget : Conditional
{

    public SharedGameObject target;
    public float attackDistance;
    public Animator Animator;

    public override void OnAwake()
    {
        GetComponent<BehaviorTree>().SetVariableValue("player", MapManager.Instance.Player);
        Animator = transform.GetChild(0).GetComponent<Animator>();
        attackDistance = GetComponent<AIController>().nowAI.AttackDistance;
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value == null)
        {
            return TaskStatus.Failure;
        }

        float dis = Vector3.Distance(target.Value.transform.position, transform.position);
        if (dis < attackDistance)
        {
            Animator.SetFloat("Speed", 0);
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
