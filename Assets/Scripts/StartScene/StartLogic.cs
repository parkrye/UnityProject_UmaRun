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

        DataManager.Instance.LoadData();

        StartCoroutine(ReadyToPlay());
    }

    private IEnumerator ReadyToPlay()
    {
        var voices = Resources.LoadAll<AudioClip>("Voices");
        var randIndex = Random.Range(0, voices.Length);
        var randTarget = voices[randIndex];

        yield return new WaitForSeconds(randTarget.length * 0.5f); ;
        audioSource.clip = voices[randIndex];
        audioSource.Play();

        titleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(randTarget.length * 2f); ;
        titleText.gameObject.SetActive(false);

        yield return new WaitForSeconds(randTarget.length); ;
        SceneManager.LoadScene("MainScene");
    }
}