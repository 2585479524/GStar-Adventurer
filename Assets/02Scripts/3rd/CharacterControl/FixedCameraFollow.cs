using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraFollow : MonoBehaviour
{
    public Transform target;
    //public float speed;

    private Vector3 offest;

    private void Start()
    {
        offest = transform.position - target.position;
    }

    private void LateUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, target.position + offest, speed * Time.deltaTime);
        transform.position = target.position + offest;
    }
}
