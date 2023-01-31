using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    private SpriteRenderer _sprite;

    public Action<int> PianoKeyPressedEvent;
    private int _keyNum;

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

    public void SetKeyNum(int keyNum)
    {
        _keyNum = keyNum;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            PianoKeyPressedEvent?.Invoke(_keyNum);
            _sprite.color = Color.blue;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        _sprite.color = Color.white;
    }
}
