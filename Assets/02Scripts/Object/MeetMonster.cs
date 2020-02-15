using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeetMonster : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Role"))
        {
            //见到怪物，更新数据
            int monsterID = other.GetComponent<AIController>().ID;
            if (!Consts.AIList[monsterID].IsFirst)
            {
                //更新数据
                Consts.AIList[monsterID].IsFirst = true;
                //调用提示UI
                MessageManager.Instance.MeetMonsterHander(monsterID);
            }
        }
    }
}
