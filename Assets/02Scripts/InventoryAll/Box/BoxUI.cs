using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
    public static BoxUI Instance;

    private List<BoxSlot> boxSlots;

    //物品icon
    private Sprite[] iconSprites;

    private void Awake()
    {
        Instance = this;
        iconSprites = Resources.LoadAll<Sprite>("Sprite/Icon");

        boxSlots = new List<BoxSlot>();
        boxSlots.AddRange(GetComponentsInChildren<BoxSlot>());
        for(int i = 0; i < boxSlots.Count; i++)
        {
            boxSlots[i].slotIndex = i;
        }
        //注册箱子物品改变事件
        BoxManager.Instance.BoxChangeEvent += SetUIByOreder;
    }

    /// <summary>
    ///按物品槽顺序更新UI
    /// </summary>
    public void SetUIByOreder(InventoryData inventoryData)
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
        boxSlots[index].SetSlotUI(iconSprites[inventoryData.itemInfo.ID], amount, inventoryData.itemInfo);
    }

    /// <summary>
    /// 通过物品槽索引更新UI
    /// </summary>
    /// <param name="index"></param>
    public void SetUIBySlotIndex(int index, BaseItem item, int amount)
    {
        boxSlots[index].SetSlotUI(iconSprites[item.ID], amount, item);
    }

    public void RemoveUIByIndex(int index)
    {
        boxSlots[index].RemoveSlotUI();
    }

    /// <summary>
    /// 数量增加
    /// </summary>
    public void AddUIByIndex(int amount, int index)
    {
        boxSlots[index].AddSlotUI(amount);
    }

    /// <summary>
    /// 空物品槽索引
    /// </summary>
    /// <returns></returns>
    private int GetEmptySlot()
    {
        int index = -1;
        for (int i = 0; i < boxSlots.Count; i++)
        {
            if (boxSlots[i].storedItem == null) 
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
        for (int i = 0; i < boxSlots.Count; i++)
        {
            if (boxSlots[i].storedItem!=null&& boxSlots[i].storedItem.ID == id) 
            {
                index = i;
                break;
            }
        }
        return index;
    }
}
