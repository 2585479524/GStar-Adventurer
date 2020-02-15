using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//检测怪物是否可追踪
public class CanFollowTarget : Conditional
{
    public SharedGameObject target;
    public float followDistance;

    private bool IsFirst;
    public override void OnAwake()
    {
        IsFirst = true;
        GetComponent<BehaviorTree>().SetVariableValue("player", MapManager.Instance.Player);
        followDistance = GetComponent<AIController>().nowAI.FollowDistance;
    }

    public override TaskStatus OnUpdate()
    {
        if(target.Value == null)
        {
            return TaskStatus.Failure;
        }

        if ((target.Value.transform.position - transform.position).magnitude < followDistance)
        {
            if (IsFirst)
            {
                IsFirst = false;
                MessageManager.Instance.MonsterHaveTarge(transform);
            }
            return TaskStatus.Success;
        }
        IsFirst = true;
        return TaskStatus.Failure;
    }
}
