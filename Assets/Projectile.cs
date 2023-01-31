using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //private AttackDetails attackDetails;

    private float _speed;
    private float _startPosX;
    private float _startPosY;

    private bool _hasHitGround;
    private Rigidbody2D _rb;

    [SerializeField] private float _damageRadius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _playerLayer;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();       
    }

    private void FixedUpdate()
    {
        if (!_hasHitGround)
        {
            Collider2D playerHit = Physics2D.OverlapCircle(transform.position, _damageRadius, _playerLayer);
            Collider2D groundHit = Physics2D.OverlapCircle(transform.position, _damageRadius, _groundLayer);

            if (playerHit)
            {
                //damageHit.transform.SendMessage("Damage", attackDetails);
                Destroy(gameObject);
            }

            if (groundHit)
            {
                _hasHitGround = true;
                _rb.velocity = Vector2.zero;
            }
        }
    }
    public void FireProjectile(Vector2 directionToShoot, Vector2 startPos, float speed)
    {
        _speed = speed;

        _rb.velocity = directionToShoot * _speed;

        _startPosX = startPos.x;
        _startPosY = startPos.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _damageRadius);
    }
}