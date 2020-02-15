using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxManager : SingletonBas<BoxManager>
{
    private int boxSelectedItemID;
    private List<InventoryData> boxDatas = new List<InventoryData>();

    //箱子变化事件
    public delegate void BoxChange(InventoryData data);
    public event BoxChange BoxChangeEvent;

    /// <summary>
    /// 箱子是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return boxDatas.Count == 0 ? true : false;
    }

    /// <summary>
    /// 存储物品根据种类
    /// </summary>
    /// <param name="storeItem"></param>
    public void StoreItemByType(BaseItem toStoreItem,int count,bool updateUI = true) 
    {
        for(int i = 0; i < boxDatas.Count; i++)
        {
            if (boxDatas[i].itemInfo.ID == toStoreItem.ID)
            {
                boxDatas[i].amounts += count;

                //更新UI
                if (updateUI)
                {
                    BoxChangeEvent(boxDatas[i]);
                }
                return;
            }
        }

        InventoryData inventoryData = new InventoryData(toStoreItem, count);
        boxDatas.Add(inventoryData);
        //更新UI
        if (updateUI)
        {
            BoxChangeEvent(inventoryData);
        }
    }


    /// <summary>
    /// 删除物品，根据物品ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public void RemoveItemById(int id, int count, bool updateUI=true)
    {
        for(int i =0; i < boxDatas.Count; i++)
        {
            if (boxDatas[i].itemInfo.ID == id)
            {
                boxDatas[i].amounts = boxDatas[i].amounts - count;

                //更新UI
                if (updateUI)
                {
                    BoxChangeEvent(boxDatas[i]);
                }
                
                if (boxDatas[i].amounts <= 0)
                {
                    boxDatas.RemoveAt(i);
                }
                break;
            }
        }
    }

    /// <summary>
    /// 更新选择物体
    /// </summary>
    /// <param name="index"></param>
    public void SelectItem(int id)
    {
        boxSelectedItemID = id;
    }

    public int GetSelectItemID()
    {
        return boxSelectedItemID;
    }
}
