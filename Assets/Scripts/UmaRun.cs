using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UmaRun : MonoBehaviour
{
    #region Inspector
    [Header("Images")]
    [SerializeField] private Image[] images = default;

    [Space(), Header("Plays")]
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private RectTransform albumRect = default;
    [SerializeField] private Slider albumSlider = default;

    [Header("Textx")]
    [SerializeField] private Text songTitleText = default;
    [SerializeField] private TextMeshProUGUI playText = default;
    [SerializeField] private TextMeshProUGUI recycleText = default;
    [SerializeField] private TextMeshProUGUI skipText = default;

    [Space(), Header("Steps")]
    [Header("Textx")]
    [SerializeField] private TextMeshProUGUI stepCountText = default;
    [SerializeField] private TextMeshProUGUI distanceText = default;
    [SerializeField] private TextMeshProUGUI timeText = default;
    [SerializeField] private TextMeshProUGUI speedText = default;
    #endregion

    #region PlayerPrefs
    private const string StepCount = "StepCount";
    private const string Distance = "Distance";
    private const string StartTime = "StartTime";
    private const string Playing = "Playing";
    private const string Recycling = "Recycling";
    #endregion

    #region Parameter
    private Sprite[] sprites = default;
    private AudioClip[] audios = default;

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

    private bool isPaused = false;

    private IEnumerator playAudioRoutine = default;
    private WaitWhile playAudioWait = default;

    private int currentImageIndex = default;
    private int currentAudioIndex = default;

    private float audioSpeed = default;

    private bool isPlaying = true;
    private bool isRecycle = true;
    #endregion

    #region Unity Routine
    private void Awake()
    {
        // 백그라운드 실행 설정
        Application.runInBackground = true;

        sprites = Resources.LoadAll<Sprite>("Albums");
        audios = Resources.LoadAll<AudioClip>("Audios");

        playAudioWait = new WaitWhile(() => audioSource.isPlaying || isPlaying == false);
    }

    private void Start()
    {
        // 시작 시간 설정
        startTime = Time.time;

        // UI 초기화
        UpdateStepCountUI();

        StartCoroutine(playAudioRoutine = PlayAudioClipRoutine());
    }

    private void Update()
    {
        if (isPaused)
            return; // 백그라운드 상태에서는 Update를 멈춤

        UpdateTrackSteps();
        UpdateAlbumState();
    }

    private void LateUpdate()
    {
        TurnningAlbumImage();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;

        if (pauseStatus)
            SaveState();
        else
            LoadState();
    }

    private void OnApplicationQuit()
    {
        SaveState();
    }
    #endregion

    #region StepCounter
    void UpdateTrackSteps()
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

    void UpdateStepCountUI()
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
            timeText.text = $"{FormatTime(elapsedTime)}";
        }

        if (speedText != null)
        {
            float elapsedTime = Time.time - startTime;
            float averageSpeed = elapsedTime > 0 ? distance / elapsedTime : 0;
            speedText.text = $"{averageSpeed:F2} m/s";
        }
    }

    string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return $"{minutes:00}:{seconds:00}";
    }
    #endregion

    #region Album
    private IEnumerator PlayAudioClipRoutine()
    {
        while (true)
        {
            if (isRecycle)
                currentAudioIndex = GetRandomIndex(currentAudioIndex, audios.Length);
            currentImageIndex = GetRandomIndex(currentImageIndex, sprites.Length);

            var albumIMage = sprites[currentImageIndex];
            foreach (var image in images)
            {
                image.sprite = albumIMage;
            }

            var audio = audios[currentAudioIndex];

            audioSource.clip = audio;
            audioSource.Play();
            
            UpdateAlbumUI();

            yield return playAudioWait;
        }
    }

    private void TurnningAlbumImage()
    {
        albumRect.Rotate(Vector3.back, Time.deltaTime * audioSpeed);
    }

    private void UpdateAlbumState()
    {
        var progress = audioSource.time / audioSource.clip.length;
        albumSlider.value = progress;
    }

    private void UpdateAlbumUI()
    {
        var audio = audios[currentAudioIndex];
        songTitleText.text = audio.name;
        audioSpeed = 240f / audio.length;

        if (isPlaying)
            playText.text = "Playing";
        else
            playText.text = "Paused";

        if (isRecycle)
            recycleText.text = "Recycling";
        else
            recycleText.text = "Repeating";
    }

    private void SetupAlbum()
    {
        if (playAudioRoutine != null)
        {
            if (isPlaying && audioSource.isPlaying == false)
                audioSource.Play();
            return;
        }

        if (isPlaying && audioSource.isPlaying == false)
        {
            StartCoroutine(playAudioRoutine = PlayAudioClipRoutine());
            return;
        }
    }

    public void OnClickedPlayButton()
    {
        isPlaying = isPlaying == false;

        if (isPlaying)
        {
            audioSource.UnPause();
            playText.text = "Playing";
        }
        else
        {
            audioSource.Pause();
            playText.text = "Paused";
        }
    }

    public void OnClickedRecycleButton()
    {
        isRecycle = isRecycle == false;

        if (isRecycle)
            recycleText.text = "Recycling";
        else
            recycleText.text = "Repeating";
    }

    public void OnClickedSkipButton()
    {
        if (playAudioRoutine == null)
            return;
        audioSource.Stop();

        isPlaying = true;
        currentAudioIndex = GetRandomIndex(currentAudioIndex, audios.Length);
        UpdateAlbumUI();

        StopCoroutine(playAudioRoutine);
        StartCoroutine(playAudioRoutine = PlayAudioClipRoutine());
    }
    #endregion

    #region Data
    private void SaveState()
    {
        // 현재 상태 저장
        PlayerPrefs.SetInt(StepCount, stepCount);
        PlayerPrefs.SetFloat(Distance, distance);
        PlayerPrefs.SetFloat(StartTime, Time.time - startTime);
        PlayerPrefs.SetInt(Playing, isPlaying ? 0 : 0);
        PlayerPrefs.SetInt(Recycling, isRecycle ? 0 : 0);
        PlayerPrefs.Save();

        audioSource.Pause();
    }

    private void LoadState()
    {
        // 저장된 상태 불러오기
        stepCount = PlayerPrefs.GetInt(StepCount, 0);
        distance = PlayerPrefs.GetFloat(Distance, 0f);
        startTime = Time.time - PlayerPrefs.GetFloat(StartTime, 0f);
        isPlaying = PlayerPrefs.GetInt(Playing) == 0 ? true : false;
        isRecycle = PlayerPrefs.GetInt(Recycling) == 0 ? true : false;

        SetupAlbum();

        UpdateStepCountUI();
        UpdateAlbumUI();
    }
    #endregion

    #region Util
    private int GetRandomIndex(int currentIndex, int range)
    {
        int returnValue;

        do
            returnValue = Random.Range(0, range);
        while (currentIndex == returnValue);

        return returnValue;
    }
    #endregion
}
