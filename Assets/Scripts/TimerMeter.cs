using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TimerMeter : MonoBehaviour
{
    [SerializeField] private GameObject _meterGameObject;    
    [SerializeField] private GameObject _timerClock;
    [SerializeField] private Transform _teleportPosition;

    [SerializeField] private float _maxTime;
    [SerializeField] private float _startDelay;   
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private float _detectionRadius;

    [SerializeField] private AudioClip _clockTickAudio;
    [SerializeField] private AudioClip _clockTouchAudio;

    private Image _meter;
    private bool _triggeredByClock;
    private bool _clockTimerRunning;
    
    private float _currentTime;
    private float _startTime;

    private AudioSource _audioSource;
    


    private void Start()
    {
        _triggeredByClock = _timerClock != null;
        _meter = _meterGameObject.GetComponent<Image>();
        _meter.fillAmount = 1;
        _currentTime = _maxTime;
        _startTime = Time.time;
        _audioSource = GetComponent<AudioSource>();

        if(_triggeredByClock)
            _meterGameObject.SetActive(false);
    }

    private void Update()
    {
        if (_triggeredByClock)
        {
            HandleTimerOnClock();
        }
        else
        {
            HandleTimerOnStart();
        }                 
    }

    private void HandleTimerOnStart()
    {
        if (Time.time < _startTime + _startDelay)
            return;

        _currentTime -= Time.deltaTime;
        var newFill = 1 / _maxTime * _currentTime;
        _meter.fillAmount = Mathf.MoveTowards(_meter.fillAmount, newFill, Time.deltaTime);
        FindObjectOfType<AudioManager>().PlaySound(_clockTickAudio, _audioSource, TrackType.Music, true);

        if (_currentTime < 0)
        {
            _startTime = Time.time * 10;
            StartCoroutine(FindObjectOfType<LevelManager>().Respawn(FindObjectOfType<Player>()));
        }
    }

    private void HandleTimerOnClock()
    {
        HandleOnPlayerDetected();
        if(_clockTimerRunning)
        {
            _meterGameObject.SetActive(true);
            _currentTime -= Time.deltaTime;
            var newFill = 1 / _maxTime * _currentTime;
            _meter.fillAmount = Mathf.MoveTowards(_meter.fillAmount, newFill, Time.deltaTime);
            FindObjectOfType<AudioManager>().PlaySound(_clockTickAudio, _audioSource, TrackType.Music, true);

            if (_currentTime < 0)
            {
                _clockTimerRunning = false;
                _timerClock.GetComponent<SpriteRenderer>().DOFade(1, 1);
                FindObjectOfType<AudioManager>().StopSound(_audioSource);
                FindObjectOfType<LevelManager>().SpawnPlayerAtPoint(_teleportPosition.position);
                _meterGameObject.SetActive(false);
            }
        }
        else
        {
            _meter.fillAmount = Mathf.MoveTowards(_meter.fillAmount, 1, Time.deltaTime);
        }
    }

    private void HandleOnPlayerDetected()
    {
        if (_clockTimerRunning)
            return;

        var hit = Physics2D.OverlapCircle(_timerClock.transform.position, _detectionRadius, _playerLayer);
        if(hit)
        {
            _clockTimerRunning = true;
            _currentTime = _maxTime;
            FindObjectOfType<AudioManager>().PlaySound(_clockTouchAudio, _audioSource, TrackType.Sfx, false);
            _timerClock.GetComponent<SpriteRenderer>().DOFade(0, 1);
        }
    }

    public void OnAllKeysCollected()
    {
        _clockTimerRunning = false;
        FindObjectOfType<AudioManager>().StopSound(_audioSource);
        _meterGameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if(_timerClock != null)
            Gizmos.DrawWireSphere(_timerClock.transform.position, _detectionRadius);
    }


}
