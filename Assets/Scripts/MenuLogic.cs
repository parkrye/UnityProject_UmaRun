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

    public void Initialize()
    {
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
            DataManager.CheckDate();
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

    public void OnClickedRecordButton()
    {

    }

    public void OnClickedResetButton()
    {

    }

    public void OnClickedQuitButton()
    {
        QuitApplication();
    }
}
