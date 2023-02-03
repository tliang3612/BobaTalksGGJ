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
    public bool CanBeJumpedOn { get { return true; } }

    [SerializeField] protected float _detectionRadius;
    [SerializeField] protected LayerMask _groundLayer;
    [SerializeField] protected LayerMask _playerLayer;

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
            float direction = playerDamageHit.GetComponent<Player>().transform.position.x - transform.position.x;
            playerDamageHit.GetComponent<Player>().TakeDamage(direction > 0 ? 1 : -1);
            Destroy(gameObject);
        }
        else if(playerDamageHit && !playerDamageHit.GetComponent<Player>().CanInteractWithCollideables)
        {
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
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position , _detectionRadius);
    }

    public void OnPlayerJumpedOn()
    {
        
    }
}