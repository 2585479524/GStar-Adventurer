using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物逃跑
public class Flee : Action
{
    public SharedGameObject target;
    public float followSpeed;
    public Rigidbody rb;
    public float jumpSpeed = 3.0f;

    public override void OnAwake()
    {
        followSpeed = GetComponent<AIController>().nowAI.FollowSpeed;
        rb = GetComponent<Rigidbody>();
    }

    public override TaskStatus OnUpdate()
    {
        if(followSpeed > 0)
        {
            return TaskStatus.Success;
        }

        Vector3 forwardPos = transform.position  + transform.forward;
       
        if (Time.frameCount % 30 == 0)
        {
            if (Chunk.GetBlock(forwardPos) != BlockType.None)
            {
                Vector3 Dir = transform.position - target.Value.transform.position;
                Dir.y = 0;
                if (Dir.sqrMagnitude > 0.3f)
                {
                    transform.forward = Dir.normalized;
                }
                Vector3 ve = transform.forward * -followSpeed;
                rb.velocity = new Vector3(ve.x, transform.up.y * jumpSpeed, ve.z);
            }
            else
            {
                Vector3 Dir = transform.position - target.Value.transform.position;
                Dir.y = 0;
                if (Dir.sqrMagnitude > 0.3f)
                {
                    transform.forward = Dir.normalized;
                }
                Vector3 ve = transform.forward * -followSpeed;
                rb.velocity = new Vector3(ve.x, rb.velocity.y, ve.z);
            }
        }
        return TaskStatus.Running;
    }
}
