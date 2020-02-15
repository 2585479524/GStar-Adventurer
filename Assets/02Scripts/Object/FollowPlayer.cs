using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 Offect=new Vector3(0,10,-6); 
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        Offect = transform.position - player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + Offect;
    }
}
