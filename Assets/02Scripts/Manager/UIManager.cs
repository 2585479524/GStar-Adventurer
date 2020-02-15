using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 全局UI管理
/// 通过UIManager.Instance获取单例
/// </summary>
public class UIManager : Singleton<UIManager>
{
    #region UI层引用
    public Sprite[] EatShoot;
    public Image Hander;
    public Image EatTime;
    public Image ShootCold;
    public GameObject Shoot;
    private Image[] Hps;
    public FragmentUI FragmentUI;

    public bool IsTouchUI;
    [Header("--------------图鉴--------------")]
    public GameObject IllustrationSystem;

    public Button maskLayerCubeFirst;
    public Button maskLayerMonsterFirst;
    public Image cubeViewPanel;
    public Image monsterViewPanel;

    #endregion

    #region 事件触发

    public void WriteOrReadScene(bool IsRead)
    {
        Debug.Log("保存");
        if (IsRead)
        {
            MessageManager.Instance.ReadScene();
        }
        else
        {
            MessageManager.Instance.WriteScene();
        }
    }

    public void ShowSettings(bool isShow)
    {
        gameObject.SetActive(isShow);
    }


    /// <summary>
    /// 设置面板
    /// </summary>
    /// <param name="isPause"></param>
    public void PauseOrContinue(bool isPause)
    {
        if (isPause)
        {
            // 暂停
            Time.timeScale = 0;
        }
        else
        {
            // 继续
            Time.timeScale = 1;
        }
    }

    public void Exit()
    {
        TurnScene.Instance.Turn(1);
    }


    public void RePlayNow(int scene)
    {
        TurnScene.Instance.Turn(scene);
    }

    public void changeBGM(bool isOpen)
    {
        if (isOpen)
        {
            Debug.Log("音乐打开");
            SoundManager.Instance.SetBGMusic(true);
        }
        else
        {
            Debug.Log("音乐关闭");
            SoundManager.Instance.SetBGMusic(false);
        }
    }

    public void ChangeSoundEffect(bool isOpen)
    {
        if (isOpen)
        {
            Debug.Log("音效打开");

            SoundManager.Instance.SetSFX(true);
        }
        else
        {
            Debug.Log("音效关闭");
            SoundManager.Instance.SetSFX(false);
        }
    }

    public void ShootCube()
    {
        MessageManager.Instance.Shoot();
    }

    private void Update()
    {


    }

    #endregion

    #region 事件监听

    /* protected override void Awake()
     {
         base.Awake();
     }*/

    public void Init()
    {
        Hps = transform.GetChild(0).GetComponentsInChildren<Image>();
        MessageManager.Instance.DeOrAdd += DeOrAddHander;
        MessageManager.Instance.EatTimer += EatTimerHander;
        MessageManager.Instance.HpChange += ShowHp;
        MessageManager.Instance.ShootCold += ShootColdTime;
        MessageManager.Instance.EatPudding += EatPudding;

        //MessageManager.Instance.AddBagItem += EatBlockHander;
        MessageManager.Instance.MeetMonsterHander += MeetMonsterHander;
        MessageManager.Instance.MonsterHaveTarge += InstanceFollowUI;
        MessageManager.Instance.FindBlock += EatBlockHander;
    }

    private void InstanceFollowUI(Transform obj)
    {
        GameObject ga = Resources.Load("Mark") as GameObject;
        GameObject ma = Instantiate(ga);
        ma.GetComponent<FollowMonster>().Follow(obj);
        //childTransform.SetSiblingIndex(count - 1);
    }

    private void EatPudding()
    {
        FragmentUI.AddFragment();
    }

    private void ShootColdTime(float time)
    {
        ShootCold.fillAmount = time / 1f;
    }

    void Start()
    {
        IllustrationSystem.SetActive(false);
    }
    
