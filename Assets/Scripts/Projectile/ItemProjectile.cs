using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProjectile : Projectile
{
    public Action ItemCollectedEvent;

    private void Start()
    {
        ItemCollectedEvent += FindObjectOfType<CatchGameManager>().OnItemCollected;
    }

    public override void HandleCollision()
    {
        Collider2D playerHit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _playerLayer);
        Collider2D groundHit = Physics2D.OverlapCircle(transform.position, _detectionRadius, _groundLayer);

        if(groundHit)
        {
            _rb.velocity = Vector2.zero;
            Destroy(gameObject);
        }
        else if(playerHit)
        {
            ItemCollectedEvent?.Invoke();
            Destroy(gameObject);
        }
            
    }
}