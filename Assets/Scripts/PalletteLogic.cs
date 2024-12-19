using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class PalletteLogic : MonoBehaviour
{
    [SerializeField] private Image[] images = default;
    [SerializeField] private TextMeshProUGUI[] texts = default;
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

    private void UpdatePallette()
    {
        foreach (var image in images)
        {
            var alpha = image.color.a;
            image.color = new Color(colors[palletIndex].r, colors[palletIndex].g, colors[palletIndex].b, alpha);
        }

        var textColor = Color.white - colors[palletIndex];
        textColor = new Color(textColor.r, textColor.g, textColor.b, 1f);
        foreach (var text in texts)
        {
            text.color = textColor;
        }
        songTitleText.color = textColor;
    }

    public void OnClickedChangePallette()
    {
        palletIndex++;
        if (palletIndex >= colors.Length)
            palletIndex = 0;
        UpdatePallette();
    }

    public int GetPalletteData()
    {
        return palletIndex;
    }
}
