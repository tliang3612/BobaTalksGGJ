using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IJumpable
{
    protected float _speed;
    protected Rigidbody2D _rb;

    [SerializeField] protected float _detectionRadius;
    [SerializeField] protected float _detectionXOffset;
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
            playerDamageHit.GetComponent<Player>().TakeDamage();
            Destroy(gameObject);
        }

        if (groundHit)
        {
            _rb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
    }

    public void FireProjectile(Vector2 directionToShoot, Vector2 startPos, float speed)
    {
        _rb = GetComponent<Rigidbody2D>();

        _speed = speed;
        _rb.velocity = directionToShoot * _speed;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + new Vector3(_detectionXOffset, 0, 0) , _detectionRadius);
    }
}