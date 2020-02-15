using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///每类物品对应数据
/// </summary>
public class InventoryData
{
    public BaseItem itemInfo;
    public int amounts;

    public InventoryData(BaseItem itemInfo, int amounts) 
    {
        this.itemInfo = itemInfo;
        this.amounts = amounts;
    }
}

