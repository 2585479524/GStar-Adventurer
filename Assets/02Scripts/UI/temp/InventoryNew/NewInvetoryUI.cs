using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewInvetoryUI : MonoBehaviour
{
    private NewSlot[] slots;
    private Toggle[] toggles;

    private void Awake()
    {
        slots = GetComponentsInChildren<NewSlot>();
        toggles = GetComponentsInChildren<Toggle>();
         MessageManager.Instance.UpdataSlotUI += UpdataSlotsUI;
         MessageManager.Instance.UpdateSelectedToggle += UpdateSelectedToggle;
    }

    private void UpdataSlotsUI(NewInventoryData[] datas)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            slots[i].SetUI(datas[i]);
        }
    }

    /// <summary>
    /// 设置选中的Toggle
    /// </summary>
    private void UpdateSelectedToggle(int index)
    {
        toggles[index + 1].isOn = true;
    }

    public void SetSelectBlock(int index)
    {
        NewInventoryManager.Instance.SetSelectBlock(index);
    }
}
