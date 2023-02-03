using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;
using TMPro;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;

    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _pauseScreen;

    [SerializeField] private GameObject _tempDialogue;
    [SerializeField] private float _tempDialogueDuration;

    private PlayerInputController _playerInput;

    private bool _isPaused;

    private void Awake()
    {
        if (_pauseScreen == null)
        {
            Debug.Log(true);
            _pauseScreen = GameObject.Find("PauseScreen");
        }

        if (_hearts.Length <= 0)
        {
            for (int i = 0; i < 3; i++)
            {
                _hearts[i] = GameObject.Find("HealthPanel").GetComponentsInChildren<Image>()[i];
            }
        }
    }

    public void Start()
    {
        UpdateHearts(3);
        if (FindObjectOfType<Player>() != null)
        {
            FindObjectOfType<Player>().PlayerHealthChangedEvent += UpdateHearts;
            _playerInput = FindObjectOfType<PlayerInputController>();
        }

        if (_tempDialogue != null)
        {
            var images = _tempDialogue.GetComponentsInChildren<Image>();
            var text = _tempDialogue.GetComponentInChildren<Text>();
            foreach (var image in images)
                image.color = new Color(255,255,255, 0);
            text.color = new Color(255, 255, 255, 0);
            StartTempDialogue();
        }
            

        SetPauseScreen(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SetPauseScreen(!_isPaused);
        }
    }

    public void StartTempDialogue()
    {
        var images = _tempDialogue.GetComponentsInChildren<Image>();
        var text = _tempDialogue.GetComponentInChildren<Text>();
        foreach (var image in images)
            image.DOFade(1, 2);
        text.DOFade(1, 2);        
    }

    public void EndTempDialogue()
    {
        var images = _tempDialogue.GetComponentsInChildren<Image>();
        var text = _tempDialogue.GetComponentInChildren<Text>();

        foreach (var image in images)
            image.DOFade(0, 2);
        text.DOFade(0, 2);
    }

    public void UpdateHearts(int hp)
    {
        Debug.Log("Health updated");

        for (int i = 0; i < _hearts.Length; i++)
        {
            _hearts[i].sprite = _emptyHeartSprite;
        }

        for (int i = 0; i < hp; i++)
        {
            _hearts[i].sprite = _fullHeartSprite;
        }

    }


    public void SetDeathScreen(bool isActive)
    {
        _deathScreen.SetActive(isActive);
        Time.timeScale = isActive ? 0 : 1;
    }

    public void SetPauseScreen(bool isActive)
    {
        _pauseScreen.SetActive(isActive);
        if(_playerInput)
            _playerInput.IsActive = !isActive;

        AudioListener.pause = isActive;
        Time.timeScale = isActive ? 0 : 1;
        _isPaused = isActive;

    }
}



