using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//怪物信息
public class AIInfo
{
    private int AIID;  //怪物ID
    private string AIName;

    private string imgUrl;    //图片

    private int patrolDistance;  //巡逻范围
    private int patrolSpeed; //巡逻速度
    private int followDistance;  //追踪范围
    private float followSpeed;   //追踪速度
    private float attackDistance;    //攻击范围
    private int jumpHeight;  //跳跃高度
    private int attackPower;  //攻击力
    private int HP;  //血量
    private bool isStepDeath;     //走过的方块是否变伤害块
    private bool isHurtBlock;    //是否受伤害块伤害
    private bool isBurstBlock;   //是否受爆炸块伤害
    private bool isGravityBlock;     //是否受重力块活埋
    private bool attackOtherAI;  //是否攻击其他怪
    private bool attackBlock;    //是否破坏方块

    private int fallDeathHeight;  //摔死高度
    
    private string Describe;   //描述

    private string attack;
    private string health;
    private string speed;

    private bool isFirst = false; // 是否第一次遇见，默认false

    public int Aiid
    {
        get => AIID;
        set => AIID = value;
    }

    public string AiName
    {
        get => AIName;
        set => AIName = value;
    }

    public string ImgUrl
    {
        get => imgUrl;
        set => imgUrl = value;
    }

    public int PatrolDistance
    {
        get => patrolDistance;
        set => patrolDistance = value;
    }

    public int PatrolSpeed
    {
        get => patrolSpeed;
        set => patrolSpeed = value;
    }

    public int FollowDistance
    {
        get => followDistance;
        set => followDistance = value;
    }

    public float FollowSpeed
    {
        get => followSpeed;
        set => followSpeed = value;
    }

    public float AttackDistance
    {
        get => attackDistance;
        set => attackDistance = value;
    }

    public int JumpHeight
    {
        get => jumpHeight;
        set => jumpHeight = value;
    }

    public int AttackPower
    {
        get => attackPower;
        set => attackPower = value;
    }

    public int Hp
    {
        get => HP;
        set => HP = value;
    }

    public bool IsStepDeath
    {
        get => isStepDeath;
        set => isStepDeath = value;
    }

    public bool IsHurtBlock
    {
        get => isHurtBlock;
        set => isHurtBlock = value;
    }

    public bool IsBurstBlock
    {
        get => isBurstBlock;
        set => isBurstBlock = value;
    }

    public bool IsGravityBlock
    {
        get => isGravityBlock;
        set => isGravityBlock = value;
    }

    public bool AttackOtherAi
    {
        get => attackOtherAI;
        set => attackOtherAI = value;
    }

    public bool AttackBlock
    {
        get => attackBlock;
        set => attackBlock = value;
    }

    public int FallDeathHeight
    {
        get => fallDeathHeight;
        set => fallDeathHeight = value;
    }

    public string Describe1
    {
        get => Describe;
        set => Describe = value;
    }
    public string Attack {
        get => attack;
        set => attack = value;
    }

    public string Health {
        get => health;
        set => health = value;
    }

    public string Speed {
        get => speed;
        set => speed = value;
    }

    public bool IsFirst
    {
        get => isFirst;
        set => isFirst = value;
    }

    public AIInfo(int aiid, string aiName, string imgUrl, int patrolDistance, int patrolSpeed, int followDistance, float followSpeed, float attackDistance, int jumpHeight, int attackPower, int hp, bool isStepDeath, bool isHurtBlock, bool isBurstBlock, bool isGravityBlock, bool attackOtherAi, bool attackBlock, int fallDeathHeight, string describe, string attack, string health, string speed)
    {
        AIID = aiid;
        AIName = aiName;
        this.imgUrl = imgUrl;
        this.patrolDistance = patrolDistance;
        this.patrolSpeed = patrolSpeed;
        this.followDistance = followDistance;
        this.followSpeed = followSpeed;
        this.attackDistance = attackDistance;
        this.jumpHeight = jumpHeight;
        this.attackPower = attackPower;
        HP = hp;
        this.isStepDeath = isStepDeath;
        this.isHurtBlock = isHurtBlock;
        this.isBurstBlock = isBurstBlock;
        this.isGravityBlock = isGravityBlock;
        attackOtherAI = attackOtherAi;
        this.attackBlock = attackBlock;
        this.fallDeathHeight = fallDeathHeight;
        Describe = describe;
        this.attack = attack;
        this.health = health;
        this.speed = speed;
    }
}
