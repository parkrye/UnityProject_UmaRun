using UnityEngine;

using TMPro;

public class StepCounterLogic : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI stepCountText = default;
    [SerializeField] private TextMeshProUGUI distanceText = default;
    [SerializeField] private TextMeshProUGUI timeText = default;
    [SerializeField] private TextMeshProUGUI speedText = default;

    // 걸음 수와 관련 변수
    private int stepCount = 0;
    private float stepLength = 0.75f; // 보폭 (미터)
    private float distance = 0f;
    private float startTime = 0f;

    // 가속도 데이터 관련 변수
    private Vector3 previousAcceleration = Vector3.zero;
    private Vector3 filteredAcceleration = Vector3.zero;

    private float filterFactor = 5f; // 필터 강도 (높이면 예민해집)
    private float threshold = 0.2f; // 가속도 변화 임계값 (낮추면 예민해짐)
    private float stepCooldown = 0.2f; // 최소 걸음 간격 (낮추면 예민해짐)
    private float lastStepTime = 0;

    public void Initialize()
    {
        UpdateStepCountUI();
    }

    public void UpdateTrackSteps()
    {
        // 가속도 데이터 가져오기
        Vector3 rawAcceleration = Input.acceleration;

        // Low-Pass Filter 적용
        filteredAcceleration = Vector3.Lerp(filteredAcceleration, rawAcceleration, filterFactor);

        // 가속도 변화량 계산
        float accelerationChange = (filteredAcceleration - previousAcceleration).magnitude;

        // 걸음 수 계산
        if (accelerationChange > threshold && Time.time - lastStepTime > stepCooldown)
        {
            stepCount++;
            distance = stepCount * stepLength; // 거리 계산
            lastStepTime = Time.time;

            UpdateStepCountUI();
        }

        // 이전 가속도 값 업데이트
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