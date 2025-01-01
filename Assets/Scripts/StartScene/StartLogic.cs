using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartLogic : MonoBehaviour
{
    [SerializeField] private GameObject titleText = default;
    [SerializeField] private AudioSource audioSource = default;

    private void Start()
    {
        titleText.gameObject.SetActive(false);

        // 백그라운드 실행 설정
        Application.runInBackground = true;

        DataManager.LoadData();

        NotificationLogic.StartNotificationSetting();
        NotificationLogic.ShowNotification("Start UMA RUN!");

        StartCoroutine(ReadyToPlay());
    }

    private IEnumerator ReadyToPlay()
    {
        var voices = Resources.LoadAll<AudioClip>("Voices");
        var randIndex = Random.Range(0, voices.Length);
        var randTarget = voices[randIndex];
        var wait = new WaitForSeconds(randTarget.length * 1.5f);

        yield return wait;
        audioSource.clip = voices[randIndex];
        audioSource.Play();

        titleText.gameObject.SetActive(true);

        yield return wait;
        titleText.gameObject.SetActive(false);

        yield return wait;
        SceneManager.LoadScene("MainScene");
    }
}