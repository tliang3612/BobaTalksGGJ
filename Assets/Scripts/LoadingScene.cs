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

    public float _progressBarSpeed = 2f;
    public float _loadCompleteDelay = 0.5f;
    public float _audioAmountToLower = 0.5f;

    private AsyncOperation _asyncOperation;
    private static string _sceneToLoad = "";
    private float _fillTarget = 0f;
    private string _loadingTextValue;
    private Image _progressBarImage;
    private Fader _fader;
    private AudioManager _audioManager;
    private float _savedAudioVolume;

    private void Start()
    {
        _fader = FindObjectOfType<Fader>();
        _progressBarImage = LoadingProgressBar.GetComponent<Image>();
        _loadingTextValue = LoadingText.text;
        StartCoroutine(LoadAsynchronously());

        _audioManager = GetComponent<AudioManager>();
        _savedAudioVolume = _audioManager.AudioSetting.GetTrackVolume(TrackType.Music);
        _audioManager.AudioSetting.SetTrackVolume(TrackType.Music, _savedAudioVolume * _audioAmountToLower);
    }

    private void Update()
    {
        Time.timeScale = 1f;
        _progressBarImage.fillAmount = Mathf.Lerp(_progressBarImage.fillAmount, _fillTarget, Time.deltaTime * _progressBarSpeed);
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

        while (_fillTarget -  _progressBarImage.fillAmount > 0.05f) 
        {
            yield return null;
        }
        yield return new WaitForSeconds(_loadCompleteDelay);

        _fader.FadeIn();
        yield return new WaitForSeconds(_fader.FadeDuration);

        _asyncOperation.allowSceneActivation = true;
        _audioManager.AudioSetting.SetTrackVolume(TrackType.Music, _savedAudioVolume);
        SceneManager.LoadScene(_sceneToLoad);
    }
    private void LoadingSetup()
    {
        _progressBarImage.fillAmount = 0f;
        LoadingText.text = _loadingTextValue;
    }
}
