using System;
using System.Collections;

using UnityEngine.Networking;

public class SpreadSheet
{
    private const string Address = "docs.google.com/spreadsheets/d/1iAhd-F2alKkXx3hBXTL829rfhTECfSJmKYLoc7iZfUo";
    private const string Range = "A2:E";
    private const string SheetID = "0";
    private const string format = "{0}/export?format=tsv&range={1}&gid={2}";

    public IEnumerator LoadData()
    {
        var req = UnityWebRequest.Get(string.Format(format, Address, Range, SheetID));
        yield return req.SendWebRequest();

        var data = GetData(req.downloadHandler.text);
    }

    private Data[] GetData(string readData)
    {
        var splitedByLine = readData.Split("\n");
        var returnData = new Data[splitedByLine.Length];

        string[] splitedByTab;
        DateTime date;
        int stepCount;
        int distacne;
        float timeSeconds;
        float averageSpeed;
        for (int i = 0; i < splitedByLine.Length; i++)
        {
            splitedByTab = splitedByLine[i].Split("\t");
            if (splitedByTab.Length != 5)
                continue;

            var data = new Data();
            if (DateTime.TryParse(splitedByTab[0], out date))
                data.Date = date;
            if (int.TryParse(splitedByTab[1], out stepCount))
                data.StepCount = stepCount;
            if (int.TryParse(splitedByTab[2], out distacne))
                data.Distance = distacne;
            if (float.TryParse(splitedByTab[3], out timeSeconds))
                data.TimeSeconds = timeSeconds;
            if (float.TryParse(splitedByTab[4], out averageSpeed))
                data.AverageSpeed = averageSpeed;

            returnData[i] = data;
        }

        return returnData;
    }
}

[Serializable]
public class Data
{
    public DateTime Date;
    public int StepCount;
    public int Distance;
    public float TimeSeconds;
    public float AverageSpeed;
}