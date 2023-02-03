using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class DoorKey : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private GameObject[] _keys;
    [SerializeField] private float _fadeSpeed;
    [SerializeField] private float _keyDetectionRadius;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private bool _triggersEvent;
    [SerializeField] private AudioClip _keyCollectedClip;
    
    private int _keysObtained;
    private AudioSource _audioSource;
    private Dictionary<GameObject, bool> _keyCollectedDict;

    public Action AllKeysCollectedEvent;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_triggersEvent && FindObjectOfType<TimerMeter>() != null)
            AllKeysCollectedEvent += FindObjectOfType<TimerMeter>().OnAllKeysCollected;
    }

    private void Start()
    {
        _keyCollectedDict = new Dictionary<GameObject, bool>();
        foreach(var key in _keys)
        {
            _keyCollectedDict.Add(key, false);
        }
        _keysObtained = 0;
    }

    private void FixedUpdate()
    {
        foreach(var key in _keys)
        { 
            if(_keyCollectedDict.ContainsKey(key) && _keyCollectedDict[key] == false)
            {
                var hit = Physics2D.OverlapCircle(key.transform.position, _keyDetectionRadius, _playerLayer);
                if (hit)
                    OnKeyCollected(key);
            }           
        }
        
    }

    private void OnKeyCollected(GameObject key)
    {
        key.SetActive(false);
        _keyCollectedDict[key] = true;
        _keysObtained++;
        FindObjectOfType<AudioManager>().PlaySound(_keyCollectedClip, _audioSource, TrackType.Sfx, false);
        if(_keysObtained >= _keys.Length)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        if(_triggersEvent)
            AllKeysCollectedEvent?.Invoke();

        _door.GetComponent<SpriteRenderer>().DOFade(0, _fadeSpeed).OnComplete(() => _door.SetActive(false));
    }

    private void OnDrawGizmos()
    {
        foreach(var key in _keys)
        {
            if(key)
                Gizmos.DrawWireSphere(key.transform.position, _keyDetectionRadius);
        }
        
    }

}
