using System.Collections.Generic;

public static class Consts
{
    public const float EatTime = 1f;

    public static List<BaseCube> cubeList = new List<BaseCube>();
    public static List<AIInfo> AIList = new List<AIInfo>();

    public const int LevelNum = 8;
    public const int GuidanceNum = 10;

    public const int CubeNum = 15;
    public const int MonsterNum=7;
}
/// <summary>
/// 存储关卡进度
/// </summary>
public class LevelSchedule
{
    public bool[] Pass;
    public bool[] ShowGuidance;
    /*public LevelSchedule()
    {
        Pass = new bool[Consts.LevelNum];
        ShowGuidance = new bool[Consts.GuidanceNum];
    }*/
}
/// <summary>
/// 全局的一些数据
/// 图鉴是否已有
/// </summary>
public class CubeAndMonsterData
{
    public bool[] CubeIsFirst;
    public bool[] MonsterIsFirst;
}
public class LevelMes
{
    public string Name;
    public int diff;
    public int EatPudding;
    public bool IsPass;

    public LevelMes()
    {
    }

    public LevelMes(string name,int dif,int eat,bool isPass)
    {
        Name = name;
        diff = dif;
        EatPudding = eat;
        IsPass = isPass;
    }
}
