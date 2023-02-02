using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _friendsNeeded;

    private Collider2D _collider;
    private Rigidbody2D _rb;
    private bool _isMoving;
    private int _friendsDetected;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        
    }

    public void Start()
    {
        
    }

   

}
