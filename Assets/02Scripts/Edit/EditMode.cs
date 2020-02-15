using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 场景编辑工具
/// </summary>
public class EditMode : MonoBehaviour
{
    //区别哪些是物体，哪些地形
    public int ObjecyStartIndex = 12;
    public float UpDownSpeed = 5f;
    private Vector3 CamerPos;
    public Text Mode;
    public Transform Evn;
   // public GameObject[] Prefabs;
    public Text Pos;
    private Vector3 MousePos;
    GameObject hitObject;
    Chunk pointChunk;
    public Text Name;
    public Text Count;
    bool Isdele;
    private int Num = 1;
    float timer = 0.2f;
    bool CanOp = false;
    private int SeleNum
    {
        set
        {
            Num = value;
            toggles[value - 1].isOn = true;
        }
        get
        {
            return Num;
        }
    }

    public Toggle[] toggles;
    public void SetSele(int num)
    {
        Num = num;
    }
    public int GetSele()
    {
        return SeleNum;
    }
    // Start is called before the first frame update
    void Start()
    {
        CamerPos = transform.GetChild(0).localPosition;

        GetComponent<Rigidbody>().useGravity = false;

        GetComponent<CapsuleCollider>().enabled = false;

        transform.GetChild(0).position = transform.position;

        //MapManager.Instance.UpdateWorld();
    }

