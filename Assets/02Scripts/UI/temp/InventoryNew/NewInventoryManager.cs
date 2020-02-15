using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NewInventoryManager :Singleton<NewInventoryManager>
{
    public Transform tip;

    private const int inventoryCapacity = 6;      //背包格子数量
    private const int slotCapacity = 999;           //背包格子容量

    private int index=-1;
    private int Index
    {
        set
        {
            if (value!=index)
            {
                if (value == -1)
                {
                     MessageManager.Instance.SelectedItem(BlockType.None);
                    index = -1;
                }
                else
                {
                    index = value;
                    selectBlock = datasArray[index];
                     MessageManager.Instance.SelectedItem(selectBlock.blockType);
                }
            }
        }
        get
        {
            return index;
        }
    }

    private int usedSlots = 0;                  //已经使用格子数量
    private NewInventoryData selectBlock = null;
    private NewInventoryData[] datasArray = new NewInventoryData[inventoryCapacity];

    protected override void Awake()
    {
        base.Awake();
        //初始化datasArray
        for (int i = 0; i < inventoryCapacity; i++)
        {
            datasArray[i] = new NewInventoryData(0, BlockType.None);
        }
        selectBlock = datasArray[0];

        //注册事件
         MessageManager.Instance.AddBagItem += AddBlock;
         MessageManager.Instance.DeleteBagItem += DeleteBlock;

    }


    /// <summary>
    /// 存放方块
    /// </summary>
    /// <param name="addBlock"></param>
    /// <param name="amount"></param>
    public void AddBlock(BlockType addBlock, int amount)
    {
        //尝试存到存放相同类型方块的格子
        List<int> sameSlotIndexs = FindSameType(addBlock);

        int leftoverAmout = amount; //未存放完的方块数
        for(int i = 0; i < sameSlotIndexs.Count; i++)
        {
            int canAddAmount = slotCapacity - datasArray[sameSlotIndexs[i]].amount;
            leftoverAmout -= canAddAmount;
            if (leftoverAmout >= 0)
            {
                datasArray[sameSlotIndexs[i]].amount += canAddAmount;
                 MessageManager.Instance.UpdataSlotUI(datasArray);
                if (sameSlotIndexs[i]==index)
                {
                     MessageManager.Instance.SelectedItem(selectBlock.blockType);
                }
                if (leftoverAmout == 0)
                {
                    return;
                }
            }
            else
            {
                datasArray[sameSlotIndexs[i]].amount += (canAddAmount + leftoverAmout);
                 MessageManager.Instance.UpdataSlotUI(datasArray);
                if (sameSlotIndexs[i] == index)
                {
                     MessageManager.Instance.SelectedItem(selectBlock.blockType);
                }
                return;
            }
        }

        if (leftoverAmout > 0)
        {
            //存放到空格子
            List<int> emptySlotIndexs = FindEmptySlot();

            for (int i = 0; i < emptySlotIndexs.Count; i++)
            {
                int canAddAmount = slotCapacity - datasArray[emptySlotIndexs[i]].amount;
                leftoverAmout -= canAddAmount;
                if (leftoverAmout >= 0)
                {
                    datasArray[emptySlotIndexs[i]].amount += canAddAmount;
                    datasArray[emptySlotIndexs[i]].blockType = addBlock;
                     MessageManager.Instance.UpdataSlotUI(datasArray);
                    if (emptySlotIndexs[i] == index)
                    {
                         MessageManager.Instance.SelectedItem(selectBlock.blockType);
                    }                 
                    if (leftoverAmout == 0)
                    {
                        return;
                    }
                }
                else
                {
                    datasArray[emptySlotIndexs[i]].amount += (canAddAmount + leftoverAmout);
                    datasArray[emptySlotIndexs[i]].blockType = addBlock;
                     MessageManager.Instance.UpdataSlotUI(datasArray);
                    if (emptySlotIndexs[i] == index)
                    {
                         MessageManager.Instance.SelectedItem(selectBlock.blockType);
                    }
                    return;
                }
            }
        }

        //存不下
        Debug.Log("未存下数" + leftoverAmout);
        tip.localScale = Vector3.zero;
        tip.DOScale(Vector3.one, 1f).onComplete += () => { tip.DOScale(Vector3.zero, 0.1f); };
    }

    /// <summary>
    /// 删除选中物品
    /// </summary>
    /// <param name="deleteBlock"></param>
    /// <param name="amount"></param>
    public void DeleteBlock(BlockType deleteBlock,int amount)
    {
        selectBlock.amount -= amount;
        if (selectBlock.amount <= 0)
        {
            selectBlock.amount = 0;
            selectBlock.blockType = BlockType.None;
             MessageManager.Instance.UpdateSelectedToggle(FindNotEmptySlot());
        }
         MessageManager.Instance.UpdataSlotUI(datasArray);
    }


    /// <summary>
    /// 查找存放相同类型方块的格子索引
    /// </summary>
    /// <param name="blockType"></param>
    /// <returns></returns>
    private List<int> FindSameType(BlockType blockType)
    {
        List<int> temp = new List<int>();

        for(int i = 0; i < inventoryCapacity; i++)
        {
            if (datasArray[i].blockType == blockType)
            {
                temp.Add(i);
            }
        }
        return temp;
    }


    /// <summary>
    /// 查找空格子
    /// </summary>
    /// <returns></returns>
    private List<int> FindEmptySlot()
    {
        List<int> temp = new List<int>();
        for(int i = 0; i < inventoryCapacity; i++)
        {
            if (datasArray[i].amount == 0)
            {
                temp.Add(i);
            }
        }
        return temp;
    }

    /// <summary>
    /// 查找不为空的格子
    /// </summary>
    /// <returns></returns>
    private int FindNotEmptySlot()
    {
        for (int i = 0; i < inventoryCapacity; i++)
        {
            if (datasArray[i].amount != 0)
            {
                return i;
            }
        }
        return -1;
    }

    public void SetSelectBlock(int index)
    {
        Index = index;
    }
}
