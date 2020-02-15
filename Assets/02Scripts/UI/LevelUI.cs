using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    public Text levelName;
    public GameObject[] difficultyStars;
    public GameObject[] puddings;
    public GameObject seal;


    public void SetLevelName(string name)
    {
        levelName.text = name;
    }

    public void SetDifficulty(int difficulty)
    {
        for(int i = difficulty; i < difficultyStars.Length; i++)
        {
            difficultyStars[i].SetActive(false);
        }
    }

    public void SetPuddingAmount(int amount)
    {
        for(int i = amount; i < puddings.Length; i++)
        {
            puddings[i].SetActive(false);
        }
    }

    public void SetSeal(bool canShow)
    {
        seal.SetActive(canShow);
    }

    public void IsStart()
    {
        TurnScene.Instance.Turn(2);
    }
}
