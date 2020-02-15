using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using LitJson;
using System.IO;

public static class Saver
{
    //public static string MapDataPath = Application.dataPath + "/StreamingAssets/Save";
    public static string WritePath = Application.persistentDataPath;
    public static string SteamPath=Application.streamingAssetsPath + "/Save";
    public static string LevelPath = SteamPath + "/Level";

    
    /// <summary>
    /// 写入Json数据
    /// </summary>
    /// <param name="data">Json字符串</param>
    public static void WriteJsonString(string data, string Path)
    {
        FileInfo file = new FileInfo(Path);
        StreamWriter writer = file.CreateText();
        writer.Write(data);
        writer.Close();
        writer.Dispose();
    }
    /// <summary>
    /// 读取Json数据
    /// </summary>
    /// <returns>Json字符串</returns>
    public static string ReadJsonString(string Path)
    {
        StreamReader reader = new StreamReader(Path);
        string JsonData = reader.ReadToEnd();
        reader.Close();
        reader.Dispose();
        return JsonData;
    }
}
