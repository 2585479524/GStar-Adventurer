using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物巡逻行为
public class AIPatrol : Action
{
    public SharedGameObject target;
    public int patrolSpeed;
    public int patrolDistance;
    public float timer;
    public Vector3 randomDirection;
    public Rigidbody rb;
    public bool attackOtherAI;
    public float t;
    public Animator Animator;

    public override void OnAwake()
    {
        GetComponent<BehaviorTree>().SetVariableValue("player", MapManager.Instance.Player);
        Animator = transform.GetChild(0).GetComponent<Animator>();
        patrolSpeed = GetComponent<AIController>().nowAI.PatrolSpeed;
        patrolDistance = GetComponent<AIController>().nowAI.PatrolDistance;
        attackOtherAI = GetComponent<AIController>().nowAI.AttackOtherAi;
        rb = GetComponent<Rigidbody>();
    }

    public override void OnStart()
    {
        timer = 2f;
        randomDirection = new Vector3(0f, Random.Range(-359, 359), 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(randomDirection), 0.8f * Time.deltaTime);
       // transform.Rotate(randomDirection);
    }

    public override TaskStatus OnUpdate()
    {
        if(target.Value == null && attackOtherAI)
        {
            target.Value = GameObject.Find("Player");
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(randomDirection), 0.8f * Time.deltaTime);

        //每过一段时间，随机转向
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            timer = 2;
            randomDirection = new Vector3(0f, Random.Range(-359, 359), 0f);
            //transform.Rotate(randomDirection);
        }

        Vector3 forwordPos = transform.position + transform.forward * 1.2f;
        Vector3 downPos = transform.position - transform.up + transform.forward;
        //若前方的下方无方块，或前方有方块，停止
        if (Chunk.GetBlock(downPos) == BlockType.None || Chunk.GetBlock(forwordPos) != BlockType.None)
        {
            //Debug.Log("停");
            Animator.SetFloat("Speed", 0);
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        else 
        {
            if (patrolSpeed == 0)
            {
                Animator.SetFloat("Speed", 0);
            }
            else
            {
                Animator.SetFloat("Speed", 1);
            }
            rb.velocity = transform.forward * patrolSpeed;
        }

        return TaskStatus.Running;
    }
}
