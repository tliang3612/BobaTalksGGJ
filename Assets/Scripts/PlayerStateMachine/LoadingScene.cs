using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public static string LoadingScreenSceneName = "Loading Scene";

    public TextMeshProUGUI LoadingText;

    public Image LoadingProgressBar;

    public float ProgressBarSpeed = 2f;
    public float LoadCompleteDelay = 0.5f;

    private AsyncOperation _asyncOperation;
    private static string _sceneToLoad = "";
    private float _fillTarget = 0f;
    private string _loadingTextValue;
    private Image _progressBarImage;
    private Fader _fader;   

    private void Start()
    {
        _fader = FindObjectOfType<Fader>();
        _progressBarImage = LoadingProgressBar.GetComponent<Image>();
        _loadingTextValue = LoadingText.text;
        StartCoroutine(LoadAsynchronously());
    }

    private void Update()
    {
        Time.timeScale = 1f;
        _progressBarImage.fillAmount = Mathf.MoveTowards(_progressBarImage.fillAmount, _fillTarget, Time.deltaTime * ProgressBarSpeed);
    }

    public static void LoadScene(string sceneToLoad)
    {
        _sceneToLoad = sceneToLoad;
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (LoadingScreenSceneName != null)
        {
            SceneManager.LoadScene(LoadingScreenSceneName);
        }

    }
    private IEnumerator LoadAsynchronously()
    {
        LoadingSetup();

        _fader.FadeOut();
        yield return new WaitForSeconds(_fader.FadeDuration);

        _asyncOperation = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Single);
        _asyncOperation.allowSceneActivation = false;

        while (_asyncOperation.progress < 0.9f)
        {
            _fillTarget = _asyncOperation.progress;
            yield return null;
        }

        _fillTarget = 1f;

        while (_progressBarImage.fillAmount != _fillTarget)
        {
            yield return null;
        }
        yield return new WaitForSeconds(LoadCompleteDelay);

        _fader.FadeIn();
        yield return new WaitForSeconds(_fader.FadeDuration);

        _asyncOperation.allowSceneActivation = true;
        SceneManager.LoadScene(_sceneToLoad);
    }
    private void LoadingSetup()
    {
        _progressBarImage.fillAmount = 0f;
        LoadingText.text = _loadingTextValue;
    }
}
