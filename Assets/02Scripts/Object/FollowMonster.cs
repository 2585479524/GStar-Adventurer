using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMonster : MonoBehaviour
{
    private float time = 1f;
    private Transform Monster;
    private bool IsFollw=true;
    //Range Distance=new Range ()
    // Start is called before the first frame update
    void Start()
    {
        transform.SetParent ( GameObject.Find("Canvas").transform);
        transform.SetSiblingIndex(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFollw)
        {
            Vector2 po = Camera.main.WorldToScreenPoint(Monster.position+Monster.forward*0.5f+new Vector3(0,1f,0));
            //float dis = (Camera.main.transform.position - Monster.position).sqrMagnitude;
            transform.position = po;
        }
    }
    public void Follow(Transform monster)
    {
        Monster = monster;
        IsFollw = true;
        Invoke("DestoryThis", 2f);
    }
    private void DestoryThis()
    {
       // DestroyImmediate
        Destroy(gameObject);
    }
}
