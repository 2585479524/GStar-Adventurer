using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    private InventorySlot[] inventorySlots;       //物品槽列表

    private Sprite[] iconSprites;

    private void Awake()
    {
        Instance = this;
        inventorySlots = GetComponentsInChildren<InventorySlot>();
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].slotIndex = i;
        }
        iconSprites = Resources.LoadAll<Sprite>("Sprite/Icon");

        //注册背包事件
        InventoryManager.Instance.InventoryChangeEvent += SetUIByOrder;
    }


    /// <summary>
    ///按物品槽顺序更新UI
    /// </summary>
    public void SetUIByOrder(InventoryData inventoryData)
    {
        int id = inventoryData.itemInfo.ID;
        int amount = inventoryData.amounts;

        int index = GetSameIdSlot(id);
        if (index == -1)
        {
            index = GetEmptySlot();
            if (index == -1)
            {
                Debug.Log("箱子_箱子已满");
                return;
            }
        }
        //更新UI
        inventorySlots[index].SetSlotUI(iconSprites[inventoryData.itemInfo.ID], amount, inventoryData.itemInfo);
    }


    /// <summary>
    /// 通过物品槽索引更新UI
    /// </summary>
    /// <param name="index"></param>
    public void SetUIBySlotIndex(int index, BaseItem item,int amount)
    {
        inventorySlots[index].SetSlotUI(iconSprites[item.ID], amount, item);
    }

    public void RemoveUIByIndex(int index)
    {
        inventorySlots[index].RemoveSlotUI();
    }

    public void RemoveAll()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            RemoveUIByIndex(i);
        }
    }

    /// <summary>
    /// 数量增加
    /// </summary>
    public void AddUIByIndex(int amount,int index)
    {
        inventorySlots[index].AddSlotUI(amount);
    }

    /// <summary>
    /// 空物品槽索引
    /// </summary>
    /// <returns></returns>
    private int GetEmptySlot()
    {
        int index = -1;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].storedItem == null)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    /// <summary>
    /// 存放相同ID物品的物品槽
    /// </summary>
    /// <returns></returns>
    private int GetSameIdSlot(int id)
    {
        int index = -1;
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].storedItem != null&& inventorySlots[i].storedItem.ID== id) 
            {
                index = i;
                break;
            }
        }
        return index;
    }
}
