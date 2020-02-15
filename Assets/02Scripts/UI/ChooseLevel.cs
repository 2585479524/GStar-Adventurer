using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    private int levelAmount;
    private int oldLevelNum = 1;
    private int currentLevelNum = 1;

    private GameObject levelUIprefab;
    private GameObject leftBtn;
    private GameObject rightBtn;
    private float levelWidth;
    private GameObject[] levelsContent;
    private LevelUI[] levelUIs;    //设置关卡UI脚本

    private float oldTargetPosX = float.MinValue;
    private float currentTargetPosX = float.MinValue;

    private float moveDistance;


    public void Init(bool IsFirst)
    {
        currentLevelNum = GameManager.Instance.LoadLevel;
        levelAmount = GameManager.Instance.LevelMess.Count;
        levelUIs = new LevelUI[levelAmount];
        levelsContent = new GameObject[levelAmount];

        leftBtn = GameObject.Find("LeftBtn");
        rightBtn = GameObject.Find("RightBtn");
        SetBtnActive();

        //关卡UIprefab
        levelUIprefab = Resources.Load<GameObject>("Prefabs/UIPrefab/Level");
        GameObject temp = Instantiate(levelUIprefab, transform);
        levelWidth = temp.GetComponent<RectTransform>().sizeDelta.x;
        Destroy(temp);
        moveDistance = (Screen.width + levelWidth) / 2;


        for (int i = 0; i < levelAmount; i++)
        {
            GameObject tempLevelObj = Instantiate(levelUIprefab, transform);
            levelUIs[i] = tempLevelObj.GetComponent<LevelUI>();
            levelsContent[i] = tempLevelObj.transform.GetChild(0).gameObject;
            levelsContent[i].SetActive(false);

            if (i > GameManager.Instance.LoadLevel - 1) 
            {
                tempLevelObj.transform.position = new Vector3(Screen.width / 2 + moveDistance, Screen.height / 2, 0);
                levelsContent[i].SetActive(false);
            }

            else if(i < GameManager.Instance.LoadLevel - 1)
            {
                tempLevelObj.transform.position = new Vector3(Screen.width / 2 - moveDistance, Screen.height / 2, 0);
                levelsContent[i].SetActive(false);
            }

            else if(i == GameManager.Instance.LoadLevel - 1)
            {
                tempLevelObj.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            }

            //设置UI
            levelUIs[i].SetLevelName(GameManager.Instance.LevelMess[i].Name);
            levelUIs[i].SetDifficulty(GameManager.Instance.LevelMess[i].diff);
            levelUIs[i].SetPuddingAmount(GameManager.Instance.LevelMess[i].EatPudding);
            levelUIs[i].SetSeal(GameManager.Instance.LevelMess[i].IsPass);
            if (i == GameManager.Instance.LoadLevel - 1) 
            {
                levelsContent[i].SetActive(true);
            }

            transform.parent.gameObject.SetActive(!IsFirst);
        }


        //for (int i = 0; i < levelAmount; i++)
        //{
        //    levelsContent[i] = transform.GetChild(i).GetChild(0).gameObject;
        //    levelsContent[i].SetActive(false);
        //    if (i != 0)
        //    {
        //        Transform temp = levelsContent[i].transform.parent;
        //        temp.position = new Vector3(temp.position.x + moveDistance, temp.position.y, temp.position.z);
        //    }
        //}
        //levelsContent[0].SetActive(true);
    }

    /// <summary>
    /// 左
    /// </summary>
    public void OnLeftBtnClick()
    {
        if (oldTargetPosX != float.MinValue && levelsContent[oldLevelNum - 1].transform.parent.position.x != oldTargetPosX)
        {
            return;
        }

        oldLevelNum = currentLevelNum;
        currentLevelNum--;
        oldTargetPosX = levelsContent[oldLevelNum - 1].transform.parent.position.x + moveDistance;
        currentTargetPosX= levelsContent[currentLevelNum- 1].transform.parent.position.x + moveDistance;

        levelsContent[oldLevelNum - 1].transform.parent.DOMoveX(oldTargetPosX, 0.5f).SetEase(Ease.OutQuad).onComplete += SetInActive;
        levelsContent[currentLevelNum - 1].transform.parent.DOMoveX(currentTargetPosX, 0.5f).SetEase(Ease.OutQuad).onComplete += SetActive;

        SetBtnActive();
        //设置选定关卡
        GameManager.Instance.LoadLevel = currentLevelNum;
    }

    /// <summary>
    /// 右
    /// </summary>
    public void OnRightBtnClick()
    {
        if (oldTargetPosX != float.MinValue && levelsContent[oldLevelNum - 1].transform.parent.position.x != oldTargetPosX)
        {
            return;
        }

        oldLevelNum = currentLevelNum;
        currentLevelNum++;
        oldTargetPosX = levelsContent[oldLevelNum - 1].transform.parent.position.x - moveDistance;
        currentTargetPosX = levelsContent[currentLevelNum - 1].transform.parent.position.x - moveDistance;

        levelsContent[oldLevelNum - 1].transform.parent.DOMoveX(oldTargetPosX, 0.5f).SetEase(Ease.OutQuad).onComplete += SetInActive;
        levelsContent[currentLevelNum - 1].transform.parent.DOMoveX(currentTargetPosX, 0.5f).SetEase(Ease.OutQuad).onComplete += SetActive;

        SetBtnActive();
        //设置选定关卡
        GameManager.Instance.LoadLevel = currentLevelNum;
    }

    private void SetBtnActive()
    {
        if (currentLevelNum <= 1)
        {
            leftBtn.SetActive(false);
        }
        else
        {
            leftBtn.SetActive(true);
        }

        if (currentLevelNum >= levelAmount)
        {
            rightBtn.SetActive(false);
        }
        else
        {
            rightBtn.SetActive(true);
        }
    }

    private void SetActive()
    {
        levelsContent[currentLevelNum - 1].SetActive(true);
    }

    private void SetInActive()
    {
        levelsContent[oldLevelNum - 1].SetActive(false);
    }

}
