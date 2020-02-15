using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem 
{
    public int ID;
    public int hasGravity;
    public int durability;

    public BaseItem(int id,int hasGravity,int durability)
    {
        ID = id;
        this.hasGravity = hasGravity;
        this.durability = durability;
    }
}
