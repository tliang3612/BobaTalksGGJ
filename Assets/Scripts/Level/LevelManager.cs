using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
	[SerializeField] private Transform _spawnPosition;
    [SerializeField] private bool _spawnPlayerOnStart;

    private Fader _fader;   
    private Camera _camera;

    private void Awake()
    {
        if(_spawnPlayerOnStart)
            InstantiatePlayer();
        
        _fader = FindObjectOfType<Fader>();
        _camera = Camera.main;
    }

    private void Start()
    {
        _fader.FadeOut();

        if(FindObjectOfType<Player>() != null)
            FindObjectOfType<Player>().PlayerDeathEvent += (player) => StartCoroutine(Respawn(player));
    }

    public IEnumerator Respawn(Player player)
    {
        yield return player.PlayDeathAnimation();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SpawnPlayerAtPoint(Vector3 position)
    {
        FindObjectOfType<Player>().transform.position = position;
    }


    public void GoToLevel(string levelName)
    {
        StartCoroutine(StartNextLevelSequence(levelName));
    }

    private IEnumerator StartNextLevelSequence(string levelName)
    {
        if(FindObjectOfType<Player>() != null)
            FindObjectOfType<Player>().FadePlayerOut();

        _fader.FadeIn();
        yield return new WaitForSeconds(_fader.FadeDuration);
        LoadingScene.LoadScene(levelName);
    }

    private void InstantiatePlayer()
    {
        if (_playerPrefab == null || FindObjectOfType<Player>() != null)
            return;

		GameObject playerObject = Instantiate(_playerPrefab, _spawnPosition.position, Quaternion.identity);
    }

}
