using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSlot : MonoBehaviour
{
    private Image icon;
    private Text amountText;
    private BlockType storedBlock = BlockType.None;

    public Sprite[] iconSprites;

    private void Awake()
    {
        icon = transform.Find("Background").GetComponent<Image>();
        amountText = transform.Find("Background/Amount").GetComponentInChildren<Text>();
        //iconSprites = Resources.LoadAll<Sprite>("Sprite/Icon");
    }
    public void SetUI(NewInventoryData data)
    {
        if (data.blockType == BlockType.None)
        {
            icon.color = new Color(1, 1, 1, 0);
            icon.sprite = null;
            amountText.text = null;
        }

        else
        {
            //if (storedBlock != data.blockType)
            //{
            //    icon.color = new Color(1, 1, 1, 1);
            //    icon.sprite = iconSprites[(int)data.blockType];
            //}
            //amountText.text = data.amount.ToString();
            //storedBlock = data.blockType;

            amountText.text = data.amount.ToString();
            icon.color = new Color(1, 1, 1, 1);
            icon.sprite = iconSprites[(int)data.blockType-1];
        }
    }

}
