using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;
using UnityEngine.UI;

public class LevelData
{
    /// <summary>
    /// 主角数据
    /// </summary>
    public double positionX;
    public double positionY;
    public double positionZ;
    // rotate
    public double rotateX;
    public double rotateY;
    public double rotateZ;

    public List<ObjectData> ObjectData;

    public List<MonsterData> monsterDatas;

    public List<TriggerData> triggerDatas;

    public LevelData()
    {
        ObjectData = new List<ObjectData>();
        monsterDatas = new List<MonsterData>();
        triggerDatas = new List<TriggerData>();
    }
}
public class MapManager : Singleton<MapManager>
{
    public GameObject Player;

    private GameObject CubePrefab;
    public Chunk chunkPrefab;

    //List<ObjectData> dataList = new List<ObjectData>();

    private GameObject env;

    private int Level = 1;

    private ObjectData mapData;
    public bool IsEditMode = false;
    public bool FreeMode = false;

    private void Start()
    {
        //从游戏管理器里拿到加载的关卡
        if (!IsEditMode)
        {
            Level = GameManager.Instance.LoadLevel;
        }
        env = gameObject;
        if (FreeMode)
        {
            UpdateWorld();
        }
        else
        {
            WriteEnv(chunkPrefab);
            WriteEnvObject();
        }
        //读入场景
        CubePrefab = Resources.Load("Prefabs/Cube") as GameObject;
        //加入按钮监听
        if (!IsEditMode)
        {
            MessageManager.Instance.ReadScene += ReadEnv;
            MessageManager.Instance.ReadScene += ReadEnvObject;
        }
        MessageManager.Instance.InstanceCube += InstanceACube;
    }



