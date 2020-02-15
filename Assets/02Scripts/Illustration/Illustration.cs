using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Illustration : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> cubeItemList;

    [HideInInspector]
    public List<GameObject> monsterItemList;

    public GameObject prefab;

    public GameObject cubeContent;

    public GameObject monsterContent;

    private Color Imagecolor;
    void Awake()
    {
        cubeItemList = new List<GameObject>();
        monsterItemList = new List<GameObject>();

        Imagecolor = new Color();
        Imagecolor.a = 0;

        Invoke("InitItem", 0.1f);
    }

    void Update()
    {

    }

    void InitItem()
    {
        // 初始化方块图鉴
        for (int i = 0; i < Consts.cubeList.Count; i++)
        {
            GameObject g = Instantiate(prefab, cubeContent.transform.position, Quaternion.identity, transform);
            g.transform.SetParent(cubeContent.transform);
            cubeItemList.Add(g);
            // 初始化小图效果

            // 没有图鉴：按预制体初始化（none有，name有，shadow有，role无）
            // if已有图鉴：none无、name有、shadow有，role有
            if (Consts.cubeList[i].isFirst == true)
            {
                Debug.Log("初始化图鉴小图");
                g.transform.Find("Text_name").gameObject.GetComponent<Text>().text = Consts.cubeList[i].CubeName;
                g.transform.Find("Img_none").gameObject.GetComponent<Image>().color = Imagecolor;
                g.transform.Find("Img_role").gameObject.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Texture/CubeList")[Consts.cubeList[i].CubeId];
                g.transform.Find("Img_role").gameObject.GetComponent<Image>().SetNativeSize();
                g.transform.Find("Img_role").gameObject.GetComponent<Image>().color = Color.white;
            }

            int index = i;
            cubeItemList[i].GetComponentInChildren<Button>().onClick.AddListener(delegate () { ViewCube(index); });
        }
        // 初始化怪物图鉴
        for (int i = 0; i < Consts.AIList.Count; i++)
        {
            GameObject g = Instantiate(prefab, monsterContent.transform.position, Quaternion.identity, transform);
            g.transform.parent = monsterContent.transform;
            monsterItemList.Add(g);

            if (Consts.AIList[i].IsFirst == true)
            {
                Debug.Log("初始化图鉴小图");
                g.transform.Find("Text_name").gameObject.GetComponent<Text>().text = Consts.AIList[i].AiName;
                g.transform.Find("Img_none").gameObject.GetComponent<Image>().color = Imagecolor;
                g.transform.Find("Img_role").gameObject.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Texture/MonsterList")[Consts.AIList[i].Aiid];
                g.transform.Find("Img_role").gameObject.GetComponent<Image>().SetNativeSize();
                g.transform.Find("Img_role").gameObject.GetComponent<Image>().color = Color.white;
            }

            int index = i;
            monsterItemList[i].GetComponentInChildren<Button>().onClick.AddListener(delegate () { ViewMonster(index); });
        }
    }

    public void ViewCube(int bt)
    {
        // 获取索引
        GameObject Img_Lift = gameObject.transform.Find("CubeViewPanel/Img_Lift").gameObject;
        GameObject Text_CubeName = gameObject.transform.Find("CubeViewPanel/Text_CubeName").gameObject;
        GameObject Text_CubeName_Shadow = gameObject.transform.Find("CubeViewPanel/Text_CubeName_Shadow").gameObject;
        GameObject Text_Describe = gameObject.transform.Find("CubeViewPanel/Text_Describe").gameObject;
        GameObject Text_Tag = gameObject.transform.Find("CubeViewPanel/Text_Tag").gameObject;

        // 清空上次
        for (int i = 0; i < cubeItemList.Count; i++)
        {
            Imagecolor = new Color(255,255,255,255);
            cubeItemList[i].transform.Find("Img_shadow").gameObject.GetComponent<Image>().color = Imagecolor;

            Text_Describe.GetComponent<Text>().text = "　　　？？？";
            Text_Tag.GetComponent<Text>().text = "？？？";
        }

            BaseCube cube = Consts.cubeList[bt];

        // 点击后小图无效果 或者 none无，name有，shadow无，role有


        // 点击后大图效果:if已有图鉴
        Img_Lift.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Texture/CubeList")[Consts.cubeList[bt].CubeId];
        Img_Lift.GetComponent<Image>().SetNativeSize();
        Img_Lift.GetComponent<Image>().color = Color.black;
        Text_CubeName.GetComponent<Text>().text = "" + cubeItemList[bt].transform.Find("Text_name").gameObject.GetComponent<Text>().text;
        Text_CubeName_Shadow.GetComponent<Text>().text = "" + cubeItemList[bt].transform.Find("Text_name").gameObject.GetComponent<Text>().text; 

        if (Consts.cubeList[bt].isFirst == true)
            {
            Imagecolor.a = 0;
            cubeItemList[bt].transform.Find("Img_shadow").gameObject.GetComponent<Image>().color = Imagecolor;
            Debug.Log("如果当前图鉴已存在，图片显示，颜色正常，文字正常");
            Img_Lift.GetComponent<Image>().color = Color.white;
            Text_Describe.GetComponent<Text>().text = "　　　" + cube.Describe;
            Text_Tag.GetComponent<Text>().text = "" + cube.Tag;
            Img_Lift.GetComponent<Image>().color = Color.white;
        }


        

    }
    public void ViewMonster(int bt)
    {

        // 获取索引
        GameObject Img_Lift = gameObject.transform.Find("MonsterViewPanel/Img_Lift").gameObject;
        GameObject Text_CubeName = gameObject.transform.Find("MonsterViewPanel/Text_CubeName").gameObject;
        GameObject Text_CubeName_Shadow = gameObject.transform.Find("MonsterViewPanel/Text_CubeName_Shadow").gameObject;
        GameObject Text_Describe = gameObject.transform.Find("MonsterViewPanel/Text_Describe").gameObject;
        GameObject Text_Attack = gameObject.transform.Find("MonsterViewPanel/Text_Attack").gameObject;
        GameObject Text_Health = gameObject.transform.Find("MonsterViewPanel/Text_Health").gameObject;
        GameObject Text_Speed = gameObject.transform.Find("MonsterViewPanel/Text_Speed").gameObject;

        // 清空上次
        for (int i = 0; i < monsterItemList.Count; i++)
        {
            Imagecolor = new Color(255, 255, 255, 255);
            monsterItemList[i].transform.Find("Img_shadow").gameObject.GetComponent<Image>().color = Imagecolor;

            Text_Describe.GetComponent<Text>().text = "　　　？？？";
            Text_Attack.GetComponent<Text>().text = "？？？";
            Text_Health.GetComponent<Text>().text = "？？？";
            Text_Speed.GetComponent<Text>().text = "？？？";
        }

        AIInfo monster = Consts.AIList[bt];

        // 点击后小图无效果 或者 none无，name有，shadow无，role有

        // 点击后大图效果:if已有图鉴
        Img_Lift.GetComponent<Image>().sprite = Resources.LoadAll<Sprite>("Texture/MonsterList")[Consts.AIList[bt].Aiid];
        Img_Lift.GetComponent<Image>().SetNativeSize();
        Img_Lift.GetComponent<Image>().color = Color.black;
        Text_CubeName.GetComponent<Text>().text = "" + monsterItemList[bt].transform.Find("Text_name").gameObject.GetComponent<Text>().text;
        Text_CubeName_Shadow.GetComponent<Text>().text = "" + monsterItemList[bt].transform.Find("Text_name").gameObject.GetComponent<Text>().text;

        if (Consts.AIList[bt].IsFirst == true)
        {
            Imagecolor.a = 0;
            monsterItemList[bt].transform.Find("Img_shadow").gameObject.GetComponent<Image>().color = Imagecolor;
            Debug.Log("如果当前图鉴已存在，图片显示，颜色正常，文字正常");
            Img_Lift.GetComponent<Image>().color = Color.white;
            Text_Describe.GetComponent<Text>().text = "　　　" + monster.Describe1;
            Text_Attack.GetComponent<Text>().text = "" + monster.Attack;
            Text_Health.GetComponent<Text>().text = "" + monster.Health;
            Text_Speed.GetComponent<Text>().text = "" + monster.Speed;
            Img_Lift.GetComponent<Image>().color = Color.white;
        }
    }


}
