using System;
using System.IO;

using UnityEngine;

public static class DataManager
{
    private const string fileName = "Data";

    public static UserData UserData;

    public static void SaveData()
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        CheckDate();
        var json = SerializeData();
        File.WriteAllText(path, json);
    }

    public static void LoadData()
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path) == false)
        {
            var data = new Data()
            {
                Date = DateTime.Now,
                StepCount = 0,
                Distance = 0,
                TimeSeconds = 0,
                AverageSpeed = 0,
            };

            UserData = new UserData()
            {
                Datas = new Data[] { data },
                Playing = false,
                Recycling = false,
                Pallette = 0,
                DateType = 0,
            };
            return;
        }

        var jsonData = File.ReadAllText(path);
        UserData = DeserializeData(jsonData);
        CheckDate();
    }

    public static void CheckDate()
    {
        if (UserData.CurrentData.Date.Date == DateTime.Now.Date)
            return;

        var prevDates = UserData.Datas;
        var updateDates = new Data[prevDates.Length + 1];
        Array.Copy(prevDates, updateDates, prevDates.Length);
        UserData.Datas = updateDates;
    }

    private static string SerializeData()
    {
        return JsonUtility.ToJson(UserData);
    }

    private static UserData DeserializeData(string jsonData)
    {
        return JsonUtility.FromJson<UserData>(jsonData);
    }
}

[Serializable]
public class UserData
{
    public Data[] Datas;
    public Data CurrentData { get { return Datas[Datas.Length - 1]; } }

    public bool Playing;
    public bool Recycling;
    public int Pallette;
    public int DateType;
}

[Serializable]
public class Data
{
    public DateTime Date;
    public int StepCount;
    public float Distance;
    public float TimeSeconds;
    public float AverageSpeed;
}