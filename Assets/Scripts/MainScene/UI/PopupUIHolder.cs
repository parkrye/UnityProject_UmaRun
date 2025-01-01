using UnityEngine;

using TMPro;
using System;

public class PopupUIHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText = default;
    [SerializeField] private TextMeshProUGUI descText = default;
    [SerializeField] private TextMeshProUGUI yesText = default;
    [SerializeField] private TextMeshProUGUI noText = default;
    [SerializeField] private GameObject yesButton = default;
    [SerializeField] private GameObject noButton = default;

    private Action OnClickedYesButtonListener = default;
    private Action OnClickedNoButtonListener = default;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowCommonConfirmPopup(string title, string desc, Action onConfirmAction)
    {
        ShowConfirmPopup(title, desc, "OK", onConfirmAction);
    }

    public void ShowConfirmPopup(string title, string desc, string confirmText, Action onConfirmAction)
    {
        titleText.text = title;
        descText.text = desc;
        yesText.text = confirmText;

        OnClickedYesButtonListener = onConfirmAction;
        OnClickedNoButtonListener = null;

        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(false);

        ShowUI();
    }

    public void ShowCommonSelectPopup(string title, string desc, Action onYesAction, Action onNoAction)
    {
        ShowSelectPopup(title, desc, "YES", "NO", onYesAction, onNoAction);
    }

    public void ShowSelectPopup(string title, string desc, string yes, string no, Action onYesAction, Action onNoAction)
    {
        titleText.text = title;
        descText.text = desc;
        yesText.text = yes;
        noText.text = no;

        OnClickedYesButtonListener = onYesAction;
        OnClickedNoButtonListener = onNoAction;

        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

        ShowUI();
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }

    public void OnClickedYesButton()
    {
        OnClickedYesButtonListener?.Invoke();
        
        CloseUI();
    }

    public void OnClickedNoButton()
    {
        OnClickedNoButtonListener?.Invoke();
        
        CloseUI();
    }
}
