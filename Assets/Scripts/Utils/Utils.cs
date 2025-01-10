using UnityEngine;

public static class Utils
{
    public static int GetRandomIndex(this int currentIndex, int range)
    {
        int returnValue;

        do
        {
            returnValue = Random.Range(0, range);
        }
        while (currentIndex == returnValue);

        return returnValue;
    }

    public static string FormatTime(this float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }
}