    private void ShowHp(int obj)
    {
        for (int i = 0; i < Hps.Length; i++)
        {
            if (i <= obj)
            {
                Hps[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                Hps[i].color = new Color(1, 1, 1, 0);
            }
        }
    }
    public void IsTouch(bool IsTouch)
    {
        IsTouchUI = IsTouch;
    }
    #endregion

    #region 事件回调

    void DeOrAddHander(bool dele)
    {
        if (dele)
        {
            Debug.Log("1");
            
            Hander.sprite = EatShoot[0];
            Shoot.SetActive(true);
        }
        else
        {
            Debug.Log("2");
            Shoot.SetActive(false);
            Hander.sprite = EatShoot[1];
        }
    }

    void EatTimerHander(float Timer, float eatTime)
    {
        EatTime.fillAmount = Timer / eatTime;
    }

    public void ChangeCamera()
    {
        MessageManager.Instance.ChangeCamera();
    }

    /// <summary>
    /// 每次吃方块调用
    /// </summary>
    /// <param name="bt">方块类型</param>
    void EatBlockHander(BlockType bt)
    {
        int index = (int) bt - 1;
        Debug.Log("当前吃到的方块类型" + bt);
        if (Consts.cubeList[index].isFirst == false)
        {
            // PauseOrContinue(true);
            GameObject Img_Tip1 = maskLayerCubeFirst.transform.Find("Img_Tip1").gameObject;
            GameObject Img_Lift = cubeViewPanel.transform.Find("Img_Lift").gameObject;
            GameObject Text_CubeName = cubeViewPanel.transform.Find("Text_CubeName").gameObject;
            GameObject Text_CubeName_Shadow = cubeViewPanel.transform.Find("Text_CubeName_Shadow").gameObject;
            GameObject Text_Describe = cubeViewPanel.transform.Find("Text_Describe").gameObject;
            GameObject Text_Tag = cubeViewPanel.transform.Find("Text_Tag").gameObject;

            BaseCube cube = Consts.cubeList[index];

            // 展示panel，文字、文字阴影、描述、类型、图片
            maskLayerCubeFirst.gameObject.SetActive(true);
            Text_CubeName.GetComponent<Text>().text = cube.CubeName + "";
            Text_CubeName_Shadow.GetComponent<Text>().text = cube.CubeName + "";
            Text_Describe.GetComponent<Text>().text = "　　　" + cube.Describe;
            Text_Tag.GetComponent<Text>().text = cube.Tag + "";
            Img_Lift.GetComponent<Image>().sprite =
                Resources.LoadAll<Sprite>("Texture/CubeList")[Consts.cubeList[index].CubeId];
            Img_Lift.GetComponent<Image>().SetNativeSize();
            GameObject g = IllustrationSystem.GetComponent<Illustration>().cubeItemList[index];

            Debug.Log("初始化图鉴小图");
            g.transform.Find("Text_name").gameObject.GetComponent<Text>().text = Consts.cubeList[index].CubeName;

            Color Imagecolor = new Color();
            Imagecolor.a = 0;
            g.transform.Find("Img_none").gameObject.GetComponent<Image>().color = Imagecolor;
            g.transform.Find("Img_role").gameObject.GetComponent<Image>().sprite =
                Resources.LoadAll<Sprite>("Texture/CubeList")[Consts.cubeList[index].CubeId];
            g.transform.Find("Img_role").gameObject.GetComponent<Image>().SetNativeSize();
            g.transform.Find("Img_role").gameObject.GetComponent<Image>().color = Color.white;
            Consts.cubeList[index].isFirst = true;
        }
    }

    /// <summary>
    /// 每次见到新的怪物调用
    /// </summary>
    /// <param name="aiid"></param>
    void MeetMonsterHander(int bt)
    {


        Debug.Log("当前吃到的方块类型" + bt);
        if (Consts.cubeList[bt].isFirst == false)
        {
            // PauseOrContinue(true);

            GameObject Img_Tip1 = maskLayerMonsterFirst.transform.Find("Img_Tip1").gameObject;
            GameObject Img_Lift = monsterViewPanel.transform.Find("Img_Lift").gameObject;
            GameObject Text_CubeName = monsterViewPanel.transform.Find("Text_CubeName").gameObject;
            GameObject Text_CubeName_Shadow = monsterViewPanel.transform.Find("Text_CubeName_Shadow").gameObject;
            GameObject Text_Describe = monsterViewPanel.transform.Find("Text_Describe").gameObject;
            GameObject Text_Attack = monsterViewPanel.transform.Find("Text_Attack").gameObject;
            GameObject Text_Health = monsterViewPanel.transform.Find("Text_Health").gameObject;
            GameObject Text_Speed = monsterViewPanel.transform.Find("Text_Speed").gameObject;

            AIInfo monster = Consts.AIList[bt];

            // 展示panel，文字、文字阴影、描述、类型、图片
            maskLayerMonsterFirst.gameObject.SetActive(true);
            Text_CubeName.GetComponent<Text>().text = monster.AiName + "";
            Text_CubeName_Shadow.GetComponent<Text>().text = monster.AiName + "";
            Text_Describe.GetComponent<Text>().text = "　　　" + monster.Describe1;
            Text_Attack.GetComponent<Text>().text = monster.Attack + "";
            Text_Health.GetComponent<Text>().text = monster.Health + "";
            Text_Speed.GetComponent<Text>().text = monster.Speed + "";
            Img_Lift.GetComponent<Image>().sprite =  Resources.LoadAll<Sprite>("Texture/MonsterList")[Consts.AIList[bt].Aiid];
            Img_Lift.GetComponent<Image>().SetNativeSize();
            GameObject g = IllustrationSystem.GetComponent<Illustration>().monsterItemList[bt];

            Debug.Log("初始化图鉴小图");
            g.transform.Find("Text_name").gameObject.GetComponent<Text>().text = Consts.AIList[bt].AiName;

            Color Imagecolor = new Color();
            Imagecolor.a = 0;
            g.transform.Find("Img_none").gameObject.GetComponent<Image>().color = Imagecolor;
            g.transform.Find("Img_role").gameObject.GetComponent<Image>().sprite =
                Resources.LoadAll<Sprite>("Texture/MonsterList")[Consts.AIList[bt].Aiid];
            g.transform.Find("Img_role").gameObject.GetComponent<Image>().SetNativeSize();
            g.transform.Find("Img_role").gameObject.GetComponent<Image>().color = Color.white;
            Consts.AIList[bt].IsFirst = true;
        }
        
        #endregion
    }
}

