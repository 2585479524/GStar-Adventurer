using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimplexNoise;
using Random = UnityEngine.Random;




public enum BlockType
{
    None,
    LoveCube,           //爱心方块
    Trap,               //陷阱块
    ExplotiveCube,      //爆炸块
    Bedrock,            //基岩

    GravityCube,        //重力方块
    DragonFruit,        //火龙果
    Kiwi,               //猕猴桃
    Orange,              //橘子
    MoonCake,           //月饼

    BurningWheat,       //烧麦
    Meat,                //肉
    Cheese,             //奶酪
    Cake,               //蛋糕
    Cake1,              //饼干1
    Cake2,              //饼干2
    Object
}
public enum FaceType
{
    Top,
    Front,
    Back,
    Left,
    Right,
    Bottom,
}
/// <summary>
/// 该类用于实例生成地图
/// </summary>
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{
    //贴图数据
    private const int y_num = 15;
    private const int x_num = 6;
    public const int ChunkNum = 9;
    public static Chunk[,] AllChunks = new Chunk[ChunkNum, ChunkNum];
    //public static List<Chunk> chunks = new List<Chunk>();
    public static int width = 10;
    public static int height = 30;

    public int seed;
    public float baseHeight = 10;
    public float frequency = 0.025f;
    public float amplitude = 1;

    public BlockType[,,] map;
    public bool[,,] IsShowFace;
    Mesh chunkMesh;
    MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    MeshFilter meshFilter;

    Vector3 offset0;
    Vector3 offset1;
    Vector3 offset2;

    public static Chunk GetChunk(Vector3 wPos)
    {

        if (wPos.y >= 30 || wPos.y < 0 || wPos.x < 0 || wPos.z < 0)
        {

            return null;
        }
        Vector2Int ChunkIndex = new Vector2Int((int)wPos.x / width, (int)wPos.z / width);

        if (ChunkIndex.x >= 0 && ChunkIndex.y >= 0 && ChunkIndex.x < ChunkNum && ChunkIndex.y < ChunkNum)
        {

            return AllChunks[ChunkIndex.x, ChunkIndex.y];
        }
        return null;
    }


    void Awake()
    {

        map = new BlockType[width, height, width];
        IsShowFace = new bool[width, height, width];
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }

    public void InitMap()
    {
        //初始化时将自己加入chunks列表
        /* chunks.Add(this);
         Debug.Log(transform.position + ":" + chunks.Count);*/
        //获取自身相关组件引用
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
        //初始化随机种子
        Random.InitState(DateTime.Now.Second);
        //3个偏移量
        offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);

        //初始化Map
        

        //遍历map，生成其中每个Block的信息
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    map[x, y, z] = GenerateBlockType(new Vector3(x, y, z) + transform.position);
                }
            }
        }

        //根据生成的信息，Build出Chunk的网格

    }
    public void InitMap(int[] mapInt)
    {

        
        IsShowFace = new bool[width, height, width];
        // BlockType[,,] bt = new BlockType[width, height, width];
        int p = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    int index = mapInt[p++];
                    map[i, j, k] = (BlockType)Enum.ToObject(typeof(BlockType), index);

                }
            }
        }
        // map = bt;
        //根据生成的信息，Build出Chunk的网格

    }

    int GenerateHeight(Vector3 wPos)
    {

        //让随机种子，振幅，频率，应用于噪音采样结果
        float x0 = (wPos.x + offset0.x) * frequency;
        float y0 = (wPos.y + offset0.y) * frequency;
        float z0 = (wPos.z + offset0.z) * frequency;

        float x1 = (wPos.x + offset1.x) * frequency * 2;
        float y1 = (wPos.y + offset1.y) * frequency * 2;
        float z1 = (wPos.z + offset1.z) * frequency * 2;

        float x2 = (wPos.x + offset2.x) * frequency / 4;
        float y2 = (wPos.y + offset2.y) * frequency / 4;
        float z2 = (wPos.z + offset2.z) * frequency / 4;

        float noise0 = Noise.Generate(x0, y0, z0) * amplitude;
        float noise1 = Noise.Generate(x1, y1, z1) * amplitude / 2;
        float noise2 = Noise.Generate(x2, y2, z2) * amplitude / 4;

        float Amplitude = noise0 + noise1 + noise2;
        Amplitude = Mathf.Clamp(Amplitude, -3.1f, 3.1f);
        //在采样结果上，叠加上baseHeight，限制随机生成的高度下限
        return Mathf.FloorToInt(Amplitude + baseHeight);
    }

    /*/// <summary>
    /// 生成小块，设置类型，这里为分层采样
    /// </summary>
    /// <param name="wPos"></param>
    /// <returns></returns>
    BlockType GenerateBlockType(Vector3 wPos)
    {
        //y坐标是否在Chunk内
        if (wPos.y >= height) //当前方块位置高于随机生成的高度值时，当前方块类型为空
        {
            return BlockType.None;
        }
        else if (wPos.y == 0)
        {
            //基岩
            return BlockType.Bedrock;
        }

        //获取当前位置方块随机生成的高度值
        float genHeight = GenerateHeight(wPos);

        if (wPos.y > genHeight)
        {
            return BlockType.None;
        }
        else
        {
            return (BlockType)Random.Range(1, 8);
        }
    }*/
    /// <summary>
    /// 生成小块，设置类型，这里为分层采样
    /// </summary>
    /// <param name="wPos"></param>
    /// <returns></returns>
    //int wolfNum = 0;
    BlockType GenerateBlockType(Vector3 wPos)
    {
        if (MapManager.Instance.IsEditMode)
        {
            if (wPos.y == 0)
            {
                //基岩
                return BlockType.Bedrock;
            }
            else
            {
                return BlockType.None;
            }
        }
        else
        {
            //y坐标是否在Chunk内
            if (wPos.y > height) //当前方块位置高于随机生成的高度值时，当前方块类型为空
            {
                return BlockType.None;
            }

            else if (wPos.y == 0)
            {
                //基岩
                return BlockType.Bedrock;
            }

            //获取当前位置方块随机生成的高度值
            float genHeight = GenerateHeight(wPos);
            Vector2Int ChunkIndex = new Vector2Int((int)wPos.x / width, (int)wPos.z / width);
            if (ChunkIndex.x < 3 && ChunkIndex.y < 3)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.Meat;
                }
                if (genHeight > 5)
                {
                    return BlockType.MoonCake;
                }
                if (genHeight > 4)
                {
                    return BlockType.Cheese;
                }
                if (genHeight > 3)
                {
                    return BlockType.Cake2;
                }
                if (genHeight > 2)
                {
                    return BlockType.LoveCube;
                }
                if (genHeight > 1)
                {
                    return BlockType.DragonFruit;
                }
                return BlockType.Bedrock;
            }
            else if (ChunkIndex.x < 3 && ChunkIndex.y < 6)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.BurningWheat;
                }
                if (genHeight > 5)
                {
                    return BlockType.Cake1;
                }
                if (genHeight > 4)
                {
                    return BlockType.Cake;
                }
                if (genHeight > 3)
                {
                    return BlockType.Kiwi;
                }
                if (genHeight > 2)
                {
                    return BlockType.Trap;
                }
                if (genHeight > 1)
                {
                    return BlockType.GravityCube;
                }
                return BlockType.Bedrock;
            }
            else if (ChunkIndex.x < 6 && ChunkIndex.y < 3)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.ExplotiveCube;
                }
                if (genHeight > 5)
                {
                    return BlockType.Meat;
                }
                if (genHeight > 4)
                {
                    return BlockType.Orange;
                }
                if (genHeight > 3)
                {
                    return BlockType.Cheese;
                }
                if (genHeight > 2)
                {
                    return BlockType.DragonFruit;
                }
                if (genHeight > 1)
                {
                    return BlockType.LoveCube;
                }
                return BlockType.Bedrock;
            }
            else if (ChunkIndex.x < 3 && ChunkIndex.y < 9)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.MoonCake;
                }
                if (genHeight > 5)
                {
                    return BlockType.BurningWheat;
                }
                if (genHeight > 4)
                {
                    return BlockType.Cake2;
                }
                if (genHeight > 3)
                {
                    return BlockType.Cake;
                }
                if (genHeight > 2)
                {
                    return BlockType.GravityCube;
                }
                if (genHeight > 1)
                {
                    return BlockType.Trap;
                }
                return BlockType.Bedrock;
            }
            else if (ChunkIndex.x < 9 && ChunkIndex.y < 3)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.Cake1;
                }
                if (genHeight > 5)
                {
                    return BlockType.ExplotiveCube;
                }
                if (genHeight > 4)
                {
                    return BlockType.Kiwi;
                }
                if (genHeight > 3)
                {
                    return BlockType.Orange;
                }
                if (genHeight > 2)
                {
                    return BlockType.LoveCube;
                }
                if (genHeight > 1)
                {
                    return BlockType.DragonFruit;
                }
                return BlockType.Bedrock;
            }
            else if (ChunkIndex.x < 6 && ChunkIndex.y < 6)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.MoonCake;
                }
                if (genHeight > 5)
                {
                    return BlockType.Meat;
                }
                if (genHeight > 4)
                {
                    return BlockType.Cheese;
                }
                if (genHeight > 3)
                {
                    return BlockType.Cake2;
                }
                if (genHeight > 2)
                {
                    return BlockType.LoveCube;
                }
                if (genHeight > 1)
                {
                    return BlockType.DragonFruit;
                }
                return BlockType.GravityCube;
            }
            else if (ChunkIndex.x < 6 && ChunkIndex.y < 9)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.BurningWheat;
                }
                if (genHeight > 5)
                {
                    return BlockType.Cake1;
                }
                if (genHeight > 4)
                {
                    return BlockType.Kiwi;
                }
                if (genHeight > 3)
                {
                    return BlockType.Orange;
                }
                if (genHeight > 2)
                {
                    return BlockType.Trap;
                }
                if (genHeight > 1)
                {
                    return BlockType.GravityCube;
                }
                return BlockType.Bedrock;
            }
            else if (ChunkIndex.x < 9 && ChunkIndex.y < 6)
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.Cake1;
                }
                if (genHeight > 5)
                {
                    return BlockType.MoonCake;
                }
                if (genHeight > 4)
                {
                    return BlockType.Cake;
                }
                if (genHeight > 3)
                {
                    return BlockType.Cake2;
                }
                if (genHeight > 2)
                {
                    return BlockType.DragonFruit;
                }
                if (genHeight > 1)
                {
                    return BlockType.LoveCube;
                }
                return BlockType.Bedrock;
            }
            else
            {
                if (wPos.y > genHeight)
                {
                    return BlockType.None;
                }
                if (genHeight > 6)
                {
                    return BlockType.Cheese;
                }
                if (genHeight > 5)
                {
                    return BlockType.Cheese;
                }
                if (genHeight > 4)
                {
                    return BlockType.ExplotiveCube;
                }
                if (genHeight > 3)
                {
                    return BlockType.ExplotiveCube;
                }
                if (genHeight > 2)
                {
                    return BlockType.Trap;
                }
                if (genHeight > 1)
                {
                    return BlockType.Trap;
                }
                return BlockType.Bedrock;
            }
            //else
            //{
            //    return (BlockType)Random.Range(1, 16);
            //}
        }
    }
    /// <summary>
    /// 生成Chunk
    /// </summary>
	public void BuildChunk()
    {
        //先实例网格，以及各种顶点数据
        chunkMesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();

        //遍历chunk, 生成其中的每一个Block
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    BuildBlock(x, y, z, verts, uvs, tris);
                }
            }
        }
        chunkMesh.vertices = verts.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = tris.ToArray();
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMesh;
    }

    void BuildBlock(int x, int y, int z, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        //类型为空方块
        if (map[x, y, z] == 0 || map[x, y, z] == BlockType.Object)
        {
            IsShowFace[x, y, z] = true;
            return;
        }


        BlockType typeid = map[x, y, z];

        switch (typeid)
        {
            case BlockType.None:
                break;
            case BlockType.DragonFruit:
                break;
            case BlockType.Cake1:
                break;
            case BlockType.LoveCube:
                break;
            case BlockType.ExplotiveCube:
                break;
            case BlockType.GravityCube:
                if (y > 0)
                {
                    if (GetBlock(x, y - 1, z) == BlockType.None)
                    {
                        map[x, y, z] = BlockType.None;
                        MessageManager.Instance.InstanceCube(typeid, transform.position + new Vector3(x, y, z) + new Vector3(0.5f, 0.5f, 0.5f));
                        return;
                    }
                }
                break;
            case BlockType.Cake:
                break;
            case BlockType.Object:
                break;
            default:
                break;
        }

        bool IsShow = false;
        //六个面
        //Left
        if (CheckNeedBuildFace(x - 1, y, z))
        {
            IsShow = true;
            BuildFace(typeid, FaceType.Left, new Vector3(x, y, z), Vector3.up, Vector3.forward, false, verts, uvs, tris);
        }

        //Right
        if (CheckNeedBuildFace(x + 1, y, z))
        {
            IsShow = true;
            BuildFace(typeid, FaceType.Right, new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, verts, uvs, tris);
        }


        //Bottom
        if (CheckNeedBuildFace(x, y - 1, z))
        {
            IsShow = true;
            BuildFace(typeid, FaceType.Bottom, new Vector3(x, y, z), Vector3.forward, Vector3.right, false, verts, uvs, tris);
        }

        //Top
        if (CheckNeedBuildFace(x, y + 1, z))
        {
            IsShow = true;
            BuildFace(typeid, FaceType.Top, new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, verts, uvs, tris);
        }


        //Back
        if (CheckNeedBuildFace(x, y, z - 1))
        {
            IsShow = true;
            BuildFace(typeid, FaceType.Back, new Vector3(x, y, z), Vector3.up, Vector3.right, true, verts, uvs, tris);
        }

        //Front
        if (CheckNeedBuildFace(x, y, z + 1))
        {
            IsShow = true;
            BuildFace(typeid, FaceType.Front, new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, verts, uvs, tris);
        }
        //存下这个方块是否绘制了面
        IsShowFace[x, y, z] = IsShow;
    }

    /// <summary>
    /// 检查面是否生成
    /// 需改进
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    bool CheckNeedBuildFace(int x, int y, int z)
    {
        BlockType type;
        if (y < 0)
        {
            return false;
        }
        if (y < 0 || y > height - 1 || x < 0 || z < 0 || x >= width || z >= width)//在外面
        {
            //type = GetBlock(new Vector3(x, y, z) + transform.position);
            type = GetBlock(transform.position + new Vector3(x, y, z));
        }
        else//里面
        {
            type = map[x, y, z];
        }
        switch (type)
        {
            case BlockType.None:
                return true;
            case BlockType.Object:
                return true;
            default:
                return false;
        }
    }
    /// <summary>
    /// 通过世界坐标获取方块类型
    /// </summary>
    /// <param name="wPos"></param>
    /// <returns></returns>
    public static BlockType GetBlock(Vector3 wPos)
    {
        Chunk chunk = GetChunk(wPos);
        if (!chunk)
        {
            return BlockType.None;
        }
        Vector3 Pos = wPos - chunk.transform.position;

        return chunk.map[(int)Pos.x, (int)Pos.y, (int)Pos.z];
    }
    /// <summary>
    /// 内部
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    BlockType GetBlock(int x, int y, int z)
    {
        return map[x, y, z];
    }
    /// <summary>
    /// 通过世界坐标获取方块是否被绘制了面
    /// </summary>
    /// <param name="wPos"></param>
    /// <returns></returns>
    public static bool GetBlockIsShow(Vector3 wPos)
    {
        Chunk chunk = GetChunk(wPos);
        if (!chunk)
        {
            return false;
        }
        Vector3 Pos = wPos - chunk.transform.position;
        return chunk.IsShowFace[(int)Pos.x, (int)Pos.y, (int)Pos.z];
    }
    void BuildFace(BlockType typeid, FaceType faceType, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);
        float offset = 0f;
        Vector2 uvWidth = new Vector2(1f / x_num, 1f / y_num);//计算宽高
        Vector2 uvCorner = new Vector2(offset, offset);                //贴图坐标原点
        Vector2 uvWidthset = new Vector2(uvWidth.x - offset, uvWidth.y - offset);

        uvCorner.x += ((float)(faceType) / x_num);
        uvCorner.y += ((float)(typeid - 1) / y_num);
        uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidthset.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidthset.x, uvCorner.y + uvWidthset.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidthset.x, uvCorner.y));

        if (reversed)
        {
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 0);
        }
        else
        {
            tris.Add(index + 1);
            tris.Add(index + 0);
            tris.Add(index + 2);
            tris.Add(index + 3);
            tris.Add(index + 2);
            tris.Add(index + 0);
        }
    }
    /// <summary>
    /// 通过外部坐标改变
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    /// <param name="SetBlock"></param>
    /// <returns></returns>
    public void ChangeBlock(Vector3 wPos, BlockType SetBlock)
    {
        if (GetChunk(wPos) == this)
        {
            Vector3 Pos = wPos - transform.position;
            int x = (int)Pos.x;
            int y = (int)Pos.y;
            int z = (int)Pos.z;
            map[x, y, z] = SetBlock;
            if (SetBlock == BlockType.Object)
            {
                return;
            }
            BuildChunk();
            //BuildChunk();

            if (SetBlock == BlockType.None)
            {
                if (x == 0)//在外面
                {
                    //type = GetBlock(new Vector3(x, y, z) + transform.position);
                    Chunk chunk = GetChunk(transform.position + new Vector3(x - 1, y, z));
                    if (chunk)
                    {
                        chunk.BuildChunk();
                    }
                }
                else if (x == width - 1)
                {
                    Chunk chunk = GetChunk(transform.position + new Vector3(x + 1, y, z));
                    if (chunk)
                    {
                        chunk.BuildChunk();
                    }
                }

                if (z == 0)//在外面
                {
                    //type = GetBlock(new Vector3(x, y, z) + transform.position);
                    Chunk chunk = GetChunk(transform.position + new Vector3(x, y, z - 1));
                    if (chunk)
                    {
                        chunk.BuildChunk();

                    }
                }
                else if (z == width - 1)
                {
                    Chunk chunk = GetChunk(transform.position + new Vector3(x, y, z + 1));
                    if (chunk)
                    {
                        chunk.BuildChunk();
                    }
                }
            }
        }
    }
    /*private void OnDrawGizmos()
    {
        if (NineChunks[3,3]!=this)
        {
            return;
        }
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < width; z++)
                {
                    if (!IsShowFace[x,y,z])
                    {
                        Gizmos.DrawWireCube(transform.position + new Vector3(x + 0.5f, y + 0.5f, z + 0.5f),Vector3.one);
                    }
                }
            }
        }
    }*/

    public void SetBlock(int x, int y, int z, BlockType SetBlock)
    {
        map[x, y, z] = SetBlock;
    }

    public void SetBlock(Vector3 wPos, BlockType setBlock)
    {
        if (GetChunk(wPos) == this)
        {
            Vector3 Pos = wPos - transform.position;
            SetBlock((int)Pos.x, (int)Pos.y, (int)Pos.z, setBlock);
        }
    }
}