    // Update is called once per frame
    void Update()
    {
        //编辑器模式
        //飞行
        Count.text = "场景物体数量:" + Evn.transform.childCount;
        if (CanOp && Input.GetMouseButton(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                CanOp = false;
                if (Isdele)
                {
                    //删除
                    //chunk.ChangeBlock(MouseSeleCubePos, BlockType.None);
                    if (pointChunk)
                    {
                        pointChunk.ChangeBlock(MousePos, BlockType.None);
                    }
                    else
                    {
                        Chunk chunk = Chunk.GetChunk(MousePos);
                        int id = int.Parse(hitObject.name.Substring(0, 2));
                        switch (id)
                        {
                            case 23:
                                for (int i = 0; i < 2; i++)
                                {
                                    chunk.SetBlock(hitObject.transform.position + Vector3.up * i, BlockType.None);
                                }
                                break;
                            case 24:
                                for (int i = 0; i < 6; i++)
                                {
                                    chunk.SetBlock(hitObject.transform.position + Vector3.up * i, BlockType.None);
                                }
                                break;
                            case 25:
                                for (int i = 0; i < 4; i++)
                                {
                                    chunk.SetBlock(hitObject.transform.position + Vector3.up * i, BlockType.None);
                                }
                                break;
                            case 26:
                                for (int i = 0; i < 1; i++)
                                {
                                    chunk.SetBlock(hitObject.transform.position + Vector3.up * i, BlockType.None);
                                }
                                break;
                            case 27:
                                for (int i = 0; i < 2; i++)
                                {
                                    chunk.SetBlock(hitObject.transform.position + Vector3.up * i, BlockType.None);
                                }
                                break;
                            case 28:
                                for (int i = 0; i < 1; i++)
                                {
                                    chunk.SetBlock(hitObject.transform.position + Vector3.up * i, BlockType.None);
                                }
                                break;
                        }
                        Destroy(hitObject);
                    }
                }
                else
                {
                    //增加
                    /*if (Num > 0 && Num <= Prefabs.Length)
                    {
                        GameObject Cube = Instantiate(Prefabs[Num - 1], Evn);
                        Cube.transform.position = MousePos;
                    }*/
                    if (SeleNum < ObjecyStartIndex)
                    {
                        pointChunk.ChangeBlock(MousePos, (BlockType)(SeleNum));
                    }
                    else
                    {
                        GameObject pre = Resources.Load<GameObject>("Prefabs/EditMode/" + SeleNum);
                        GameObject Cube = Instantiate(pre, Evn);
                        Cube.transform.position = MousePos;
                        Chunk chunk = Chunk.GetChunk(MousePos);
                        switch (SeleNum)
                        {
                            case 23:
                                for (int i = 0; i < 2; i++)
                                {
                                    chunk.SetBlock(Cube.transform.position + Vector3.up * i, BlockType.Object);
                                }
                                break;
                            case 24:
                                for (int i = 0; i < 6; i++)
                                {
                                    chunk.SetBlock(Cube.transform.position + Vector3.up * i, BlockType.Object);
                                }
                                break;
                            case 25:
                                for (int i = 0; i < 4; i++)
                                {
                                    chunk.SetBlock(Cube.transform.position + Vector3.up * i, BlockType.Object);
                                }
                                break;
                            case 26:
                                for (int i = 0; i < 1; i++)
                                {
                                    chunk.SetBlock(Cube.transform.position + Vector3.up * i, BlockType.Object);
                                }
                                break;
                            case 27:
                                for (int i = 0; i < 2; i++)
                                {
                                    chunk.SetBlock(Cube.transform.position + Vector3.up * i, BlockType.Object);
                                }
                                break;
                            case 28:
                                for (int i = 0; i < 1; i++)
                                {
                                    chunk.SetBlock(Cube.transform.position + Vector3.up * i, BlockType.Object);
                                }
                                break;
                        }
                    }
                }
            }
        }
        //物体才可以旋转
        if (hitObject && !pointChunk && CanOp && Input.GetMouseButton(1))
        {
            CanOp = false;
            hitObject.transform.Rotate(new Vector3(0, 90, 0));
        }
        if (!CanOp)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0.2f;
                CanOp = true;
            }
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Mode.text = "删除模式";
            Isdele = true;
        }
        if (Input.GetKey(KeyCode.E))
        {
            Mode.text = "添加模式";
            Isdele = false;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * Time.deltaTime * UpDownSpeed;
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.position += Vector3.down * Time.deltaTime * UpDownSpeed;
        }

        // Vector3 forward = transform.TransformDirection(Vector3.forward);
        // Debug.Log(forward);
        // Vector3 right = transform.TransformDirection(Vector3.right);
        float h1 = Input.GetAxis("Horizontal");
        float v1 = Input.GetAxis("Vertical");

        transform.position += (transform.forward * v1 * 0.7f);
        transform.position += (transform.right * h1 * 0.7f);
        // transform.forward += new Vector3(h1, 0, v1) * Time.deltaTime * Speed;

        if (Input.GetKey(KeyCode.Alpha1))
        {
            SeleNum = 1;
        }
        else if (Input.GetKey(KeyCode.Alpha2))
        {
            SeleNum = 2;
        }
        else if (Input.GetKey(KeyCode.Alpha3))
        {
            SeleNum = 3;
        }
        else if (Input.GetKey(KeyCode.Alpha4))
        {
            SeleNum = 4;
        }
        else if (Input.GetKey(KeyCode.Alpha5))
        {
            SeleNum = 5;
        }
        else if (Input.GetKey(KeyCode.Alpha6))
        {
            SeleNum = 6;
        }
        else if (Input.GetKey(KeyCode.Alpha7))
        {
            SeleNum = 7;
        }
        else if (Input.GetKey(KeyCode.Alpha8))
        {
            SeleNum = 8;
        }
        else if (Input.GetKey(KeyCode.Alpha9))
        {
            SeleNum = 9;
        }
        else if (Input.GetKey(KeyCode.Alpha0))
        {
            SeleNum = 10;
        }
    }
    bool GetMouseRayPoint(out Vector3 addCubePosition)
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        //LayerMask layerMask = 1 << 10|18;//只检测层级为10
        if (Physics.Raycast(ray, out hitInfo, 99999f))
        {
            hitObject = hitInfo.collider.gameObject;

            // Debug.DrawRay(hitInfo.point, Vector3.up, Color.red);
            Vector3 point;
            if (Isdele)
            {
                point = hitInfo.point + ray.direction * 0.2f;
            }
            else
            {
                point = hitInfo.point - ray.direction * 0.2f;
            }

            addCubePosition = Character.FromWorldPositionToCubePosition(point);
            if (hitInfo.collider.tag == "Chunk")
            {
                pointChunk = hitObject.GetComponent<Chunk>();
                BlockType blockType = Chunk.GetBlock(hitInfo.point + ray.direction * 0.2f);
                Name.text = blockType.ToString();
            }
            else
            {
                pointChunk = null;
                Name.text = hitObject.name;
            }

            return true;
        }
        else
        {
            hitObject = null;
            Name.text = "";
        }
        addCubePosition = Vector3.zero;
        return false;
    }


    private void OnDrawGizmos()
    {
        if (Isdele)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        if (GetMouseRayPoint(out MousePos))
        {
            Gizmos.DrawWireCube(MousePos, Vector3.one);
            Pos.text = "坐标" + MousePos.ToString();
        }
        else
        {
            Pos.text = "未选中";
        }
    }
}
