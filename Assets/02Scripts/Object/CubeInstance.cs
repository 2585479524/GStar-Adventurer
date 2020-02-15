using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 实例的方块，用于飞行方块，重力方块
/// 从对象池中取出
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class CubeInstance : ReusbleObject
{
    public BlockType blockType;
    private bool Fly = false;
    private float Speed = 2f;
    private Vector3 Dir;
    //贴图数据
    private const int y_num = 15;
    private const int x_num = 6;

    Mesh chunkMesh;
    MeshRenderer meshRenderer;
    // MeshCollider meshCollider;
    MeshFilter meshFilter;
    bool FlyStop;
    Vector3 StopPos;

    private float Timer=12f;
    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        //meshCollider = GetComponent<MeshCollider>();
        meshFilter = GetComponent<MeshFilter>();
    }
    private void Update()
    {
        if (Fly)
        {
            Timer -= Time.deltaTime;
            transform.position += (Dir * Time.deltaTime * Speed);
        }
        else if(FlyStop)
        {
            transform.position = Vector3.Lerp(transform.position, StopPos, Time.deltaTime*Speed/2);
            if ((transform.position-StopPos).sqrMagnitude<0.05)
            {

                //transform.position = StopPos;
                Chunk chunk= Chunk.GetChunk(transform.position);
                if (chunk)
                {
                    chunk.ChangeBlock(transform.position, blockType);
                }
                ObjectPool.Instance.Unspawn(gameObject);
                //GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionY;
            }
        }
        if (Timer<=0)
        {
            Fly = false;
            FlyStop = false;
            Timer = 12f;
            ObjectPool.Instance.Unspawn(gameObject);
        }
    }
    public void InstanceFly(BlockType Type, Vector3 ShootPos, float speed, Vector3 pos)
    {
        transform.position = pos;
        blockType = Type;
        Speed = speed;
        Dir = ShootPos;
       /* if (Dir.x == 0)
        {
            GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionZ;
        }
        if (Dir.z == 0)
        {
            GetComponent<Rigidbody>().constraints -= RigidbodyConstraints.FreezePositionX;
        }*/
        BuildCube();
        Fly = true;
    }
    public void InstanceGravity(BlockType Type, Vector3 pos)
    {
        Fly = false;
        FlyStop = false;
        transform.position = pos;
        blockType = Type;
        BuildCube();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.constraints -= RigidbodyConstraints.FreezePositionY;
    }
    private void BuildCube()
    {

        //先实例网格，以及各种顶点数据
        chunkMesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> tris = new List<int>();


        BuildCube(new Vector3(-0.5f, -0.5f, -0.5f), verts, uvs, tris);

        chunkMesh.vertices = verts.ToArray();
        chunkMesh.uv = uvs.ToArray();
        chunkMesh.triangles = tris.ToArray();
        chunkMesh.RecalculateBounds();
        chunkMesh.RecalculateNormals();

        meshFilter.mesh = chunkMesh;
        //meshCollider.sharedMesh = chunkMesh;
    }

    void BuildCube(Vector3 corner, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        BlockType typeid = blockType;
        //六个面
        //Left
        BuildFace(typeid, FaceType.Left, corner, Vector3.up, Vector3.forward, false, verts, uvs, tris);
        //Right

        BuildFace(typeid, FaceType.Right, corner + Vector3.right, Vector3.up, Vector3.forward, true, verts, uvs, tris);

        //Bottom

        BuildFace(typeid, FaceType.Bottom, corner, Vector3.forward, Vector3.right, false, verts, uvs, tris);
        //Top

        BuildFace(typeid, FaceType.Top, corner + Vector3.up, Vector3.forward, Vector3.right, true, verts, uvs, tris);

        //Back

        BuildFace(typeid, FaceType.Back, corner, Vector3.up, Vector3.right, true, verts, uvs, tris);
        //Front

        BuildFace(typeid, FaceType.Front, corner + Vector3.forward, Vector3.up, Vector3.right, false, verts, uvs, tris);
    }

    void BuildFace(BlockType typeid, FaceType faceType, Vector3 corner, Vector3 up, Vector3 right, bool reversed, List<Vector3> verts, List<Vector2> uvs, List<int> tris)
    {
        int index = verts.Count;

        verts.Add(corner);
        verts.Add(corner + up);
        verts.Add(corner + up + right);
        verts.Add(corner + right);

        Vector2 uvWidth = new Vector2(1f / x_num, 1f / y_num);//计算宽高
        Vector2 uvCorner = new Vector2(0f, 0f);                //贴图坐标原点
        uvCorner.x += ((float)(faceType) / x_num);
        uvCorner.y += ((float)(typeid - 1) / y_num);
        uvs.Add(uvCorner);
        uvs.Add(new Vector2(uvCorner.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y + uvWidth.y));
        uvs.Add(new Vector2(uvCorner.x + uvWidth.x, uvCorner.y));

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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            //Fly = false;
            Chunk chunk=  Chunk.GetChunk(transform.position);
            if (chunk)
            {
                chunk.ChangeBlock(transform.position, blockType);
            }
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ObjectPool.Instance.Unspawn(gameObject);
        }
        else if (collision.gameObject.layer == 18)
        {
            if (Fly)
            {
                Fly = false;
                if (blockType==BlockType.ExplotiveCube)
                {
                    MessageManager.Instance.InstanceParticle(ParticleType.Boom, transform.position, null);
                    SoundManager.Instance.PlaySfx("Boom", transform.position);
                    collision.gameObject.GetComponent<Role>().Attack(AttackType.BoomBlock, 2, transform.position);
                    ObjectPool.Instance.Unspawn(gameObject);
                }
                else
                {
                    FlyStop = true;
                    GameObject rol = collision.gameObject;
                    StopPos = Character.FromWorldPositionToCubePosition(transform.position += (Dir.normalized / 2));
                    /* Vector3 dir = rol.transform.position - transform.position;
                     dir.y = 1;
                     dir *= 10f;
                     rol.GetComponent<Rigidbody>().AddForce(dir);*/
                    rol.GetComponent<Role>().Attack(AttackType.FlyBlock, 1, transform.position);
                    Debug.Log("造成伤害");
                }
            }
            else
            {
                if (blockType == BlockType.GravityCube)
                {
                    collision.gameObject.GetComponent<Role>().Attack(AttackType.GravityCube, 100, transform.position);
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    ObjectPool.Instance.Unspawn(gameObject);
                }
            }
        }
    }

    //
    public override void OnSpawn()
    {
        Fly = false;
        FlyStop = false;
    }

    public override void OnUnspawn()
    {
        
    }
}

