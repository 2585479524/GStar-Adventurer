using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonBas<InventoryManager>
{
    //背包物品变化事件
    public delegate void InventoryChange(InventoryData data);
    public event InventoryChange InventoryChangeEvent;

    //背包容量
    public int inventoryCapacity = 8;

    //背包数据
    private List<InventoryData> inventoryDatas = new List<InventoryData>();

    //当前选择物品ID
    private int selectedItemID;

    //以存物品种类数
    private int allAmount = 0;

    /// <summary>
    /// 添加物品,根据种类
    /// </summary>
    /// <param name="inventoryType"></param>
    public void AddItemByType(BaseItem addBlock, int count, bool updateUI = true) 
    {
        for (int i = 0; i < inventoryDatas.Count; i++)
        {
            if (inventoryDatas[i].itemInfo.ID == addBlock.ID)
            {
                inventoryDatas[i].amounts += count;

                //更新UI
                if (updateUI)
                {
                    InventoryChangeEvent(inventoryDatas[i]);
                }
                return;
            }
        }

        if (allAmount >= inventoryCapacity)
        {
            return;
        }
        InventoryData inventoryData = new InventoryData(addBlock, count);
        inventoryDatas.Add(inventoryData);
        //更新UI
        if (updateUI)
        {
            InventoryChangeEvent(inventoryData);
        }
    }

    /// <summary>
    /// 删除物品,根据ID
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItemByID(int id,int count,bool updateUI=true)
    {
        int index = -1;
        for(int i = 0; i < inventoryDatas.Count; i++)
        {
            if (inventoryDatas[i].itemInfo.ID == id) 
            {
                index = i;
                break;
            }
        }
        if (index == -1)
        {
            Debug.Log("背包_删除物品不存在");
            return;
        }

        inventoryDatas[index].amounts -= count;
        //更新UI
        if (updateUI)
        {
            InventoryChangeEvent(inventoryDatas[index]);
        }

        if (inventoryDatas[index].amounts <= 0)
        {
            inventoryDatas.RemoveAt(index);
            allAmount--;
        }
    }

    /// <summary>
    /// 更新选择物体
    /// </summary>
    /// <param name="index"></param>
    public void SelectItem(int id)
    {
        selectedItemID = id;
    }

    public int GetSelectItemID()
    {
        return selectedItemID;
    }

    public void AllToBox()
    {
        for (int i = 0; i < inventoryDatas.Count; i++)
        {
            BaseItem addItem = inventoryDatas[i].itemInfo;
            int addAmount = inventoryDatas[i].amounts;
            BoxManager.Instance.StoreItemByType(addItem, addAmount);
        }

        inventoryDatas.Clear();
        InventoryUI.Instance.RemoveAll();
    }
}
