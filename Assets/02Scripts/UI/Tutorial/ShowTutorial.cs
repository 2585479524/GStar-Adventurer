using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

/// <summary>
/// 教程信息
/// </summary>

[Serializable]
public class TutorialInfo
{
    public string target;
    public string des;
}

public class ShowTutorial : MonoBehaviour
{
    public RectTransform hole;
    public Text des;

    private bool isShowTutorial = false;
    private TutoriaInfos tutoriaInfos;

    private class Info
    {
        public RectTransform target;
        public string des;
        public bool haveShown = false;

        public Info(RectTransform rectTransform, string des)
        {
            target = rectTransform;
            this.des = des;
        }
    }

    private List<Info> infos = new List<Info>();

    private void Awake()
    {
        tutoriaInfos = Resources.Load<TutoriaInfos>("TutoriaInfos/MyTutoriaInfos");
        for(int i = 0; i < tutoriaInfos.tutorialInfos.Count; i++)
        {
            if (tutoriaInfos.tutorialInfos[i].target!="") 
            {
                RectTransform rectTransform = GameObject.Find(tutoriaInfos.tutorialInfos[i].target).GetComponent<RectTransform>();
                Info info = new Info(rectTransform, tutoriaInfos.tutorialInfos[i].des);
                infos.Add(info);
                if (tutoriaInfos.tutorialInfos[i].target == "Shoot")
                {
                    rectTransform.gameObject.SetActive(false);
                }
            }
            else
            {
                Info info = new Info(null, tutoriaInfos.tutorialInfos[i].des);
                infos.Add(info);
            }
        }

        //注册事件
        MessageManager.Instance.TriggerGuidance += ShowTutorialPanel;
       // ShowTutorialPanel(1);
    }

    private void Update()
    {
        if (isShowTutorial)
        {
            if (Input.GetMouseButtonDown(0))
            {
                hole.sizeDelta = new Vector2(4500, 4500);
                des.transform.parent.gameObject.SetActive(false);

            }
        }
    }


    /// <summary>
    /// 显示新手教程
    /// </summary>
    /// <param name="id"></param>
    public void ShowTutorialPanel(int id)
    {
        Debug.Log(id);
        Info temp = infos[id];
        if (temp.haveShown)
        {
            return;
        }

        if (temp.target != null)
        {
            isShowTutorial = false;
            hole.position = temp.target.position;
            des.text = temp.des;
            hole.DOSizeDelta(temp.target.sizeDelta * 1.2f, 1f).onComplete += ShowInfoText;
        }
        else
        {
            des.text = temp.des;
            ShowInfoText();
        }
        temp.haveShown = true;
    }

    public void ShowInfoText()
    {
        des.transform.parent.gameObject.SetActive(true);
        des.transform.parent.localScale = Vector3.zero;
        des.transform.parent.DOScale(Vector3.one, 0.5f).onComplete += () => { isShowTutorial = true; };
    }
}
