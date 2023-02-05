using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PianoGameManager : MonoBehaviour
{
    //[SerializeField] private List<int> _correctKeyOrder;
    [SerializeField] private PianoKey[] _pianoKeys;
    [SerializeField] private AudioClip[] _noteAudioClips;
    //[SerializeField] private AudioClip _incorrectKeyClip;
    /*[SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private GameObject _victoryDoor;*/

    /*[field: SerializeField]
    private List<int> _currentKeyOrder;
    [field: SerializeField]
    private int _nextValidKeyIndex;*/

    private AudioManager _audioManager;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _audioSource = GetComponent<AudioSource>();
        var player = FindObjectOfType<Player>();
        player.LandState = new PlayerLandState(player, player.StateMachine, player.GetPlayerData(), "Land", null);
    }

    private void Start()
    {
        /*_nextValidKeyIndex = 0;
        _currentKeyOrder = new List<int>();*/

        for (int i = 0; i < _pianoKeys.Length; i++)
        {
            _pianoKeys[i].SetKey(i, _noteAudioClips[i]);
            _pianoKeys[i].PianoKeyPressedEvent += OnPianoKeyPressed;
        }
    }

    private void OnPianoKeyPressed(int keyNum, AudioClip clip)
    {
        //_currentKeyOrder.Add(keyNum);
        _audioManager.PlaySound(clip, _audioSource, TrackType.Sfx, false);
        //HandleValidOrder();
    }

    /*private void HandleValidOrder()
    {
        if (_currentKeyOrder.Last() != _correctKeyOrder[_nextValidKeyIndex])
        {
            OnIncorrectKey();
        }
        else if (_currentKeyOrder.Count == _correctKeyOrder.Count)
        {
            if(_projectileSpawner)
                _projectileSpawner.StopSpawn();
            //_bgm.StopMusic();
            _victoryDoor.SetActive(true);
        }
        else
        {
            _nextValidKeyIndex++;
        }
    }

    private void OnIncorrectKey()
    {
        _audioManager.PlaySound(_incorrectKeyClip, _audioSource, TrackType.Sfx, false);
        _nextValidKeyIndex = 0;
        _currentKeyOrder.Clear();
        Debug.Log("Wrong Key Pressed");
    }*/
}
