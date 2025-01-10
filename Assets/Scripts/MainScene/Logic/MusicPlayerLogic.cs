using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class MusicPlayerLogic : MonoBehaviour
{
    [SerializeField] private Image[] albumImages = default;

    [Space()]
    [SerializeField] private AudioSource audioSource = default;
    [SerializeField] private RectTransform albumImageRect = default;
    [SerializeField] private SliderEventHandler musicSlider = default;

    [Space()]
    [SerializeField] private Text songTitleText = default;
    [SerializeField] private Image playImage = default;
    [SerializeField] private Image recycleImag = default;

    [Space()]
    [SerializeField] private Sprite[] iconSprites = default;

    private Sprite[] sprites = default;
    private AudioClip[] audios = default;

    private IEnumerator playAudioRoutine = default;
    private IEnumerator controlSliderRoutine = default;
    private WaitWhile playAudioWait = default;
    private WaitWhile waitSliderControl = default;
    private WaitForSeconds waitForSeconds = default;

    private int currentImageIndex = default;
    private int currentAudioIndex = default;

    private float audioSpeed = default;

    private bool isPlaying = true;
    private bool isRecycle = true;
    private bool isSliderControl = false;

    public void Initialize()
    {
        sprites = Resources.LoadAll<Sprite>("Albums");
        audios = Resources.LoadAll<AudioClip>("Audios");

        playAudioWait = new WaitWhile(() => audioSource.isPlaying || isPlaying == false || isSliderControl);
        waitSliderControl = new WaitWhile(() => isSliderControl);
        waitForSeconds = new WaitForSeconds(1f);

        if (DataManager.Instance.UserData.MusicIndex >= 0)
        {
            currentAudioIndex = DataManager.Instance.UserData.MusicIndex;
            return;
        }

        currentAudioIndex = currentAudioIndex.GetRandomIndex(audios.Length);
        DataManager.Instance.UserData.MusicIndex = currentAudioIndex;
    }

    private IEnumerator PlayAudioClipRoutine()
    {
        musicSlider.OnPointerEvent = OnControlSlider;

        while (true)
        {
            if (isRecycle)
            {
                currentAudioIndex = currentAudioIndex.GetRandomIndex(audios.Length);
                DataManager.Instance.UserData.MusicIndex = currentAudioIndex;
            }

            currentImageIndex = currentImageIndex.GetRandomIndex(sprites.Length);

            var albumIMage = sprites[currentImageIndex];
            foreach (var image in albumImages)
            {
                image.sprite = albumIMage;
            }

            var audio = audios[currentAudioIndex];
            audioSource.clip = audio;
            audioSource.Play();

            UpdateMusicPlayerUI();

            yield return playAudioWait;
        }
    }

    public void TurnningAlbumImage()
    {
        if (audioSource.isPlaying == false || isPlaying == false || isSliderControl)
            return;

        albumImageRect.Rotate(Vector3.back, Time.deltaTime * audioSpeed);
    }

    public void UpdateAlbumState()
    {
        if (audioSource.isPlaying || isPlaying || isSliderControl == false)
            return;

        var progress = audioSource.time / audioSource.clip.length;
        musicSlider.SetSliderValue(progress);
    }

    public void UpdateMusicPlayerUI()
    {
        var audio = audios[currentAudioIndex];
        songTitleText.text = audio.name;
        audioSpeed = 240f / audio.length;

        if (isPlaying)
            playImage.sprite = iconSprites[0];
        else
            playImage.sprite = iconSprites[1];

        if (isRecycle)
            recycleImag.sprite = iconSprites[2];
        else
            recycleImag.sprite = iconSprites[3];
    }

    public void SetUpMusicPlayer(bool isPlaying, bool isRecycle)
    {
        this.isPlaying = isPlaying;
        this.isRecycle = isRecycle;

        UpdateMusicPlayerUI();

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
            if (playAudioRoutine == null)
                StartCoroutine(playAudioRoutine = PlayAudioClipRoutine());

            audioSource.UnPause();
            playImage.sprite = iconSprites[0];
        }
        else
        {
            audioSource.Pause();
            playImage.sprite = iconSprites[1];
        }
    }

    public void OnClickedRecycleButton()
    {
        isRecycle = isRecycle == false;

        if (isRecycle)
            recycleImag.sprite = iconSprites[2];
        else
            recycleImag.sprite = iconSprites[3];
    }

    public void OnClickedSkipButton()
    {
        if (playAudioRoutine == null)
        {
            isPlaying = true;
            isSliderControl = false;
            StartCoroutine(playAudioRoutine = PlayAudioClipRoutine());
            return;
        }

        audioSource.Stop();

        isPlaying = true;
        isSliderControl = false;
        currentAudioIndex = currentAudioIndex.GetRandomIndex(audios.Length);
        DataManager.Instance.UserData.MusicIndex = currentAudioIndex;
        UpdateMusicPlayerUI();

        StopCoroutine(playAudioRoutine);
        StartCoroutine(playAudioRoutine = PlayAudioClipRoutine());
    }

    private void OnControlSlider(bool inOn)
    {
        isSliderControl = inOn;

        if (isSliderControl && controlSliderRoutine == null)
            StartCoroutine(controlSliderRoutine = ControlSliderRoutine());
    }

    private IEnumerator ControlSliderRoutine()
    {
        if (isPlaying)
            audioSource.Pause();

        yield return waitSliderControl;
        musicSlider.SetInteractable(false);

        audioSource.time = musicSlider.SliderValue * audios[currentAudioIndex].length;

        if (isPlaying)
            audioSource.UnPause();
        else
            OnClickedPlayButton();

        yield return waitForSeconds;

        controlSliderRoutine = null;
        isSliderControl = false;
        musicSlider.SetInteractable(true);
    }

    public (bool isPlaying, bool isRecycle) GetMusicPlayerData()
    {
        return (isPlaying, isRecycle);
    }
}
