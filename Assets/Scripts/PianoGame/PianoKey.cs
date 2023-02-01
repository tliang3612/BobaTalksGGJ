using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    private SpriteRenderer _sprite;

    public Action<int, AudioClip> PianoKeyPressedEvent;
    private int _keyNum;
    private AudioClip _audioClip;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        
    }

    /*private void OnMouseDown()
    {
        PianoKeyPressedEvent?.Invoke(_keyNum);
        Debug.Log(_keyNum);

        _sprite.color = Color.blue;
    }

    private void OnMouseUpAsButton()
    {
        _sprite.color = Color.white;
    }

    private void OnMouseEnter()
    {
        _sprite.color = Color.gray;
    }

    private void OnMouseExit()
    {
        _sprite.color = Color.white;
    }*/

    public void SetKey(int keyNum, AudioClip clip)
    {
        _keyNum = keyNum;
        _audioClip = clip;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null && collision.GetComponent<Player>().CanInteractWithCollideables)
        {
            PianoKeyPressedEvent?.Invoke(_keyNum, _audioClip);
            _sprite.color = Color.blue;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        _sprite.color = Color.white;
    }
}
