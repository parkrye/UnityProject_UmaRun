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
    private float startTime = 0f;

    // ���ӵ� ������ ���� ����
    private Vector3 previousAcceleration = Vector3.zero;
    private Vector3 filteredAcceleration = Vector3.zero;

    private float filterFactor = 5f; // ���� ���� (���̸� ��������)
    private float threshold = 0.2f; // ���ӵ� ��ȭ �Ӱ谪 (���߸� ��������)
    private float stepCooldown = 0.2f; // �ּ� ���� ���� (���߸� ��������)
    private float lastStepTime = 0;

    public void Initialize()
    {
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
        {
            stepCountText.text = $"{stepCount}";
        }

        if (distanceText != null)
        {
            distanceText.text = $"{distance:F2} m";
        }

        if (timeText != null)
        {
            float elapsedTime = Time.time - startTime;
            timeText.text = $"{elapsedTime.FormatTime()}";
        }

        if (speedText != null)
        {
            float elapsedTime = Time.time - startTime;
            float averageSpeed = elapsedTime > 0 ? distance / elapsedTime : 0;
            speedText.text = $"{averageSpeed:F2} m/s";
        }
    }

    public void SetUpStepCounter(int stepCount, float distance, float startTime)
    {
        this.stepCount = stepCount;
        this.distance = distance;
        this.startTime = startTime;

        UpdateStepCountUI();
    }

    public (int stepCount, float distance, float startTime) GetStepCounterData()
    {
        return (stepCount, distance, startTime);
    }
}