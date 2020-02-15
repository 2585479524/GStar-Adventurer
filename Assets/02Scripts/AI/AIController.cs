using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 根据ID获取某个怪物的信息  怪物受伤死亡判断
public class AIController : Role
{
    public int ID;
    public AIInfo nowAI;
    public bool isBack = false;
    public bool isBoomBack = false;
    public bool isTrapBack = false;
    public Vector3 attackPosition;
    public float backTimer;
    public bool hasStar;
    public Material skinMaterial;
    public Color skinColor;
    public float hurtTimer = 0.3f;
    public bool isHurt;
    public bool isDeath = false;

    void Awake()
    {
        Init();
    }

    public void Init()
    {
        nowAI = Consts.AIList[ID];
        MaxHp = nowAI.Hp;
        HP = nowAI.Hp;
        if(nowAI.Aiid == 0)
        {
            skinMaterial = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        }
        else if(nowAI.Aiid == 5)
        {
            skinMaterial = transform.GetChild(0).GetChild(0).GetComponent<MeshRenderer>().material;
        }
        else
        {
            skinMaterial = transform.GetChild(0).GetChild(0).GetComponent<SkinnedMeshRenderer>().material;
        }
        skinColor = skinMaterial.color;
        backTimer = 0.8f;
        hurtTimer = 0.3f;
        isHurt = false;
        isDeath = false;
        /*if (hasStar)
        {
            GameObject game= Resources.Load<GameObject>("Star");
            GameObject te = Instantiate(game);
            te.transform.position = transform.position;
            te.transform.SetParent(transform);
        }*/
        if (nowAI.AttackOtherAi)
        {
            GameObject obj = new GameObject("CheckMonster");
            obj.AddComponent<SphereCollider>().isTrigger = true;
            obj.layer = LayerMask.NameToLayer("CheckRole");
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.AddComponent<CheckMonster>().SetFollowRange(nowAI.FollowDistance);
        }
    }

    protected override void Update()
    {
        base.Update();
        if(isBack)
        {
            backTimer -= Time.deltaTime;
            if(backTimer < 0)
            {
                backTimer = 0.8f;
                isBack = false;
                isBoomBack = false;
                isTrapBack = false;
            }
        }
        if(isHurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer < 0)
            {
                hurtTimer = 0.3f;
                skinMaterial.color = skinColor;
            }
        }
        if(isDeath)
        {
            Rigidbody.velocity = new Vector3(0, Rigidbody.velocity.y, 0);
        }
    }

    public override void Attack(AttackType attackType, int Damage, Vector3 attackPos)
    {
        // Debug.Log("怪物ID:" + ID + "受到类型伤害:" + attackType + "，掉血:" + Damage);
        if(HP > 0)
        {
            //普通块击飞
            if (attackType == AttackType.FlyBlock)
            {
                isBack = true;
                attackPosition = attackPos;
                return;
            }
            //爆炸块
            if (attackType == AttackType.BoomBlock)
            {
                isBack = true;
                isBoomBack = true;
                attackPosition = attackPos;
                if (nowAI.IsBurstBlock)
                {
                    HP -= Damage;
                    skinMaterial.color = Color.red;
                    isHurt = true;
                }
                return;
            }
            //重力块
            if (attackType == AttackType.GravityCube && nowAI.IsGravityBlock)
            {
                HP -= Damage;
                isHurt = true;
                skinMaterial.color = Color.red;
                return;
            }
            //伤害块
            if (attackType == AttackType.Trap && nowAI.IsHurtBlock)
            {
                isBack = true;
                isTrapBack = true;
                skinMaterial.color = Color.red;
                isHurt = true;
                HP -= Damage;
                return;
            }
            //掉落伤害
            if (attackType == AttackType.High)
            {
                skinMaterial.color = Color.red;
                isHurt = true;
                HP -= Damage;
                return;
            }
            //怪物攻击伤害
            if (attackType == AttackType.MonsterAttack)
            {
                skinMaterial.color = Color.red;
                isHurt = true;
                HP -= Damage;
                return;
            }
            //掉出地图
            if(attackType == AttackType.OutMap)
            {
                Invoke("Death1", 1.0f);
            }
        }
    }

    public override void Death()
    {
        if (ID == 1)
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }
        else if(ID == 2)
        {
            transform.Rotate(new Vector3(270, 0, 0));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }
        isDeath = true;
        GetComponent<BehaviorTree>().DisableBehavior();
        Invoke("Death1", 2.0f);
    }

    public void Death1()
    {
        if (hasStar)
        {
            GameObject buDing = Instantiate(Resources.Load("Prefabs/PlayMode/30"), LastPos, Quaternion.identity) as GameObject;
            buDing.GetComponent<RotateFragment>().flyToPlayer = true;
        }

        if (MapManager.Instance.FreeMode)
        {
            switch (ID)
            {
                case 0:
                    MessageManager.Instance.CreatMonster(LastPos, 18);
                    break;
                case 1:
                    MessageManager.Instance.CreatMonster(LastPos, 20);
                    break;
                case 2:
                    MessageManager.Instance.CreatMonster(LastPos, 16);
                    break;
                case 3:
                    MessageManager.Instance.CreatMonster(LastPos, 21);
                    break;
                case 4:
                    MessageManager.Instance.CreatMonster(LastPos, 19);
                    break;
                case 5:
                    MessageManager.Instance.CreatMonster(LastPos, 17);
                    break;
                case 6:
                    MessageManager.Instance.CreatMonster(LastPos, 22);
                    break;
            }
        }
        Destroy(gameObject);
    }
}
