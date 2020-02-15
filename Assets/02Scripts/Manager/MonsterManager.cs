using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    //Dictionary<int, int> MonsterCount;
    //Dictionary<int, Vector3> MonsterPosition;
    Queue<int> Monsters = new Queue<int>();
    Queue<Vector3> MonsterPos = new Queue<Vector3>();
    public float Timer = 5f;
    // Start is called before the first frame update
    void Start()
    {
        MessageManager.Instance.CreatMonster += CreatMonster;
    }
    private void CreatMonster(Vector3 pos, int id)
    {
        Debug.Log("555");
            Monsters.Enqueue(id);
            MonsterPos.Enqueue(pos);
    }
    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer<=0)
        {
            Timer = 5f;
            if (Monsters.Count>0)
            {
                int id = Monsters.Dequeue();
                Vector3 pos = MonsterPos.Dequeue();

                GameObject game = Resources.Load<GameObject>("Prefabs/PlayMode/" + id);
                GameObject monster = Instantiate(game);
                game.transform.position = pos;
            }
        }
    }
}
