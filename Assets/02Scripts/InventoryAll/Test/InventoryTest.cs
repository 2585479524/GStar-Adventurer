using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    private List<int> ids = new List<int>();

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            //BoxManager.Instance.StoreItemByType(new BaseItem(Random.Range(0, 9), 0, 1), 2);

            InventoryManager.Instance.AddItemByType(new BaseItem(Random.Range(0, 9), 1, 1), 2);
        }
    }
    void Update()
    {
        //背包测试

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    InventoryManager.Instance.AddItem(new BaseItem(Random.Range(0, 9), 0, 1));
        //}

        //else if (Input.GetKeyDown(KeyCode.R))
        //{
        //    InventoryManager.Instance.RemoveSelectItem();
        //}


        //箱子测试
        if (Input.GetKeyDown(KeyCode.A))
        {
            int id = Random.Range(0, 9);
            ids.Add(id);
            BoxManager.Instance.StoreItemByType(new BaseItem(id, 0, 1), 2);
            //BoxManager.Instance.StoreItem(new BaseItem(0, 0, 1));
        }

        //else if (Input.GetKeyDown(KeyCode.R))
        //{
        //    if (ids.Count > 0)
        //    {
        //        int l = ids.Count - 1;
        //        BoxManager.Instance.GetItemById(ids[l]);
        //        ids.RemoveAt(l);
        //    }

        //    //BoxManager.Instance.GetItemById(0);
        //}

    }
}


