using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeFriend : MonoBehaviour, IJumpable
{
    [SerializeField] private AudioClip _jumpedOnSound;
    [SerializeField] private LayerMask _playerLayer;

    private AudioSource _audioSource;

    public bool CanBeJumpedOn { get { return true; } }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public AudioClip SoundToPlay
    {
        get { return _jumpedOnSound; }
    }

    

    public void OnPlayerJumpedOn()
    {
        FindObjectOfType<AudioManager>().PlaySound(_jumpedOnSound, _audioSource, TrackType.Sfx, false);
    }
}
