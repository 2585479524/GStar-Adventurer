using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    // 挂载传送门信息的UI
    public Image doorMsg;
    // 场景ID
    public int sceneId = 1;
    // 偏移量
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, 50, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        doorMsg.gameObject.SetActive(true);
    }

    void OnTriggerStay()
    {
        MsgFollowEnemy();
        if (Input.GetKeyUp(KeyCode.Q))
        {
            EnterScene();
        }
    }

    void OnTriggerExit()
    {
        doorMsg.gameObject.SetActive(false);
    }

    /// <summary>
    /// UI位置跟随传送门
    /// </summary>
    void MsgFollowEnemy()
    {
        doorMsg.transform.position = Camera.main.WorldToScreenPoint(transform.position) + offset;
    }
    /// <summary>
    /// 根据ID切换场景
    /// </summary>
    public void EnterScene()
    {
        SceneManager.LoadSceneAsync(sceneId);
    }

}
