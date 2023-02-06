using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private SpriteRenderer _sprite;
    private Transform _playerTransform;

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _playerTransform = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        if ((_playerTransform.position.x - transform.position.x) < 0)
            _sprite.flipX = true;
        else
            _sprite.flipX = false;
    }
}
