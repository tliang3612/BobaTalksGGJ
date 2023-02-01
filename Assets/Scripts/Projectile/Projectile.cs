using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IJumpable
{
    protected float _speed;
    protected float _duration;
    protected bool _isDurationBased;
    protected bool _isGroundBased;
    protected Vector2 _directionToShoot;
    protected Rigidbody2D _rb;

    protected float _startTime;

    [SerializeField] protected float _detectionRadius;
    [SerializeField] protected float _detectionXOffset;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _playerLayer;

    //public Action<Projectile> ProjectileDestroyedEvent;

    private void FixedUpdate()
    {
        HandleCollision();
    }
    public virtual void HandleCollision()
    {
        Collider2D playerDamageHit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);
        Collider2D groundHit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _groundLayer);

        if (playerDamageHit && playerDamageHit.GetComponent<Player>().CanInteractWithCollideables)
        {
            playerDamageHit.GetComponent<Player>().TakeDamage();
            Destroy(gameObject);
        }

        if(groundHit)
        {
            if(_isGroundBased)
                Destroy(gameObject);
        }

        if(_isDurationBased)
        {
            if(Time.time > _startTime + _duration)
            {
                Destroy(gameObject);
            }
        }
        
    }

    public void FireProjectile(Vector2 directionToShoot, float duration, float speed, bool isGroundBased)
    {
        _rb = GetComponent<Rigidbody2D>();

        _isDurationBased = duration > 0;
        _isGroundBased = isGroundBased;
        _duration = duration;
        _directionToShoot = directionToShoot;
        _speed = speed;
        _startTime = Time.time;

        _rb.velocity = directionToShoot * _speed;
        _rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(_detectionXOffset, 0, 0) , _detectionRadius);
    }
}