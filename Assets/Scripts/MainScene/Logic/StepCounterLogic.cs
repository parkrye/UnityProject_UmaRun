using System;

using UnityEngine;

using TMPro;

public class StepCounterLogic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepCountText = default;
    [SerializeField] private TextMeshProUGUI distanceText = default;
    [SerializeField] private TextMeshProUGUI timeText = default;
    [SerializeField] private TextMeshProUGUI speedText = default;

    // ���� ���� ���� ����
    private int stepCount = 0;
    private float stepLength = 0.75f; // ���� (����)
    private float distance = 0f;
    private DateTime startTime;

    // ���ӵ� ������ ���� ����
    private Vector3 previousAcceleration = Vector3.zero;
    private Vector3 filteredAcceleration = Vector3.zero;

    private float filterFactor = 5f; // ���� ���� (���̸� ��������)
    private float threshold = 0.2f; // ���ӵ� ��ȭ �Ӱ谪 (���߸� ��������)
    private float stepCooldown = 0.2f; // �ּ� ���� ���� (���߸� ��������)
    private float lastStepTime = 0;

    public void Initialize()
    {
        startTime = DateTime.Now;

        UpdateStepCountUI();
    }

    public void UpdateTrackSteps()
    {
        // ���ӵ� ������ ��������
        Vector3 rawAcceleration = Input.acceleration;

        // Low-Pass Filter ����
        filteredAcceleration = Vector3.Lerp(filteredAcceleration, rawAcceleration, filterFactor);

        // ���ӵ� ��ȭ�� ���
        float accelerationChange = (filteredAcceleration - previousAcceleration).magnitude;

        // ���� �� ���
        if (accelerationChange > threshold && Time.time - lastStepTime > stepCooldown)
        {
            stepCount++;
            distance = stepCount * stepLength; // �Ÿ� ���
            lastStepTime = Time.time;

            UpdateStepCountUI();
        }

        // ���� ���ӵ� �� ������Ʈ
        previousAcceleration = filteredAcceleration;
    }

    public void UpdateStepCountUI()
    {
        if (stepCountText != null)
            stepCountText.text = $"{stepCount}";

        if (distanceText != null)
            distanceText.text = $"{distance:F2} m";

        if (timeText != null)
        {
            var elapsedTime = (int)(DateTime.Now - startTime).TotalSeconds;
            timeText.text = $"{elapsedTime}";
        }

        if (speedText != null)
        {
            var elapsedTime = (int)(DateTime.Now - startTime).TotalSeconds;
            float averageSpeed = elapsedTime > 0 ? distance / elapsedTime : 0;
            speedText.text = $"{averageSpeed} m/s";
        }
    }

    public void SetUpStepCounter(int stepCount, float distance)
    {
        this.stepCount = stepCount;
        this.distance = distance;

        UpdateStepCountUI();
    }

    public void UpdateDate()
    {
        if (DataManager.Instance.UserData.CurrentData.GetDate().Date == DateTime.Now.Date)
            return;

        startTime = DateTime.Now;
    }

    public (int stepCount, float distance, DateTime startTime) GetStepCounterData() => (stepCount, distance, startTime);
}