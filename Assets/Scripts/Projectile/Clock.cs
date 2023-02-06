using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Clock : MonoBehaviour, IJumpable
{
    [SerializeField] private AudioClip _jumpedOnSound;
    [SerializeField] private float _fadeDuration;
    [SerializeField] private float _disappearDuration;

    private bool _isActive;
    private float _startTime;
    private AudioSource _audioSource;
    private Animator _anim;

    public bool CanBeJumpedOn
    {
        get { return _isActive; }
    }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _isActive = true;
    }

    private void Update()
    {
        if(Time.time > _startTime + _disappearDuration && !_isActive)
        {
            _isActive = true;
            GetComponent<SpriteRenderer>().DOFade(1, _fadeDuration);
        }
    }


    public void OnPlayerJumpedOn()
    {
        if (!_isActive)
            return;

        _isActive = false;
        FindObjectOfType<AudioManager>().PlaySound(_jumpedOnSound, _audioSource, TrackType.Sfx, false);
        StartCoroutine(PlayBreakAnimation());       
    }

    public IEnumerator PlayBreakAnimation()
    {
        _anim.SetTrigger("Break");
        _startTime = Time.time;
        yield return new WaitForSeconds(.6f);
        GetComponent<SpriteRenderer>().DOFade(0, _fadeDuration);
    }

}
