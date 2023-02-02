using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFriend : MonoBehaviour, IJumpable
{
    [SerializeField] private AudioClip _onPlayerJumpSound;
    [SerializeField] private float _offsetY;
    [SerializeField] private float _detectionRadius;
    [SerializeField] private LayerMask _playerLayer;

    private float _audioDelay = 0.2f;
    private float _startTime;

    private void FixedUpdate()
    {
        if (Time.time < _startTime + _audioDelay)
            return;

        var hit = Physics2D.OverlapCircle(new Vector3(transform.position.x, transform.position.y + _offsetY, transform.position.z),
            _detectionRadius, _playerLayer);

        if(hit)
        {
            _startTime = Time.time;
            FindObjectOfType<AudioManager>().PlaySound(_onPlayerJumpSound, GetComponent<AudioSource>(), TrackType.Sfx, false);           
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + _offsetY, transform.position.z),
            _detectionRadius);

    }
}