    private void InstanceACube(BlockType obj, Vector3 Pos)
    {
        Debug.Log("实例" + obj);
        switch (obj)
        {
            case BlockType.None:
                break;
            case BlockType.DragonFruit:
                break;
            case BlockType.ExplotiveCube:
                break;
            case BlockType.GravityCube:
                GameObject GCube;
                if (IsEditMode)
                {
                    GCube = Instantiate(CubePrefab);
                }
                else
                {
                    GCube =ObjectPool.Instance.Spawn("Cube");
                }
                GCube.GetComponent<CubeInstance>().InstanceGravity(obj, Pos);
                break;
            case BlockType.Cake:
                break;
            case BlockType.LoveCube:
                break;
            case BlockType.Object:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 创建81个Chunk
    /// </summary>
    public void UpdateWorld()
    {
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                Vector3 pos = new Vector3(i * Chunk.width, 0, j * Chunk.width);
                Chunk chunk = Instantiate(chunkPrefab, pos, Quaternion.identity);
                Chunk.AllChunks[i, j] = chunk;
            }
        }
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                Chunk.AllChunks[i, j].InitMap();
            }
        }
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                Chunk.AllChunks[i, j].BuildChunk();
            }
        }
    }

    /*public void ReadEnv()
    {
        List<BlockType[,]> blockTypes=new List<BlockType[,]>();
        //JsonMapper.ToJson();
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                string Data = "";
                Data = JsonMapper.ToJson(Chunk.AllChunks[i, j].map);
                string path = Saver.LevelPath + Level + "/MapData_" + i + "" + j + ".json";
                Saver.WriteJsonString(Data, path);
            }
        }
        Debug.Log("已保存场景");
    }*/

    public void ReadEnv()
    {
        List<int> blockTypes = new List<int>();
        //JsonMapper.ToJson();
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {

                BlockType[,,] ss = Chunk.AllChunks[i, j].map;
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 30; y++)
                    {
                        for (int z = 0; z < 10; z++)
                        {

                            blockTypes.Add((int)ss[x, y, z]);                 
                        }
                    }
                }
            }
        }
        string Data = "";
        Data = JsonMapper.ToJson(blockTypes); ;
        string path = Saver.LevelPath + Level + "/Leve1Data" + Level;
        Saver.WriteJsonString(Data, path);
        Debug.Log("已保存场景");
    }
    /// <summary>
    /// 根据json生成场景
    /// </summary>
    /*public void WriteEnv(Chunk chunkPrefab)
    {
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                string path = Saver.LevelPath + Level + "/MapData_" + i + "" + j + ".json";
                // json --> string
                string fileStream = Saver.ReadJsonString(path);
                // Debug.Log(fileStream);
                // string --> SceneData.BlockType[,]
                int[] fileData = JsonMapper.ToObject<int[]>(fileStream);
                Vector3 pos = new Vector3(i * Chunk.width, 0, j * Chunk.width);
                Chunk chunk = Instantiate(chunkPrefab, pos, Quaternion.identity);
                Chunk.AllChunks[i, j] = chunk;
                Chunk.AllChunks[i, j].InitMap(fileData);
            }
        }
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                Chunk.AllChunks[i, j].BuildChunk();
            }
        }


        //foreach (var data in fileData)
        //{
        //    string path = "Prefabs/" + data.prefabId;
        //    hp_bar = Resources.Load(path) as GameObject;
        //    GameObject g = Instantiate(hp_bar);
        //}

    }*/
    /// <summary>
    /// 根据json生成场景
    /// </summary>
    public void WriteEnv(Chunk chunkPrefab)
    {
        List<int> blockTypes = new List<int>();

        string path = Saver.WritePath + "/Level" + Level + "/Leve1Data" + Level;


        string fileStream = Saver.ReadJsonString(path);


        blockTypes = JsonMapper.ToObject<List<int>>(fileStream);
        int co = 0;
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                Vector3 pos = new Vector3(i * Chunk.width, 0, j * Chunk.width);
                Chunk chunk = Instantiate(chunkPrefab, pos, Quaternion.identity);
                Chunk.AllChunks[i, j] = chunk;
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 30; y++)
                    {
                        for (int z = 0; z < 10; z++)
                        {
                            chunk.map[x, y, z] = (BlockType)blockTypes[co];
                            co++;
                        }
                    }
                }
                // Chunk.AllChunks[i, j].InitMap(fileData);
            }
        }
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                Chunk.AllChunks[i, j].BuildChunk();
            }
        }
    }

    /// <summary>
    /// 读取env下的相关场景信息，并保存到MapData.json文件中
    /// </summary>
    public void ReadEnvObject()
    {
        LevelData setData = new LevelData();
        foreach (Transform tr in env.transform)
        {
            string Name = tr.name.Substring(0, 2);
            int ID = 0;
            if (int.TryParse(Name, out ID))
            {
                if (ID >= 16 && ID <= 22)
                {
                    bool islight = tr.GetComponent<EditMonster>().IsLighting;
                    setData.monsterDatas.Add(new MonsterData(Name, tr.position.x, tr.position.y, tr.position.z,
                tr.eulerAngles.x, tr.eulerAngles.y, tr.eulerAngles.z,
                tr.localScale.x, tr.localScale.y, tr.localScale.z, islight));
                }
                else if ((ID <= 28 && ID >= 23) || ID == 30)
                {
                    setData.ObjectData.Add(new ObjectData(Name, tr.position.x, tr.position.y, tr.position.z,
                tr.eulerAngles.x, tr.eulerAngles.y, tr.eulerAngles.z,
                tr.localScale.x, tr.localScale.y, tr.localScale.z));
                }
                else if (ID == 29)
                {
                    int id = tr.GetComponent<EditTrigger>().TriggerID;
                    setData.triggerDatas.Add(new TriggerData(Name, tr.position.x, tr.position.y, tr.position.z,
                    tr.localScale.x, tr.localScale.y, tr.localScale.z, id));
                }
            }
        }

        setData.positionX = Player.transform.position.x;
        setData.positionY = Player.transform.position.y;
        setData.positionZ = Player.transform.position.z;

        setData.rotateX = Player.transform.eulerAngles.x;
        setData.rotateY = Player.transform.eulerAngles.y;
        setData.rotateZ = Player.transform.eulerAngles.z;

        //JsonMapper.ToJson();
        string Data = JsonMapper.ToJson(setData);

        Saver.WriteJsonString(Data, Saver.LevelPath + Level + "/MapObjectData.json");
    }

    /// <summary>
    /// 根据json生成场景
    /// </summary>
    public void WriteEnvObject()
    {
        // json --> string
        string fileStream = Saver.ReadJsonString(Saver.WritePath + "/Level" + Level + "/MapObjectData.json");
        // string --> SetData
        LevelData fileData = JsonMapper.ToObject<LevelData>(fileStream);

        //主角的数据
        Player.transform.position = new Vector3((float)fileData.positionX, (float)fileData.positionY, (float)fileData.positionZ);
        Player.transform.eulerAngles = new Vector3((float)fileData.rotateX, (float)fileData.rotateY, (float)fileData.rotateZ);


        string path;
        if (IsEditMode)
        {
            path = "Prefabs/EditMode/";
        }
        else
        {
            path = "Prefabs/PlayMode/";
        }
        foreach (var data in fileData.ObjectData)
        {
            int id = int.Parse(data.prefabId.Substring(0, 2));
            GameObject pre = Resources.Load(path + data.prefabId.Substring(0, 2)) as GameObject;
            GameObject g = Instantiate(pre);
            g.transform.position = new Vector3((float)data.positionX, (float)data.positionY, (float)data.positionZ);
            g.transform.eulerAngles = new Vector3((float)data.rotateX, (float)data.rotateY, (float)data.rotateZ);
            g.transform.localScale = new Vector3((float)data.scaleX, (float)data.scaleY, (float)data.scaleZ);
            g.transform.parent = env.transform;
        }
        foreach (var data in fileData.monsterDatas)
        {
            GameObject pre = Resources.Load(path + data.prefabId.Substring(0, 2)) as GameObject;
            GameObject g = Instantiate(pre);
            g.transform.position = new Vector3((float)data.positionX, (float)data.positionY, (float)data.positionZ);
            g.transform.eulerAngles = new Vector3((float)data.rotateX, (float)data.rotateY, (float)data.rotateZ);
            g.transform.localScale = new Vector3((float)data.scaleX, (float)data.scaleY, (float)data.scaleZ);
            g.transform.parent = env.transform;
            if (IsEditMode)
            {
                g.GetComponent<EditMonster>().IsLighting = data.IsLighting;
            }
            else
            {
                g.GetComponent<AIController>().hasStar = data.IsLighting;
            }

        }
        foreach (var data in fileData.triggerDatas)
        {
            GameObject pre = Resources.Load(path + data.prefabId.Substring(0, 2)) as GameObject;
            GameObject g = Instantiate(pre);
            g.transform.position = new Vector3((float)data.positionX, (float)data.positionY, (float)data.positionZ);
            g.transform.localScale = new Vector3((float)data.scaleX, (float)data.scaleY, (float)data.scaleZ);
            g.transform.parent = env.transform;
            g.GetComponent<EditTrigger>().TriggerID = data.triggerID;
        }
    }
    public void CleanScene()
    {
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                Destroy(Chunk.AllChunks[i, j].gameObject);
            }
        }
        List<Transform> lst = new List<Transform>();
        foreach (Transform child in transform)
        {
            lst.Add(child);
            Debug.Log("删除:" + child.gameObject.name);
        }
        for (int i = 0; i < lst.Count; i++)
        {
            Destroy(lst[i].gameObject);
        }
    }
    public void LoadLevel(int level)
    {
        CleanScene();
        Level = level;
        WriteEnv(chunkPrefab);
        WriteEnvObject();
    }
    public void SaveLevel()
    {
        ReadEnv();
        ReadEnvObject();
    }
}

