using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using TMPro;
using System;

public class RecordUIHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalStepCountText = default;
    [SerializeField] private TextMeshProUGUI totalDistanceText = default;
    [SerializeField] private TextMeshProUGUI totalTimeText = default;
    [SerializeField] private TextMeshProUGUI averageStepCountText = default;
    [SerializeField] private TextMeshProUGUI averageDistanceText = default;
    [SerializeField] private TextMeshProUGUI averageTimeText = default;
    [SerializeField] private TextMeshProUGUI averageSpeedText = default;

    public void Initialize()
    {
        int totalStepCount = 0;
        float totalDistance = 0f;
        float totalTime = 0f;
        float totalSpeed = 0f;

        var data = DataManager.Instance.UserData.Datas;
        var dataLength = data.Length - 1;

        for (int i = 0; i < dataLength; i++)
        {
            totalStepCount += data[i].StepCount;
            totalDistance += data[i].Distance;
            totalTime += data[i].TimeSeconds;
            totalSpeed += data[i].AverageSpeed;
        }

        if (dataLength == 0)
            dataLength = 1;

        totalStepCountText.text = string.Format("(T)Step: {0}", totalStepCount);
        totalDistanceText.text = string.Format("(T)Dist: {0} m", (int)totalDistance);
        totalTimeText.text = string.Format("(T)Time: {0} s", (int)totalTime);
        averageStepCountText.text = string.Format("(A)Step: {0}", (int)(totalStepCount / dataLength));
        averageDistanceText.text = string.Format("(A)Dist: {0} m/d", (int)(totalDistance / dataLength));
        averageTimeText.text = string.Format("(A)Time: {0} s/d", (int)(totalTime / dataLength));
        averageSpeedText.text = string.Format("(A)Speed: {0} m/s/d", (int)(totalSpeed / dataLength));

        HideRecordUI();
    }

    public void ShowRecordUI() => gameObject.SetActive(true);

    public void HideRecordUI() => gameObject.SetActive(false);

    public void OnClickedCloseButton() => HideRecordUI();
}
