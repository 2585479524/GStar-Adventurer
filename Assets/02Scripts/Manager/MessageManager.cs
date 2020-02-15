using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/// <summary>
/// 全局单例类
///用于消息处理
/// </summary>
/// 
public class MessageManager : Singleton<MessageManager>
{

    #region 委托
    //public EventHandler<bool> DeOrAdd;
    public Action<bool> DeOrAdd;

    public Action WriteScene;

    public Action ReadScene;

    /// <summary>
    /// 吃的倒计时
    /// </summary>
    public Action<float, float> EatTimer;

    //有按钮被按;
    public Action<bool> IsTouchUI;

    //主角死亡
    public Action RoleDeath;

    //吃按钮抬起
    public Action EatButtonUp;

    //添加方块
    public Action<BlockType, int> AddBagItem;

    //删除方块
    public Action<BlockType, int> DeleteBagItem;

    //选中方块
    public Action<BlockType> SelectedItem;

    public Action<NewInventoryData[]> UpdataSlotUI;
    //射击方块
    public Action Shoot;

    public Action<int> HpChange;

    public Action<BlockType, Vector3> InstanceCube;

    //更新选中的Toggle
    public Action<int> UpdateSelectedToggle;

    public Action<int> PlayerChooseLevel;

    //射击冷却时间
    public Action<float> ShootCold;

    // 切换近景远景
    public Action ChangeCamera;

    // 第一次见到怪物
    public Action<int> MeetMonsterHander;

    /// <summary>
    /// 实例粒子的消息
    /// </summary>
    public Action<ParticleType, Vector3,Transform> InstanceParticle;

    public Action EatPudding;

    public Action MainCharShowWin;

    public Action<Transform> MonsterHaveTarge;

    public Action<BlockType> FindBlock;

    public Action<int> TriggerGuidance;

    public Action<Vector3, int> CreatMonster;
    #endregion
}
