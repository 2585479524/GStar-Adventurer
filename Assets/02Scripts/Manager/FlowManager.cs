using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class FlowManager : MonoBehaviour
{
    public Character Character;
    private UIManager UIManager;
    public int PuddingNum = 0;
    public GameObject fail;
    public GameObject success;
    private void Start()
    {
        UIManager = GetComponent<UIManager>();
        UIManager.Init();       
        MessageManager.Instance.RoleDeath+=EndGameDeath;
        MessageManager.Instance.EatPudding += Eatpudding;
        PuddingNum = 0;
        Character.Init();
    }

    private void EndGameDeath()
    {
        fail.SetActive(true);
        Debug.Log("主角死亡");
        GameManager.Instance.LevelEndToSave(false, PuddingNum);
        SoundManager.Instance.PlaySfx("PlayerDie", transform.position);
        Invoke("ShowEnd", 3f);
    }
    private void Eatpudding()
    {
        PuddingNum++;
        if (PuddingNum >= 3)
        {
            success.SetActive(true);
            Debug.Log("胜利");
            GameManager.Instance.LevelEndToSave(true, PuddingNum);
            SoundManager.Instance.PlaySfx("Victory1", transform.position);
            Invoke("ShowEnd", 2f);
            MessageManager.Instance.MainCharShowWin();
        }
    }
    public void ShowEnd()
    {
        Debug.Log("结束");
        TurnScene.Instance.Turn(1);
    }
    private void Update()
    {

    }
}

