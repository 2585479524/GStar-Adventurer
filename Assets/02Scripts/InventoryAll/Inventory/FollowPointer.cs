using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPointer : MonoBehaviour
{
    private Image icon;
    private Text amountText;
    private Vector2 originPos;
    private Sprite[] iconSprites;

    private void Awake()
    {
        icon = GetComponent<Image>();
        amountText = GetComponentInChildren<Text>();
        originPos = transform.position;
        iconSprites = Resources.LoadAll<Sprite>("Sprite/Icon");
    }

    /// <summary>
    /// 跟随位置时数据设置
    /// </summary>
    public void Follow(Vector2 pos, int id,int amount)
    {
        if (icon.sprite == null) 
        {
            icon.sprite = iconSprites[id];
            amountText.text = amount.ToString();
        }
        transform.position = pos;
    }

    /// <summary>
    /// 返回原位置
    /// </summary>
    public void RetuenOriginPos()
    {
        transform.position = originPos;
        icon.sprite = null;
        amountText.text = null;
    }
}
