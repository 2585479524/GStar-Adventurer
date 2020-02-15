using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public GameObject oneCamera;
    public GameObject twoCamera;

    private void Start()
    {
        //twoCamera.SetActive(false);
    }

    public void SwitchCameraFun()
    {
        oneCamera.SetActive(!oneCamera.activeSelf);
        twoCamera.SetActive(!twoCamera.activeSelf);
    }
}
