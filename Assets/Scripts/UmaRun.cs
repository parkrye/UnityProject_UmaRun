using System;

using UnityEngine;

public class UmaRun : MonoBehaviour
{
    [SerializeField] private StepCounterLogic stepCounterLogic;
    [SerializeField] private MusicPlayerLogic musicPlayerLogic;
    [SerializeField] private PalletteLogic palletteLogic;
    [SerializeField] private MenuLogic menuLogic;

    private bool isPaused = false;

    private void Start()
    {
        stepCounterLogic.Initialize();
        musicPlayerLogic.Initialize();
        palletteLogic.Initialize();
        menuLogic.Initialize();

        LoadState();
    }

    private void Update()
    {
        if (isPaused)
            return; // 백그라운드 상태에서는 Update를 멈춤

        stepCounterLogic.UpdateTrackSteps();
        musicPlayerLogic.UpdateAlbumState();
        menuLogic.UpdateDate();

        musicPlayerLogic.TurnningAlbumImage();

        NotificationLogic.ShowNotification($"UMA RUN NOW! {DateTime.Now}");
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
        var dateType = menuLogic.GetDateType();

        // 현재 상태 저장
        DataManager.UserData.CurrentData.StepCount = stepCount;
        DataManager.UserData.CurrentData.Distance = distance;
        DataManager.UserData.CurrentData.TimeSeconds = Time.time - startTime;
        DataManager.UserData.Playing = isPlaying;
        DataManager.UserData.Recycling = isRecycle;
        DataManager.UserData.Pallette = pallette;
        DataManager.UserData.DateType = dateType;

        DataManager.SaveData();
    }

    private void LoadState()
    {
        // 저장된 상태 불러오기
        var stepCount = DataManager.UserData.CurrentData.StepCount;
        var distance = DataManager.UserData.CurrentData.Distance;
        var startTime = DataManager.UserData.CurrentData.TimeSeconds;
        var isPlaying = DataManager.UserData.Playing;
        var isRecycle = DataManager.UserData.Recycling;
        var pallette = DataManager.UserData.Pallette;
        var dateType = DataManager.UserData.DateType;

        stepCounterLogic.SetUpStepCounter(stepCount, distance, startTime);
        musicPlayerLogic.SetUpMusicPlayer(isPlaying, isRecycle);
        palletteLogic.SetUpPallette(pallette);
        menuLogic.SetUpDateType(dateType);
    }
}
