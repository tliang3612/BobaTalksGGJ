using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;

    [SerializeField] private GameObject _deathScreen;
    [SerializeField] private GameObject _pauseScreen;

    private void Awake()
    {
        
    }

    public void Start()
    {
        UpdateHearts(3);
        FindObjectOfType<Player>().PlayerHealthChangedEvent += UpdateHearts;
    }
    

    public void UpdateHearts(int hp)
    {
        Debug.Log("Health updated");

        for(int i=0; i < _hearts.Length; i++)
        {
            _hearts[i].sprite = _emptyHeartSprite;
        }

        for(int i=0; i < hp; i++)
        {
            _hearts[i].sprite = _fullHeartSprite;
        }

    }


    public void SetDeathScreen(bool isActive)
    {
        _deathScreen.SetActive(isActive);
    }

    public void SetPauseScreen(bool isActive)
    {

        _pauseScreen.SetActive(isActive);

    }
}
