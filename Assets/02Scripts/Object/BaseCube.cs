using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCube 
{

    private int cubeId;  // 方块id对应BlockType顺序
    private string cubeName;  // 方块Name
    private string imgUrl; // 方块图片
    private string describe; // 描述
    private string tag; // 描述
    private bool isFrist = false; // 是否第一次遇见，默认false

    public int CubeId
    {
        get => cubeId;
        set => cubeId = value;
    }
    public string CubeName {
        get => cubeName;
        set => cubeName = value;
    }
    public string ImgUrl
    {
        get => imgUrl;
        set => imgUrl = value;
    }

    public string Describe
    {
        get => describe;
        set => describe = value;
    }

    public string Tag
    {
        get => tag;
        set => tag = value;
    }

    public bool isFirst { get; set; }

    public BaseCube(int cubeId, string cubeName, string imgUrl, string describe, string tag)
    {
        this.cubeId = cubeId;
        this.cubeName = cubeName;
        this.imgUrl = imgUrl;
        this.describe = describe;
        this.tag = tag;
    }
}
