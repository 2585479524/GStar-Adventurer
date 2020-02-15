using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物追踪目标
public class FollowTarget : Action
{
    public SharedGameObject target;
    public Vector3Int pos = new Vector3Int();
    public Vector3Int targetPos = new Vector3Int();
    public Stack<Vector3Int> path;
    public Vector3Int middlePos;
    public Vector3 middlePos1;
    public float timer;
    public Rigidbody rb;
    public float jumpSpeed;
    public float followSpeed;
    public int jumpHeight;
    public bool attackBlock;
    public bool isStepDeath;
    public Animator Animator;

    public override void OnAwake()
    {
        attackBlock = GetComponent<AIController>().nowAI.AttackBlock;
        followSpeed = GetComponent<AIController>().nowAI.FollowSpeed;
        jumpHeight = GetComponent<AIController>().nowAI.JumpHeight;
        isStepDeath = GetComponent<AIController>().nowAI.IsStepDeath;
        rb = GetComponent<Rigidbody>();
        Animator = transform.GetChild(0).GetComponent<Animator>();
    }

    public override void OnStart()
    {
        timer = 0.0f;
    }

    public override TaskStatus OnUpdate()
    {
        //逃跑和破坏方块判断
        if (followSpeed < 0 || attackBlock)
        {
            Animator.SetFloat("Speed", 2);
            return TaskStatus.Success;
        }

        //变伤害块判断
        if(isStepDeath)
        {
            Vector3 downPos = transform.position - transform.up;
            if (Chunk.GetBlock(downPos) != BlockType.None && Chunk.GetBlock(downPos) != BlockType.Trap)
                Chunk.GetChunk(downPos).ChangeBlock(downPos, BlockType.Trap);
        }

        //寻路
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 1.0f;
            pos.x = (int)(transform.position.x);
            pos.y = (int)(transform.position.y);
            pos.z = (int)(transform.position.z);
            //Debug.Log(pos);
            targetPos.x = (int)(target.Value.transform.position.x);
            targetPos.y = (int)(target.Value.transform.position.y);
            targetPos.z = (int)(target.Value.transform.position.z);
            //Debug.Log(targetPos);
            path = AStar.FindPath(pos, targetPos, jumpHeight);
            if (path == null)
            {
                return TaskStatus.Running;
            }

            else
            {
                middlePos = path.Pop();
                //Debug.Log(middlePos);
                middlePos1.x = middlePos.x + 0.5f;
                middlePos1.y = middlePos.y + 0.5f;
                middlePos1.z = middlePos.z + 0.5f;
            }
        }

        if (path == null)
        {
            Animator.SetFloat("Speed", 0);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            return TaskStatus.Running;
        }

        Animator.SetFloat("Speed", 2);
        //取出下一个中间点
        float dis = Vector3.Distance(transform.position, middlePos1);
        if (dis < 0.5f && path.Count != 0)
        {
            middlePos = path.Pop();
            middlePos1.x = middlePos.x + 0.5f;
            middlePos1.y = middlePos.y + 0.5f;
            middlePos1.z = middlePos.z + 0.5f;
        }

        if ((middlePos1.y - transform.position.y) > 0.1f)
        {
            Animator.SetFloat("Speed", 3);
            //Debug.Log(middlePos1.y);
            Vector3 Dir = middlePos1 - transform.position;
            Dir.y = 0;
            if (Dir.sqrMagnitude > 0.35f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), 5.0f * Time.deltaTime);
              //   transform.forward = Dir.normalized;
            }
            Vector3 ve = transform.forward * followSpeed;
            rb.velocity = new Vector3(ve.x, transform.up.y *  jumpSpeed, ve.z);
        }
        else
        {
            Vector3 Dir = middlePos1 - transform.position;
            Dir.y = 0;
            if (Dir.sqrMagnitude > 0.35f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Dir), 5.0f * Time.deltaTime);
               // transform.forward = Dir.normalized;
            }
            Vector3 ve = transform.forward * followSpeed;
            rb.velocity = new Vector3(ve.x, rb.velocity.y, ve.z);
        }
        return TaskStatus.Running;
    }
}
