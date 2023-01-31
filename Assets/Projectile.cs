using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IJumpable, IKnockbackable
{
    //private AttackDetails attackDetails;
    private float _speed;

    private Rigidbody2D _rb;

    [SerializeField] private float _damageRadius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _playerLayer;

    private void FixedUpdate()
    {
        Collider2D playerDamageHit = Physics2D.OverlapCircle(transform.position, _damageRadius, _playerLayer);
        Collider2D groundHit = Physics2D.OverlapCircle(transform.position, _damageRadius, _groundLayer);

        if (playerDamageHit)
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }

    public void Knockback(Vector2 angle, float strength, int direction)
    {
        
    }
}