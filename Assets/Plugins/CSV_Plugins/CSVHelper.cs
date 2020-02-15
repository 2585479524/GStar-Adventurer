using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ReadCSVFinished(CSVTable result);

public class CSVHelper : MonoBehaviour
{
    #region singleton
    private static GameObject container = null;
    private static CSVHelper instance = null;
    public static CSVHelper Instance()
    {
        if (instance == null)
        {
            container = new GameObject("CSVHelper");
            instance = container.AddComponent<CSVHelper>();
        }
        return instance;
    }
    #endregion

    #region mono
    void Awake()
    {
        DontDestroyOnLoad(container);
    }
    #endregion

    #region private members
    //不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
    public static readonly string csvFilePath = Application.streamingAssetsPath + "/Config/";
    Dictionary<string, CSVTable> readedTable = null;
    #endregion

    #region public interfaces
    public void ReadCSVFile(string fileName, ReadCSVFinished callback)
    {

        if (readedTable == null)
            readedTable = new Dictionary<string, CSVTable>();
        CSVTable result;
        if (readedTable.TryGetValue(fileName, out result))
        {
            Debug.LogWarning(string.Format("CSVHelper ReadCSVFile: You already read the file:{0}", fileName));
            return;
        }
        StartCoroutine(LoadCSVCoroutine(fileName, callback));
    }

    public CSVTable SelectFrom(string tableName)
    {
        CSVTable result = null;
        if (!readedTable.TryGetValue(tableName, out result))
        {
            Debug.LogError(string.Format("CSVHelper SelectFrom: The table you want to get data from is not readed. table name:{0}", tableName));
        }
        return result;
    }
    #endregion

    #region private imp
    private IEnumerator LoadCSVCoroutine(string fileName, ReadCSVFinished callback)
    {
        string fileFullName = csvFilePath + fileName + ".csv";
        using (WWW www = new WWW(fileFullName))
        {
            yield return www;
            string text = string.Empty;
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError(string.Format("CSVHelper LoadCSVCoroutine:Load file failed file = {0}, error message = {1}", fileFullName, www.error));
                yield break;
            }
            text = www.text;
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError(string.Format("CSVHelper LoadCSVCoroutine:Loaded file is empty file = {0}", fileFullName));
                yield break;
            }
            CSVTable table = ReadTextToCSVTable(text);
            readedTable.Add(fileName, table);
            if (callback != null)
            {
                callback.Invoke(table);
            }
        }
    }

    private CSVTable ReadTextToCSVTable(string text)
    {
        CSVTable result = new CSVTable();
        text = text.Replace("\r", "");
        string[] lines = text.Split('\n');
        if (lines.Length < 2)
        {
            Debug.LogError("CSVHelper ReadTextToCSVData: Loaded text is not csv format");//必需包含一行键，一行值，至少两行
        }
        string[] keys = lines[0].Split(',');//第一行是键
        for (int i = 1; i < lines.Length; i++)//第二行开始是值
        {
            CSVLine curLine = new CSVLine();
            string line = lines[i];
            if (string.IsNullOrEmpty(line.Trim()))//略过空行
            {
                break;
            }
            string[] items = line.Split(',');
            string key = items[0].Trim();//每一行的第一个值是唯一标识符
            for (int j = 0; j < items.Length; j++)
            {
                string item = items[j].Trim();
                curLine[keys[j]] = item;
            }
            result[key] = curLine;
        }
        return result;
    }
    #endregion
}
