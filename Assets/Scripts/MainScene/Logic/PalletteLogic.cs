using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PalletteLogic : MonoBehaviour
{
    [SerializeField] private Image[] images = default;
    [SerializeField] private TextMeshProUGUI[] texts = default;
    [SerializeField] private Image[] rImages = default;
    [SerializeField] private TextMeshProUGUI[] rTexts = default;
    [SerializeField] private Text songTitleText = default;

    [SerializeField] private Color[] colors = default;

    private int palletIndex = 0;

    public void Initialize()
    {
        colors = new Color[] 
        {
            Color.black,
            Color.white,
            Color.red,
            Color.blue,
            Color.yellow,
            Color.green,
            Color.magenta,
            Color.cyan,
        };
    }

    public void SetUpPallette(int palletIndex)
    {
        this.palletIndex = palletIndex;

        UpdatePallette();
    }

    public void OnClickedChangePallette()
    {
        palletIndex++;
        if (palletIndex >= colors.Length)
            palletIndex = 0;
        UpdatePallette();
    }

    public int GetPalletteData() => palletIndex;

    private void UpdatePallette()
    {
        var color = colors[palletIndex];
        foreach (var image in images)
        {
            var alpha = image.color.a;
            image.color = new Color(color.r, color.g, color.b, alpha);
        }

        foreach (var rText in rTexts)
        {
            rText.color = colors[palletIndex];
        }

        var reverseColor = Color.white - color;
        reverseColor.a = 1f;
        foreach (var text in texts)
        {
            text.color = reverseColor;
        }

        foreach (var rImage in rImages)
        {
            rImage.color = reverseColor;
        }

        songTitleText.color = reverseColor;
    }
}
