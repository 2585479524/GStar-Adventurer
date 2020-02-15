using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBox : MonoBehaviour
{
    public GameObject box;

    private void Start()
    {
        box.SetActive(false);
    }
    public void OnOpenBoxBtnClick()
    {
        box.SetActive(!box.activeSelf);
    }

}
