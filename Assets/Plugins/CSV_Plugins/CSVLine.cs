using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVLine : IEnumerable
{
    private Dictionary<string, string> dataContainer = new Dictionary<string, string>();

    /// <summary>
    /// 添加一行表项
    /// </summary>
    /// <param name="key">string</param>
    /// <param name="value">string</param>
    private void AddItem(string key, string value)
    {
        if (dataContainer.ContainsKey(key))
        {
            Debug.LogError(string.Format("CSVLine AddItem: there is a same key you want to add. key = {0}", key));
        }
        else
        {
            dataContainer.Add(key, value);
        }
    }

    public string this[string key]
    {
        get { return dataContainer[key]; }
        set { AddItem(key, value); }
    }
    public IEnumerator GetEnumerator()
    {
        foreach (KeyValuePair<string, string> item in dataContainer)
        {

            yield return item;
        }

    }
}