using System;

using UnityEngine;
using UnityEngine.Events;

public class UmaRun : MonoBehaviour
{
    [Header("Sub Logics")]
    [SerializeField] private StepCounterLogic stepCounterLogic;
    [SerializeField] private MusicPlayerLogic musicPlayerLogic;
    [SerializeField] private PalletteLogic palletteLogic;
    [SerializeField] private MenuLogic menuLogic;

    [Space(), Header("UI Holders")]
    [SerializeField] private PopupUIHolder popupUIHolder;
    [SerializeField] private RecordUIHolder recordUIHolder;

    private bool isPaused = false;

    private float secondTick = 0f;
    private int tenSecondsTick = 0;

    private UnityEvent secondTickEvent = new UnityEvent();
    private UnityEvent tenSecondTickEvent = new UnityEvent();

    private void Start()
    {
        Initialize();
        SetListeners();

        LoadState();
    }

    private void Update()
    {
        if (isPaused)
            return; // 백그라운드 상태에서는 Update를 멈춤

        TimeCheck();

        stepCounterLogic.UpdateTrackSteps();
        musicPlayerLogic.UpdateAlbumState();

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

    private void Initialize()
    {
        stepCounterLogic.Initialize();
        musicPlayerLogic.Initialize();
        palletteLogic.Initialize();
        menuLogic.Initialize();

        recordUIHolder.Initialize();
    }

    private void SetListeners()
    {
        menuLogic.OnClcikedPalletteButtonListener = palletteLogic.OnClickedChangePallette;
        menuLogic.OnClickedResetButtonListener = ShowCommonSelectPopup;
        menuLogic.OnClickedQuitButtonListener = ShowCommonSelectPopup;
        menuLogic.OnClcikedRecordButtonListener = recordUIHolder.ShowRecordUI;

        secondTickEvent.AddListener(menuLogic.UpdateDate);
        tenSecondTickEvent.AddListener(stepCounterLogic.UpdateDate);
        tenSecondTickEvent.AddListener(SaveState);
    }

    private void TimeCheck()
    {
        secondTick += Time.deltaTime;

        if (secondTick > 1f)
        {
            secondTick = 0f;
            tenSecondsTick++;
            secondTickEvent?.Invoke();
        }

        if (tenSecondsTick > 10)
        {
            tenSecondsTick = 0;
            tenSecondTickEvent?.Invoke();
        }
    }

    private void SaveState()
    {
        (int stepCount, float distance, DateTime startTime) = stepCounterLogic.GetStepCounterData();
        (bool isPlaying, bool isRecycle) = musicPlayerLogic.GetMusicPlayerData();
        var pallette = palletteLogic.GetPalletteData();
        var dateType = menuLogic.GetDateType();

        DataManager.Instance.CheckDate();

        DataManager.Instance.UserData.CurrentData.StepCount = stepCount;
        DataManager.Instance.UserData.CurrentData.Distance = distance;
        DataManager.Instance.UserData.CurrentData.TimeSeconds = (int)(DateTime.Now - startTime).TotalSeconds;
        DataManager.Instance.UserData.IsPlaying = isPlaying;
        DataManager.Instance.UserData.IsRecycling = isRecycle;
        DataManager.Instance.UserData.PalletteIndex = pallette;
        DataManager.Instance.UserData.DateType = dateType;

        DataManager.Instance.SaveData();
    }

    private void LoadState()
    {
        DataManager.Instance.CheckDate();

        var stepCount = DataManager.Instance.UserData.CurrentData.StepCount;
        var distance = DataManager.Instance.UserData.CurrentData.Distance;
        var isPlaying = DataManager.Instance.UserData.IsPlaying;
        var isRecycle = DataManager.Instance.UserData.IsRecycling;
        var pallette = DataManager.Instance.UserData.PalletteIndex;
        var dateType = DataManager.Instance.UserData.DateType;

        stepCounterLogic.SetUpStepCounter(stepCount, distance);
        musicPlayerLogic.SetUpMusicPlayer(isPlaying, isRecycle);
        palletteLogic.SetUpPallette(pallette);
        menuLogic.SetUpDateType(dateType);
    }

    private void ShowCommonConfirmPopup(string title, string desc, Action onConfirmAction) => popupUIHolder.ShowCommonConfirmPopup(title, desc, onConfirmAction);

    private void ShowConfirmPopup(string title, string desc, string confirmText, Action onConfirmAction) => popupUIHolder.ShowConfirmPopup(title, desc, confirmText, onConfirmAction);

    private void ShowCommonSelectPopup(string title, string desc, Action onYesAction, Action onNoAction) => popupUIHolder.ShowCommonSelectPopup(title, desc, onYesAction, onNoAction);

    private void ShowSelectPopup(string title, string desc, string yes, string no, Action onYesAction, Action onNoAction) => popupUIHolder.ShowSelectPopup(title, desc, yes, no, onYesAction, onNoAction);
}