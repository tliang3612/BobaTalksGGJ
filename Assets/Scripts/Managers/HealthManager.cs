using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private Image[] _hearts;
    [SerializeField] private Sprite _fullHeartSprite;
    [SerializeField] private Sprite _emptyHeartSprite;

    private void Awake()
    {
        FindObjectOfType<Player>().PlayerHealthChangedEvent += UpdateHearts;
    }

    public void Start()
    {
        UpdateHearts(3);
    }

    public void UpdateHearts(int hp)
    {
        Debug.Log("Health updated");

        for(int i=0; i < _hearts.Length; i++)
        {
            _hearts[i].color = Color.gray;
            _hearts[i].sprite = _emptyHeartSprite;
        }

        for(int i=0; i < hp; i++)
        {
            _hearts[i].color = Color.red;
            _hearts[i].sprite = _fullHeartSprite;
        }

    }
    
}
