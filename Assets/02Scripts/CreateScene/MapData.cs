using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData
{
    // 长 宽 高
    private int x;
    private int y;
    private int z;

    // 预制体id
    public string prefabId;
    // position
    public double positionX;
    public double positionY;
    public double positionZ;
    // rotate
    public double rotateX;
    public double rotateY;
    public double rotateZ;
    // scale
    public double scaleX;
    public double scaleY;
    public double scaleZ;

    public ObjectData()
    {
    }

    public ObjectData(string prefabId, double positionX, double positionY, double positionZ, double rotateX, double rotateY, double rotateZ, double scaleX, double scaleY, double scaleZ)
    {
        this.prefabId = prefabId;
        this.positionX = positionX;
        this.positionY = positionY;
        this.positionZ = positionZ;
        this.rotateX = rotateX;
        this.rotateY = rotateY;
        this.rotateZ = rotateZ;
        this.scaleX = scaleX;
        this.scaleY = scaleY;
        this.scaleZ = scaleZ;
    }
}
public class MonsterData
{
    // 长 宽 高
    private int x;
    private int y;
    private int z;

    // MonsterID
    public string prefabId;
    // position
    public double positionX;
    public double positionY;
    public double positionZ;
    // rotate
    public double rotateX;
    public double rotateY;
    public double rotateZ;
    // scale
    public double scaleX;
    public double scaleY;
    public double scaleZ;

    public bool IsLighting;
    public MonsterData()
    {

    }

    public MonsterData(string prefabId, double positionX, double positionY, double positionZ, double rotateX, double rotateY, double rotateZ, double scaleX, double scaleY, double scaleZ, bool IsLighting)
    {
        this.prefabId = prefabId;
        this.positionX = positionX;
        this.positionY = positionY;
        this.positionZ = positionZ;
        this.rotateX = rotateX;
        this.rotateY = rotateY;
        this.rotateZ = rotateZ;
        this.scaleX = scaleX;
        this.scaleY = scaleY;
        this.scaleZ = scaleZ;
        this.IsLighting = IsLighting;
    }
}
public class TriggerData
{
    // TriggerID
    public string prefabId;
    public int triggerID;
    // position
    public double positionX;
    public double positionY;
    public double positionZ;
    // scale
    public double scaleX;
    public double scaleY;
    public double scaleZ;

    public TriggerData()
    {

    }

    public TriggerData(string prefabId, double positionX, double positionY, double positionZ, double scaleX, double scaleY, double scaleZ, int ID)
    {
        this.prefabId = prefabId;
        this.positionX = positionX;
        this.positionY = positionY;
        this.positionZ = positionZ;
        this.scaleX = scaleX;
        this.scaleY = scaleY;
        this.scaleZ = scaleZ;
        triggerID = ID;
    }
}