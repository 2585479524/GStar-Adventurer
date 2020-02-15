using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//怪物破坏方块
public class DestroyBlock : Action
{
    public SharedGameObject target;
    public float followSpeed;
    public Rigidbody rb;

    public override void OnAwake()
    {
        followSpeed = GetComponent<AIController>().nowAI.FollowSpeed;
        rb = GetComponent<Rigidbody>();
    }

    public override TaskStatus OnUpdate()
    {
        Vector3 pos = transform.forward * 0.2f;
        RaycastHit raycastHit;
        bool isRaycast = Physics.Raycast(transform.position, transform.forward, out raycastHit, 0.8f);
        if(isRaycast && raycastHit.collider.gameObject.tag == "Chunk" && Chunk.GetBlock(raycastHit.point + pos) != BlockType.Bedrock)
        {
            //Debug.Log("1111");
            Chunk.GetChunk(raycastHit.point + pos).ChangeBlock(raycastHit.point + pos, BlockType.None);
        }
        isRaycast = Physics.Raycast(transform.position + transform.up, transform.forward, out raycastHit, 0.8f);
        if (isRaycast && raycastHit.collider.gameObject.tag == "Chunk" && Chunk.GetBlock(raycastHit.point + pos) != BlockType.Bedrock)
        {
            Chunk.GetChunk(raycastHit.point + pos).ChangeBlock(raycastHit.point + pos, BlockType.None);
        }

        isRaycast = Physics.Raycast(transform.position + transform.right * 0.6f, transform.forward, out raycastHit, 0.8f);
        if (isRaycast && raycastHit.collider.gameObject.tag == "Chunk" && Chunk.GetBlock(raycastHit.point + pos) != BlockType.Bedrock)
        {
            Chunk.GetChunk(raycastHit.point + pos).ChangeBlock(raycastHit.point + pos, BlockType.None);
        }
        isRaycast = Physics.Raycast(transform.position + transform.up + transform.right * 0.6f, transform.forward , out raycastHit, 0.8f);
        if (isRaycast && raycastHit.collider.gameObject.tag == "Chunk" && Chunk.GetBlock(raycastHit.point + pos) != BlockType.Bedrock)
        {
            Chunk.GetChunk(raycastHit.point + pos).ChangeBlock(raycastHit.point + pos, BlockType.None);
        }

        isRaycast = Physics.Raycast(transform.position - transform.right * 0.6f, transform.forward, out raycastHit, 0.8f);
        if (isRaycast && raycastHit.collider.gameObject.tag == "Chunk" && Chunk.GetBlock(raycastHit.point + pos) != BlockType.Bedrock)
        {
            Chunk.GetChunk(raycastHit.point + pos).ChangeBlock(raycastHit.point + pos, BlockType.None);
        }
        isRaycast = Physics.Raycast(transform.position + transform.up - transform.right * 0.6f, transform.forward, out raycastHit, 0.8f);
        if (isRaycast && raycastHit.collider.gameObject.tag == "Chunk" && Chunk.GetBlock(raycastHit.point + pos) != BlockType.Bedrock)
        {
            Chunk.GetChunk(raycastHit.point + pos).ChangeBlock(raycastHit.point + pos, BlockType.None);
        }

        Vector3 Dir = target.Value.transform.position - transform.position;
        Dir.y = 0;
        if (Dir.sqrMagnitude > 0.3f)
        {
            transform.forward = Dir.normalized;
        }
        Vector3 ve = transform.forward * followSpeed;
        rb.velocity = new Vector3(ve.x, rb.velocity.y, ve.z);

        return TaskStatus.Running;
    }

    //public override void OnDrawGizmos()
    //{
    //    base.OnDrawGizmos();
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + transform.forward);
    //    Gizmos.DrawLine(transform.position - transform.right * 0.5f, transform.position - transform.right * 0.5f + transform.forward);
    //    Gizmos.DrawLine(transform.position + transform.right * 0.5f, transform.position + transform.right * 0.5f + transform.forward);
    //    Gizmos.DrawLine(transform.position + transform.up, transform.position + transform.forward + transform.up);
    //    Gizmos.DrawLine(transform.position - transform.right * 0.5f + transform.up, transform.position - transform.right * 0.5f + transform.forward + transform.up);
    //    Gizmos.DrawLine(transform.position + transform.right * 0.5f + transform.up, transform.position + transform.right * 0.5f + transform.forward + transform.up);
    //}
}

