using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditUI : MonoBehaviour
{
    public GameObject LevelPanal;
    private bool IsClose=true;
    public Text text;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SeleLevel()
    {       
        LevelPanal.SetActive(IsClose);
        IsClose = !IsClose;
    }
    public void EditLevel(int Level)
    {
        text.text = "当前编辑的关卡:" + Level;
        IsClose = !IsClose;
        MapManager.Instance.LoadLevel(Level);
    }
}
