using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("NEEDS A START AND END POINT")]
    [Tooltip("NEEDS A START AND END POINT")]
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _friendsNeeded;

    private int _friendsDetected;
    private bool _playerDetected;

    private void Awake()
    {
        _friendsDetected = 0;
    }

    private void FixedUpdate()
    {
        if (_friendsDetected == _friendsNeeded && _playerDetected)
            HandleMove(true);
        else if(_friendsDetected == 0)
            HandleMove(false);
    }

    private void HandleMove(bool isUp)
    {
        if (isUp)
            transform.position = Vector2.MoveTowards(transform.position, _endPoint.position, Time.deltaTime * _moveSpeed);
        else
            transform.position = Vector2.MoveTowards(transform.position, _startPoint.position, Time.deltaTime * _moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<RealFriend>() != null)
            _friendsDetected++;

        if (collision.GetComponent<Player>() != null)
            _playerDetected = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<RealFriend>() != null)
            _friendsDetected--;

        if (collision.GetComponent<Player>() != null)
            _playerDetected = false;
    }


}
