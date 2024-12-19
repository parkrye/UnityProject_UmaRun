using UnityEngine;

public class UmaRun : MonoBehaviour
{
    private const string StepCount = "StepCount";
    private const string Distance = "Distance";
    private const string StartTime = "StartTime";
    private const string Playing = "Playing";
    private const string Recycling = "Recycling";
    private const string Pallette = "Pallete";

    [SerializeField] private StepCounterLogic stepCounterLogic;
    [SerializeField] private MusicPlayerLogic musicPlayerLogic;
    [SerializeField] private PalletteLogic palletteLogic;

    private bool isPaused = false;

    private void Awake()
    {
        // 백그라운드 실행 설정
        Application.runInBackground = true;

        stepCounterLogic.Initialize();
        musicPlayerLogic.Initialize();
        palletteLogic.Initialize();

        LoadState();
    }

    private void Update()
    {
        if (isPaused)
            return; // 백그라운드 상태에서는 Update를 멈춤

        stepCounterLogic.UpdateTrackSteps();
        musicPlayerLogic.UpdateAlbumState();
    }

    private void LateUpdate()
    {
        musicPlayerLogic.TurnningAlbumImage();
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

    private void SaveState()
    {
        (int stepCount, float distance, float startTime) = stepCounterLogic.GetStepCounterData();
        (bool isPlaying, bool isRecycle) = musicPlayerLogic.GetMusicPlayerData();
        var pallette = palletteLogic.GetPalletteData();

        // 현재 상태 저장
        PlayerPrefs.SetInt(StepCount, stepCount);
        PlayerPrefs.SetFloat(Distance, distance);
        PlayerPrefs.SetFloat(StartTime, Time.time - startTime);
        PlayerPrefs.SetInt(Playing, isPlaying ? 0 : 0);
        PlayerPrefs.SetInt(Recycling, isRecycle ? 0 : 0);
        PlayerPrefs.SetInt(Pallette, pallette);
        PlayerPrefs.Save();
    }

    private void LoadState()
    {
        // 저장된 상태 불러오기
        var stepCount = PlayerPrefs.GetInt(StepCount, 0);
        var distance = PlayerPrefs.GetFloat(Distance, 0f);
        var startTime = Time.time - PlayerPrefs.GetFloat(StartTime, 0f);
        var isPlaying = PlayerPrefs.GetInt(Playing) == 0 ? true : false;
        var isRecycle = PlayerPrefs.GetInt(Recycling) == 0 ? true : false;
        var pallette = PlayerPrefs.GetInt(Pallette, 0);

        stepCounterLogic.SetUpStepCounter(stepCount, distance, startTime);
        musicPlayerLogic.SetUpMusicPlayer(isPlaying, isRecycle);
        palletteLogic.SetUpPallette(pallette);
    }
}
