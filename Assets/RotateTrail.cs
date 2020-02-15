using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrail : MonoBehaviour
{

    public float rotateSpeed = 300;

    // Update is called once per frame
    void Update()
    {


        transform.Rotate(Vector3.up, Time.deltaTime * rotateSpeed);

    }
}
