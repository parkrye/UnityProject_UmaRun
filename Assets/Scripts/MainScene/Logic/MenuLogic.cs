using System;

using UnityEngine;

using TMPro;

public class MenuLogic : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel = default;
    [SerializeField] private RectTransform menuCancelRect = default;
    [SerializeField] private TextMeshProUGUI dateText = default;

    private int dateType = 0;
    private float secondTick = 0f;
    private int minuteTick = 0;

    public Action OnClcikedPalletteButtonListener { get; set; }
    public Action<string, string, Action, Action> OnClickedResetButtonListener { get; set; }
    public Action<string, string, Action, Action> OnClickedQuitButtonListener { get; set; }

    public void Initialize()
    {
        menuCancelRect.anchoredPosition = new Vector2(Screen.width - Screen.safeArea.xMax, Screen.height - Screen.safeArea.yMax);
        menuCancelRect.sizeDelta = new Vector2(Screen.width, Screen.height);

        HideMenuPanel();
    }

    public void SetUpDateType(int dateType)
    {
        this.dateType = dateType;
    }

    public int GetDateType()
    {
        return dateType;
    }

    public void UpdateDate()
    {
        secondTick += Time.deltaTime;

        if (secondTick > 1f)
        {
            var current = DateTime.Now;
            var dateText = dateType == 0
                ? current.ToString("yyyy-MM-dd ddd tt hh:mm:ss")
                : current.ToString("yyyy-MM-dd ddd HH:mm:ss");
            this.dateText.text = dateText;

            secondTick = 0f;
            minuteTick++;
        }

        if (minuteTick > 10)
        {
            DataManager.Instance.CheckDate();
            minuteTick = 0;
        }
    }

    private void ChangeDateType()
    {
        dateType = 1 - dateType;
    }

    private void ShowMenuPanel()
    {
        menuPanel.gameObject.SetActive(true);
    }

    private void HideMenuPanel()
    {
        menuPanel.gameObject.SetActive(false);
    }

    private void QuitApplication()
    {
        Application.Quit();
    }

    public void OnClickedDateButton()
    {
        ChangeDateType();
    }

    public void OnClickedMenuButton()
    {
        ShowMenuPanel();
    }

    public void OnClickedCloseMenuButton()
    {
        HideMenuPanel();
    }

    public void OnClickedPalletteButton()
    {
        OnClcikedPalletteButtonListener?.Invoke();
    }

    public void OnClickedRecordButton()
    {

    }

    public void OnClickedResetButton()
    {
        OnClickedResetButtonListener?.Invoke(
            "Reset Data",
            "Are You Sure Reset Data?", 
            DataManager.Instance.ResetData, 
            null);
    }

    public void OnClickedQuitButton()
    {
        OnClickedQuitButtonListener?.Invoke(
            "Quit App",
            "Are You Sure Quit the UmaRun?",
            QuitApplication,
            null);
    }
}
