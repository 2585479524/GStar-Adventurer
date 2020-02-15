using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoxSlot : BaseSlot
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
            BoxManager.Instance.SelectItem(storedItem.ID);
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

        hit2D = Physics2D.Raycast(eventData.position, Vector2.down, 1, 1 << inventorySlotLayer);
        hit2DSelf= Physics2D.Raycast(eventData.position, Vector2.down, 1, 1 << boxSlotLayer);

        if (hit2D.collider != null)
        {
            BaseSlot otherSlot = hit2D.collider.GetComponent<InventorySlot>();

            //空
            if (otherSlot.storedItem == null)
            {
                InventoryManager.Instance.AddItemByType(storedItem, storedAmount, false);
                InventoryUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);
                //删除原容器原数据
                BoxManager.Instance.RemoveItemById(storedItem.ID, storedAmount, false);
                BoxUI.Instance.RemoveUIByIndex(slotIndex);
            }
            //id相同
            else if (otherSlot.storedItem.ID == storedItem.ID)
            {
                //赋予新容器数据
                InventoryManager.Instance.AddItemByType(storedItem, storedAmount, false);
                InventoryUI.Instance.AddUIByIndex(storedAmount, otherSlot.slotIndex);

                //删除原容器原数据
                BoxManager.Instance.RemoveItemById(storedItem.ID, storedAmount, false);
                BoxUI.Instance.RemoveUIByIndex(slotIndex);
            }
            //不同
            else if (otherSlot.storedItem.ID != storedItem.ID)
            {
                BaseItem tempItem = otherSlot.storedItem;
                int tempCount = otherSlot.storedAmount;

                InventoryManager.Instance.AddItemByType(storedItem, storedAmount, false);
                InventoryManager.Instance.RemoveItemByID(tempItem.ID, tempCount, false);
                InventoryUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);

                BoxManager.Instance.RemoveItemById(storedItem.ID, storedAmount, false);
                BoxManager.Instance.StoreItemByType(tempItem, tempCount, false);
                BoxUI.Instance.SetUIBySlotIndex(slotIndex, tempItem, tempCount);
            }
        }

        else if (hit2DSelf.collider != null && isDrag) 
        {
            BaseSlot otherSlot = hit2DSelf.collider.GetComponent<BoxSlot>();

            //空
            if (otherSlot.storedItem == null)
            {
                BoxUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);
                BoxUI.Instance.RemoveUIByIndex(slotIndex);
            }

            else if (otherSlot.storedItem == storedItem)
            {
                //回到原格
                icon.color = new Color(1, 1, 1, 1);
                icon.sprite = storedSprite;
                amountText.text = storedAmount.ToString();
            }

            //id相同且不是同格
            else if (otherSlot.storedItem.ID == storedItem.ID)
            {
                BoxUI.Instance.AddUIByIndex(storedAmount, otherSlot.slotIndex);
                BoxUI.Instance.RemoveUIByIndex(slotIndex);
            }

            //不同
            else if (otherSlot.storedItem.ID != storedItem.ID)
            {
                BaseItem tempItem = otherSlot.storedItem;
                int tempCount = otherSlot.storedAmount;

                BoxUI.Instance.SetUIBySlotIndex(otherSlot.slotIndex, storedItem, storedAmount);
                BoxUI.Instance.SetUIBySlotIndex(slotIndex, tempItem, tempCount);
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
