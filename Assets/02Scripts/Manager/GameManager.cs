using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using LitJson;
using System.Text.RegularExpressions;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
public class GameManager : Singleton<GameManager>
{
    private bool readFinishCube = false;
    private bool readFinishAilist = false;
    public bool readFinsh;
    private bool IsFirst = true;
    public int LoadLevel = 1;
    public List<LevelMes> LevelMess;
    public CubeAndMonsterData ObjectData;
    //全局访问功能
    [HideInInspector] public ObjectPool ObjectPool = null; //对象池
    [HideInInspector] public Sound Sound = null;           //声音控制
    //全局方法
    public void LoadScene(int level)
    {
        //发布事件
        //SendEvent(ConstName.E_ExitScene, level);
        ExitScene(level);
        string SceneName = "";
        switch (level)
        {
            case 1:
                SceneName = "01Start";
                break;
            case 2:
                SceneName = "02Player";
                break;
            default:
                break;
        }
        //---同步加载新场景
        SceneManager.LoadScene(level, LoadSceneMode.Single);
        SceneManager.sceneLoaded += LoadedEve;
    }
    void LoadedEve(Scene s, LoadSceneMode l)
    {
        if (l == LoadSceneMode.Single)
        {
            SceneManager.sceneLoaded -= LoadedEve;
            //事件参数
            int SceneIndex = s.buildIndex;

            //发布事件
            //SendEvent(ConstName.E_EnterScene, SceneIndex);
            EnterScene(SceneIndex);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        LevelMess = new List<LevelMes>();
        ObjectData = new CubeAndMonsterData();
        ObjectData.MonsterIsFirst = new bool[Consts.MonsterNum];
        ObjectData.CubeIsFirst = new bool[Consts.CubeNum];
        //InitLevelData();
        IsFirst = true;
        //确保Game对象一直存在
        DontDestroyOnLoad(gameObject);

        //全局单例赋值
        ObjectPool = ObjectPool.Instance;
        Sound = Sound.Instance;

        InitData();
        //启动游戏
        //MessageManager.Instance.PlayerChooseLevel += CurrentLevel;
        PlayerPrefs.SetInt("CurrentLevel", -1);
        if (PlayerPrefs.GetInt("CurrentLevel", -1) == -1)
        {
            Debug.Log("第一次游戏");
            PlayerPrefs.SetInt("CurrentLevel", 1);
            StartCoroutine(LoadData());
        }
        else
        {
            InitObjectData();
        }

    }

    private void Start()
    {
        SoundManager.Instance.PlayBackgroundMusic("bg");
    }
    private void Update()
    {

    }
    private void ExitScene(int data)
    {
        int index = (int)data;
        switch (index)
        {
            case 0:
                break;
            case 1:
                ///读出的数据导入到Consts
                // ReadUserData();

                break;
            case 2:
                break;
            case 3:
                break;
        }
    }
    private void EnterScene(int data)
    {
        int index = (int)data;
        switch (index)
        {
            case 0:
                break;
            case 1:
                GameObject.Find("Canvas").GetComponent<StartUI>().Init(IsFirst);
                if (IsFirst)
                {
                    IsFirst = false;
                }
                break;
            case 2:
                //读入数据
                ReadUserData();
                break;
            case 3:
                ReadUserData();
                break;
        }
    }
    /// <summary>
    /// 读优先级1
    /// 复制文件资源第一次进入游戏使用
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadData()
    {
        //创建文件夹
        for (int i = 1; i <= Consts.LevelNum; i++)
        {
            Directory.CreateDirectory(Saver.WritePath + "/Level" + i);
        }
        /*string path = Application.streamingAssetsPath + "/Save/MapData_00.json";
        WWW www = new WWW(path);*/
        //把地图数据加载出来
        for (int t = 1; t <= Consts.LevelNum; t++)
        {
            string path1 = Saver.LevelPath + t + "/Leve1Data" + t;
            WWW www = new WWW(path1);
            yield return www;
            if (www.isDone)
            {
                //拷贝数据库到指定路径
                string path = Application.persistentDataPath + "/Level" + t + "/Leve1Data" + t;
                File.WriteAllBytes(path, www.bytes);
            }
        }
        for (int t = 1; t <= Consts.LevelNum; t++)
        {
            string path1 = Saver.LevelPath + t + "/MapObjectData.json";
            WWW www1 = new WWW(path1);
            yield return www1;
            if (www1.isDone)
            {
                //拷贝数据库到指定路径
                string path3 = Application.persistentDataPath + "/Level" + t + "/MapObjectData.json";
                File.WriteAllBytes(path3, www1.bytes);
            }
        }


        string path2 = Saver.SteamPath + "/GobalData.json";
        WWW www2 = new WWW(path2);
        yield return www2;
        if (www2.isDone)
        {
            //拷贝数据库到指定路径
            string path3 = Application.persistentDataPath + "/GobalData.json";
            File.WriteAllBytes(path3, www2.bytes);
        }
        InitObjectData();
    }
    public void LevelEndToSave(bool Scuccess, int PuddingNum)
    {
        if (Scuccess)
        {
            LevelMess[LoadLevel - 1].IsPass = true;
            LevelMess[LoadLevel - 1].EatPudding = 3;
        }
        else
        {
            if (LevelMess[LoadLevel - 1].EatPudding < PuddingNum)
            {
                LevelMess[LoadLevel - 1].EatPudding = PuddingNum;
            }
        }
        WriteUserData();
        GameGobalData gameGobalData = new GameGobalData();
        gameGobalData.LevelMess = LevelMess;
        gameGobalData.CubeFirst = ObjectData.CubeIsFirst;
        gameGobalData.MonsterFirst = ObjectData.MonsterIsFirst;

        string Data = JsonMapper.ToJson(gameGobalData);
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        var ss = reg.Replace(Data, delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        Saver.WriteJsonString(ss, Saver.WritePath + "/GobalData.json");
    }
    /// <summary>
    /// 从配置表读取方块和怪物的信息
    /// </summary>
    public void InitData()
    {
        CSVHelper.Instance().ReadCSVFile("CubeConfig", (table) =>
        {

            // 可以遍历整张表
            foreach (CSVLine line in table)
            {
                Consts.cubeList.Add(new BaseCube(Int32.Parse(line["cubeId"]), line["cubeName"], line["imgUrl"], line["describe"], line["tag"]));
            }
            readFinishCube = true;
            readFinsh = readFinishCube && readFinishAilist;
        });

        CSVHelper.Instance().ReadCSVFile("monsterConfig", (table) =>
        {

            // 可以遍历整张表
            foreach (CSVLine line in table)
            {
                Consts.AIList.Add(new AIInfo(Int32.Parse(line["aiid"]), line["aiName"], line["imgUrl"],
                    Int32.Parse(line["patrolDistance"]), Int32.Parse(line["patrolSpeed"]),
                    Int32.Parse(line["followDistance"]),
                    float.Parse(line["followSpeed"]), float.Parse(line["attackDistance"]),
                    Int32.Parse(line["jumpHeight"]), Int32.Parse(line["attackPower"]), Int32.Parse(line["hp"]),
                    bool.Parse(line["isStepDeath"]), bool.Parse(line["isHurtBlock"]),
                    bool.Parse(line["isBurstBlock"]), bool.Parse(line["isGravityBlock"]),
                    bool.Parse(line["attackOtherAi"]), bool.Parse(line["attackBlock"]),
                    Int32.Parse(line["fallDeathHeight"]), line["describe"], line["attack"], line["health"], line["speed"]));
            }
            readFinishAilist = true;
            readFinsh = readFinishCube && readFinishAilist;
        });
    }

    public void InitObjectData()
    {
        string Data = Saver.ReadJsonString(Saver.WritePath + "/GobalData.json");
        GameGobalData gameGobalData = JsonMapper.ToObject<GameGobalData>(Data);
        ///图鉴数据 读入
        ObjectData.CubeIsFirst = gameGobalData.CubeFirst;
        ObjectData.MonsterIsFirst = gameGobalData.MonsterFirst;
        //关卡数据读入
        LevelMess = gameGobalData.LevelMess;
        LoadScene(1);
    }
    /// <summary>
    /// 将保存的json数据，读入Const.List
    /// </summary>
    public void ReadUserData()
    {
        for (int i = 0; i < ObjectData.CubeIsFirst.Length; i++)
        {
            Consts.cubeList[i].isFirst = ObjectData.CubeIsFirst[i];
        }
        for (int i = 0; i < ObjectData.MonsterIsFirst.Length; i++)
        {
            Consts.AIList[i].IsFirst = ObjectData.MonsterIsFirst[i];
        }
    }
    /// <summary>
    /// 将保存的json数据，读入Const.List
    /// </summary>
    public void WriteUserData()
    {
        for (int i = 0; i < ObjectData.CubeIsFirst.Length; i++)
        {
            ObjectData.CubeIsFirst[i] = Consts.cubeList[i].isFirst;
        }
        for (int i = 0; i < ObjectData.MonsterIsFirst.Length; i++)
        {
            ObjectData.MonsterIsFirst[i] = Consts.AIList[i].IsFirst;
        }
    }
}
/// <summary>
/// 全局数据
/// </summary>
public class GameGobalData
{
    public List<LevelMes> LevelMess;
    public bool[] CubeFirst;
    public bool[] MonsterFirst;
}

