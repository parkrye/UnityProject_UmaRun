using System;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;

public class DataManager
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
                instance = new DataManager();
            return instance;
        }
    }

    private const string fileName = "Data";

    public UserData UserData;

    public void SaveData()
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        CheckDate();

        var json = SerializeData();
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path) == false)
        {
            CreateNewData();
            return;
        }

        var jsonData = File.ReadAllText(path);
        UserData = DeserializeData(jsonData);
        if (UserData.CurrentData == null)
        {
            CreateNewData();
            return;
        }

        CheckDate();
    }

    public void ResetData()
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(path) == false)
            return;

        File.Delete(path);
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("StartScene");
    }

    public void CheckDate()
    {
        if (UserData.CurrentData.GetDate().Date == DateTime.Now.Date)
            return;

        var prevDates = UserData.Datas;
        var dataLength = prevDates.Length;
        var updateDates = new Data[dataLength + 1];
        Array.Copy(prevDates, updateDates, dataLength);
        updateDates[dataLength] = new Data()
        {
            StepCount = 0,
            Distance = 0,
            TimeSeconds = 0,
            AverageSpeed = 0,
        };
        updateDates[dataLength].SetDate(DateTime.Now);
        UserData.Datas = updateDates;
    }

    private void CreateNewData()
    {
        var data = new Data()
        {
            StepCount = 0,
            Distance = 0,
            TimeSeconds = 0,
            AverageSpeed = 0,
        };
        data.SetDate(DateTime.Now);

        UserData = new UserData()
        {
            Datas = new Data[] { data },
            IsPlaying = false,
            IsRecycling = false,
            PalletteIndex = 0,
            DateType = 0,
        };
    }

    private string SerializeData() => JsonUtility.ToJson(UserData);

    private UserData DeserializeData(string jsonData) => JsonUtility.FromJson<UserData>(jsonData);
}

[Serializable]
public class UserData
{
    public Data[] Datas;
    public Data CurrentData { get { return Datas[Datas.Length - 1]; } }

    public bool IsPlaying = true;
    public bool IsRecycling = true;
    public int MusicIndex = -1;
    public int PalletteIndex = 0;
    public int DateType = 0;
}

[Serializable]
public class Data
{
    public string Date;
    public int StepCount;
    public float Distance;
    public float TimeSeconds;
    public float AverageSpeed;

    public void SetDate(DateTime dateTime)
    {
        Date = dateTime.ToString("O");
    }

    public DateTime GetDate()
    {
        return DateTime.Parse(Date);
    }
}