using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum AttackType
{
    High,
    GravityCube,
    FlyBlock,
    BoomBlock,
    MonsterAttack,
    Trap,
    OutMap
}
[RequireComponent(typeof(Rigidbody))]
public abstract class Role : MonoBehaviour
{
    //protected Action<AttackType, int> BeAttacked;
    //protected Action CanJump;
    protected bool Ishigh = false;
    public int MaxHp = 3;
    private bool m_CanJump;
    protected Vector3 LastPos;
    protected BlockType DownBlock;
    public bool CanJump
    {
        set
        {
            if (!m_CanJump && value)
            {
                ToGround();
            }
            m_CanJump = value;
        }
        get
        {
            return m_CanJump;
        }
    }
    protected Rigidbody Rigidbody;
    protected int m_hp;
    protected bool IsTrap = false;
    private float TrapTime = 0f;
    public int HP
    {
        set
        {
            if (value > MaxHp)
            {
                m_hp = MaxHp;
            }
            else if (value <= 0)
            {
                //  MessageManager.Instance.RoleDeath(this);
                Death();
            }

            m_hp = value;
            // MessageManager.Instance.HpChange(m_hp);

        }
        get
        {
            return m_hp;
        }
    }
    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        RaycastHit raycastHit;
        bool isRaycast = Physics.Raycast(transform.position, Vector3.down, out raycastHit, 0.6f);

        //Debug.DrawRay(transform.position, Vector3.down,Color.blue,1);
        if (isRaycast)
        {
            CanJump = true;

            if (raycastHit.collider.gameObject.tag == "Chunk")
            {
                LastPos = raycastHit.point;
                LastPos = Character.FromWorldPositionToCubePosition(LastPos + new Vector3(0, 1f, 0));
                BlockType blockType = Chunk.GetBlock(raycastHit.point + new Vector3(0, -0.2f, 0));
                if (blockType == BlockType.Trap)
                {
                    IsTrap = true;

                }
                else
                {
                    IsTrap = false;
                }
            }
            else
            {
                IsTrap = false;
            }
        }
        else
        {
            IsTrap = false;

            if (Rigidbody.velocity.y < -15f)
            {
                Ishigh = true;
            }
            else
            {
                Ishigh = false;
            }
            CanJump = false;
        }

        if (IsTrap)
        {
            TrapTime -= Time.deltaTime;
            if (TrapTime <= 0)
            {
                Vector3 ve = Character.FromWorldPositionToCubePosition(raycastHit.point + new Vector3(0, 0.5f, 0));
                TrapTime = 0.5f;
                Attack(AttackType.Trap, 1, ve);
            }
        }
        else
        {
            TrapTime = 0f;
        }
        if (transform.position.y <= -5)
        {
            Attack(AttackType.OutMap, 1, Vector3.zero);
        }
    }
    public abstract void Attack(AttackType attackType, int Damage, Vector3 attackPos);
    public virtual void ToGround()
    {

    }
    public virtual void Death()
    {
        Destroy(gameObject);
    }
}

