using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PianoGameManager : MonoBehaviour
{
    [SerializeField] private List<int> _correctKeyOrder;
    [field: SerializeField]
    private List<int> _currentKeyOrder;
    [field: SerializeField]
    private int _nextValidKeyIndex;

    [SerializeField] private PianoKey[] _pianoKeys;
    [SerializeField] private ProjectileSpawner _projectileSpawner;

    

    private void Start()
    {
        _nextValidKeyIndex = 0;
        _currentKeyOrder = new List<int>();

        for(int i=0; i < _pianoKeys.Length; i++)
        {
            _pianoKeys[i].SetKeyNum(i);
            _pianoKeys[i].PianoKeyPressedEvent += OnPianoKeyPressed;
        }
    }

    private void OnPianoKeyPressed(int keyNum)
    {
        _currentKeyOrder.Add(keyNum);
        HandleValidOrder();
    }

    private void HandleValidOrder()
    {
        if (_currentKeyOrder.Last() != _correctKeyOrder[_nextValidKeyIndex])
        {
            OnIncorrectKey();
        }
        else if(_currentKeyOrder.Count == _correctKeyOrder.Count)
        {
            _projectileSpawner.CanSpawn = false;
            Debug.Log("You Win!");
        }
        else
        {
            _nextValidKeyIndex++;
        }
    }

    private void OnIncorrectKey()
    {
        _nextValidKeyIndex = 0;
        _currentKeyOrder.Clear();
        Debug.Log("Wrong Key Pressed");
    }

    



}
