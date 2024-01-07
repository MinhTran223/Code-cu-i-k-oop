using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using UnityEngine.iOS;
using JetBrains.Annotations;
using System.Linq;
using Unity.VisualScripting;
using System.Text;

public static class Function
{
    //Lop JsonHelper de doc ghi du lieu ma ko ghi de trong file json
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Account;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Account = array;
            return JsonUtility.ToJson(wrapper);
        }
        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Account = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Account;
        }

    }

    public static string path = Application.dataPath + "/playerdata.json";
    public static void WriteFile(string content)
    {
        FileStream strm = new FileStream(path, FileMode.Create);
        using (StreamWriter writer = new StreamWriter(strm))
        {
            writer.Write(content);
        }
    }
    public static string ReadFile()
    {
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string content = reader.ReadToEnd();
                return content;
            }
        }
        return "";
    }
    public static void Saveinfo<T>(List<T>saveinfo )
    {
        string info = JsonHelper.ToJson<T>(saveinfo.ToArray());
        WriteFile(info);
    }
    public static List<T> Readinfo<T>()
    {
        string content = ReadFile();
        if (string.IsNullOrEmpty(content) || content == "{}")
        {
            return new List<T>();
        }
        List<T> res = JsonHelper.FromJson<T>(content).ToList();
        return res;
    }
}
