using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BaseSlot : MonoBehaviour, IDragHandler, IPointerUpHandler 
{
    [HideInInspector]
    public BaseItem storedItem = null; //存储物品
    public int storedAmount;           //存储物品数量
    protected Sprite storedSprite = null;

    public int slotIndex;

    protected Image icon;
    protected Text amountText;

    protected FollowPointer followPointer;

    protected int inventorySlotLayer;
    protected int boxSlotLayer;
    protected RaycastHit2D hit2D;
    protected RaycastHit2D hit2DSelf;

    protected bool isDrag = false;

    protected virtual void Awake()
    {
        followPointer = GameObject.Find("FollowPointerIcon").GetComponent<FollowPointer>();
        inventorySlotLayer = LayerMask.NameToLayer("InventorySlot");
        boxSlotLayer = LayerMask.NameToLayer("BoxSlot");
    }

    /// <summary>
    /// 设置物品槽UI
    /// </summary>
    public void SetSlotUI(Sprite iconSprite, int amount, BaseItem item)
    {
        storedAmount = amount;
        storedItem = item;
        storedSprite = iconSprite;

        if (amount == 0)
        {
            icon.color = new Color(1, 1, 1, 0);
            icon.sprite = null;
            amountText.text = null;

            storedSprite = null;
            storedAmount = 0;
            storedItem = null;
        }
        else
        {
            icon.color = new Color(1, 1, 1, 1);
            icon.sprite = iconSprite;
            amountText.text = amount.ToString();
        }
    }

    public void RemoveSlotUI()
    {
        icon.color = new Color(1, 1, 1, 0);
        icon.sprite = null;
        amountText.text = null;

        storedSprite = null;
        storedAmount = 0;
        storedItem = null;
    }

    public void AddSlotUI(int amount)
    {
        storedAmount += amount;
        amountText.text = storedAmount.ToString();
    }

    /// <summary>
    /// 设置被选择物品ID
    /// </summary>
    /// <param name="index"></param>
    public virtual void SetSelectItemID() { }

    public virtual void OnDrag(PointerEventData eventData)
    {
        if (storedItem == null)
        {
            return;
        }

        isDrag = true;
        followPointer.Follow(eventData.position, storedItem.ID, storedAmount);

        icon.color = new Color(1, 1, 1, 0);
        icon.sprite = null;
        amountText.text = null;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        followPointer.RetuenOriginPos();
    }
}
