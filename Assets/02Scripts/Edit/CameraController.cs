using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed;
    private new Camera camera;

    // Use this for initialization
    void Start()
    {
        // 形变组件transform,与该脚本直接关联上的组件就是transform
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 得到鼠标当前位置
        float mouseX = Input.GetAxis("Mouse X") * speed;
        float mouseY = Input.GetAxis("Mouse Y") * speed;
        // 设置照相机和Player的旋转角度，X,Y值需要更具情况变化位置
        camera.transform.localRotation = camera.transform.localRotation * Quaternion.Euler(-mouseY, 0, 0);
        transform.localRotation = transform.localRotation * Quaternion.Euler(0, mouseX, 0);

    }
}