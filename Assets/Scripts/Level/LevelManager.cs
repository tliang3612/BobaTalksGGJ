using Cinemachine;
using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
	[SerializeField] private Transform _spawnPosition;
    [SerializeField] private Fader _fader;
    
    private Camera _camera;

    private void Awake()
    {
        _fader = FindObjectOfType<Fader>();
        _camera = Camera.main;
    }

    private void Start()
    {
        InstantiatePlayer();
    }

    public void GoToLevel(string levelName)
    {
        StartCoroutine(StartLevelSequence(levelName));
    }

    private IEnumerator StartLevelSequence(string levelName)
    {
        yield return null;
    }

    private void InstantiatePlayer()
    {
        if (_playerPrefab == null)
            return;

		GameObject playerObject = Instantiate(_playerPrefab, _spawnPosition.position, Quaternion.identity);      
    }

}
