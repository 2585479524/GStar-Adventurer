using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInventoryData
{
    public int amount;
    public BlockType blockType;

    public NewInventoryData(int amount, BlockType blockType)
    {
        this.amount = amount;
        this.blockType = blockType;
    }
}
