using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSVTable : IEnumerable
{
    private Dictionary<string, CSVLine> dataContainer = new Dictionary<string, CSVLine>();

    private void AddLine(string key, CSVLine line)
    {
        if (dataContainer.ContainsKey(key))
        {
            Debug.LogError(string.Format("CSVTable AddLine: there is a same key you want to add. key = {0}", key));
        }
        else
        {
            dataContainer.Add(key, line);
        }
    }
    public CSVLine this[string key]
    {
        get { return dataContainer[key]; }
        set { AddLine(key, value); }
    }

    public IEnumerator GetEnumerator()
    {
        foreach (var item in dataContainer)
        {
            yield return item.Value;
        }
    }

    public CSVLine WhereIDEquals(int id)
    {
        CSVLine result = null;
        if (!dataContainer.TryGetValue(id.ToString(), out result))
        {
            Debug.LogError(string.Format("CSVTable WhereIDEquals: The line you want to get data from is not found. id:{0}", id));
        }
        return result;
    }
}