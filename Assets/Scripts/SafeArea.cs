using UnityEngine;
using UnityEngine.UI;

public class SafeArea : MonoBehaviour
{
    [SerializeField] private Canvas canvas = default;
    [SerializeField] private RectTransform[] topCovers = default;
    [SerializeField] private RectTransform[] botCovers = default;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;

        // Viewport를 기준으로 Safe Area를 비율로 변환
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        if (rectTransform != null)
        {
            // RectTransform에 적용
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        foreach (var top in topCovers)
        {
            top.sizeDelta = Vector2.up * (Screen.height - safeArea.yMax) / canvas.scaleFactor;
        }

        foreach (var bot in botCovers)
        {
            bot.sizeDelta = Vector2.up * safeArea.position / canvas.scaleFactor;
        }
    }
}
