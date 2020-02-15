using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LitJson;


public class EditMap : Singleton<MapManager>
{

    public GameObject Prefab;
    public Chunk chunkPrefab;

    List<ObjectData> dataList = new List<ObjectData>();

    private GameObject env;

    private ObjectData mapData;
    private void Start()
    {
        env = gameObject;
        // UpdateWorld();
        //读入场景

        Prefab = Resources.Load("Prefabs/Cube") as GameObject;
        //加入按钮监听
         MessageManager.Instance.InstanceCube += InstanceACube;

        WriteEnv(chunkPrefab);
        WriteEnvObject();
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
                GameObject GCube = Instantiate(Prefab);
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

    public void ReadEnv()
    {
        //JsonMapper.ToJson();
        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                string Data = "";
                Data = JsonMapper.ToJson(Chunk.AllChunks[i, j].map);
                string path = Saver.WritePath + "/MapData_" + i + "" + j + ".json";
                Saver.WriteJsonString(Data, path);
            }
        }
        Debug.Log("已保存场景");
    }
    /// <summary>
    /// 根据json生成场景
    /// </summary>
    public void WriteEnv(Chunk chunkPrefab)
    {

        for (int i = 0; i < Chunk.ChunkNum; i++)
        {
            for (int j = 0; j < Chunk.ChunkNum; j++)
            {
                string path = Saver.WritePath + "/MapData_" + i + "" + j + ".json";
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

    }


    /// <summary>
    /// 读取env下的相关场景信息，并保存到MapData.json文件中
    /// </summary>
    public void ReadEnvObject()
    {
        foreach (Transform tr in env.transform)
        {
            mapData = new ObjectData(tr.name.Substring(0, 7), tr.position.x, tr.position.y, tr.position.z,
                tr.eulerAngles.x, tr.eulerAngles.y, tr.eulerAngles.z,
                tr.localScale.x, tr.localScale.y, tr.localScale.z);
            dataList.Add(mapData);
        }
        LevelData setData = new LevelData();
        setData.ObjectData = dataList;
        //JsonMapper.ToJson();
        string Data = JsonMapper.ToJson(setData);

        Saver.WriteJsonString(Data, Saver.WritePath + "/MapObjectData.json");
    }

    /// <summary>
    /// 根据json生成场景
    /// </summary>
    public void WriteEnvObject()
    {
        // json --> string
        string fileStream = Saver.ReadJsonString(Saver.WritePath + "/MapObjectData.json");
        // string --> SetData
        LevelData fileData = JsonMapper.ToObject<LevelData>(fileStream);

        foreach (var data in fileData.ObjectData)
        {
            string path = "Prefabs/" + data.prefabId;
            GameObject hp_bar = Resources.Load(path) as GameObject;
            GameObject g = Instantiate(hp_bar);
            g.transform.position = new Vector3((float)data.positionX, (float)data.positionY, (float)data.positionZ);
            g.transform.eulerAngles = new Vector3((float)data.rotateX, (float)data.rotateY, (float)data.rotateZ);
            g.transform.localScale = new Vector3((float)data.scaleX, (float)data.scaleY, (float)data.scaleZ);
            g.transform.parent = env.transform;
        }
    }

}

