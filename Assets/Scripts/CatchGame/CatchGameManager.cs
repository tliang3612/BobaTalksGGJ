using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchGameManager : MonoBehaviour
{
    [SerializeField] private Image _meter;
    [SerializeField] private int _requiredItems;

    [SerializeField] private ProjectileSpawner _itemSpawner;
    [SerializeField] private ProjectileSpawner _projectileSpawner;
    [SerializeField] private GameObject _victoryDoor;

    private float _collectedItems;
    private float _fillAmountPerItem;

    private void Start()
    {
        _collectedItems = 0;
        _meter.fillAmount = 0;
        _fillAmountPerItem = (float)1 / _requiredItems;
        
    }

    public void OnItemCollected()
    {
        _collectedItems++;
        _meter.fillAmount = _collectedItems * _fillAmountPerItem;
        HandleVictory();
    }

    public void HandleVictory()
    {
        if(_collectedItems == _requiredItems)
        {
            _victoryDoor.SetActive(true);
            _itemSpawner.StopSpawn();
            _projectileSpawner.StopSpawn();
            Debug.Log("You Win!");
        }
    }
}
