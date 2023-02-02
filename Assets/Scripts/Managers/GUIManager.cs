using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;

    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _pauseScreen;

    [SerializeField] private PlayerInputController _playerInput;

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
        //SetDeathScreen(false);
        FindObjectOfType<Player>().PlayerHealthChangedEvent += UpdateHearts;
        _playerInput = FindObjectOfType<PlayerInputController>();
        SetPauseScreen(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetPauseScreen(!_isPaused);
        }
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
        _playerInput.IsActive = !isActive;
        Time.timeScale = isActive ? 0 : 1;
    }

    public void SetPauseScreen(bool isActive)
    {
        _pauseScreen.SetActive(isActive);
        _playerInput.IsActive = !isActive;
        Time.timeScale = isActive ? 0 : 1;
        _isPaused = isActive;

    }
}



