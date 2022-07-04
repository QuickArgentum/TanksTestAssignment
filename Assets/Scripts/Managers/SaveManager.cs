using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : Singleton<SaveManager>
{
    public string filename;

    private string path;

    private void Start()
    {
        path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + filename;
        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    private void Load()
    {
        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        SaveData data = null;
        try
        {
            data = JsonUtility.FromJson<SaveData>(json);
        }
        catch(Exception) { }

        if (data == null)
            data = new SaveData();

        ScoreManager.Instance.SetData(data.score);
    }

    private void Save()
    {
        string json = JsonUtility.ToJson(CreateData());

        StreamWriter writer = new StreamWriter(path);
        writer.Write(json);
    }

    private SaveData CreateData()
    {
        SaveData data = new SaveData();
        data.score = ScoreManager.Instance.GetData();

        return data;
    }
}