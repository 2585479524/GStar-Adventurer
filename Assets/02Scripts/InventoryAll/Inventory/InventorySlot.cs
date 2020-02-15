using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : BaseSlot
{
    protected override void Awake()
    {
        base.Awake();
        icon = transform.Find("Background").GetComponent<Image>();
        amountText = transform.Find("Background/Amount").GetComponentInChildren<Text>();
    }

    public override void SetSelectItemID()
    {
        if (storedItem != null)
        {
            InventoryManager.Instance.SelectItem(storedItem.ID);
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        SetSelectItemID();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        hit2D = Physics2D.Raycast(eventData.position, Vector2.down, 1, 1 << boxSlotLayer);
        hit2DSelf= Physics2D.Raycast(eventData.position, Vector2.down, 1, 1 << inventorySlotLayer);

        if (hit2D.collider != null)
        {
            BaseSlot otherSlot = hit2D.collider.GetComponent<BoxSlot>();

            //空
            if (otherSlot.storedItem == null)
            {
                BoxManager.Instance.StoreItemByType(storedItem, storedAmount, false);
                BoxUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);
                //删除原容器原数据
                InventoryManager.Instance.RemoveItemByID(storedItem.ID, storedAmount, false);
                InventoryUI.Instance.RemoveUIByIndex(slotIndex);
            }

            //id相同
            else if (otherSlot.storedItem.ID == storedItem.ID)
            {
                //赋予新容器数据
                BoxManager.Instance.StoreItemByType(storedItem, storedAmount, false);
                BoxUI.Instance.AddUIByIndex(storedAmount, otherSlot.slotIndex);

                //删除原容器原数据
                InventoryManager.Instance.RemoveItemByID(storedItem.ID, storedAmount, false);
                InventoryUI.Instance.RemoveUIByIndex(slotIndex);
            }

            //不同
            else if (otherSlot.storedItem.ID != storedItem.ID)
            {
                BaseItem tempItem = otherSlot.storedItem;
                int tempCount = otherSlot.storedAmount;

                BoxManager.Instance.StoreItemByType(storedItem, storedAmount, false);
                BoxManager.Instance.RemoveItemById(tempItem.ID, tempCount, false);
                BoxUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);

                InventoryManager.Instance.RemoveItemByID(storedItem.ID, storedAmount, false);
                InventoryManager.Instance.AddItemByType(tempItem, tempCount, false);
                InventoryUI.Instance.SetUIBySlotIndex(slotIndex, tempItem, tempCount);
            }
        }

        else if (hit2DSelf.collider != null && isDrag)
        {
            BaseSlot otherSlot = hit2DSelf.collider.GetComponent<InventorySlot>();

            //空
            if (otherSlot.storedItem == null)
            {
                InventoryUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);
                InventoryUI.Instance.RemoveUIByIndex(slotIndex);
            }

            else if (otherSlot.storedItem == storedItem)
            {
                //回到原格
                icon.color = new Color(1, 1, 1, 1);
                icon.sprite = storedSprite;
                amountText.text = storedAmount.ToString();
            }

            //id相同
            else if (otherSlot.storedItem.ID == storedItem.ID)
            {
                InventoryUI.Instance.AddUIByIndex(storedAmount, otherSlot.slotIndex);
                InventoryUI.Instance.RemoveUIByIndex(slotIndex);
            }

            //不同
            else if (otherSlot.storedItem.ID != storedItem.ID)
            {
                BaseItem tempItem = otherSlot.storedItem;
                int tempCount = otherSlot.storedAmount;

                InventoryUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);
                InventoryUI.Instance.SetUIBySlotIndex(slotIndex, tempItem, tempCount);
            }
        }

        else
        {
            if (storedItem == null)
            {
                return;
            }
            //拖动失败，格子恢复
            icon.color = new Color(1, 1, 1, 1);
            icon.sprite = storedSprite;
            amountText.text = storedAmount.ToString();
        }
        isDrag = false;
    }
}
