using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
	[SerializeField] private Transform _spawnPosition;

    private Fader _fader;   
    private Camera _camera;

    private void Awake()
    {
        InstantiatePlayer();
        
        _fader = FindObjectOfType<Fader>();
        _camera = Camera.main;
    }

    private void Start()
    {
        _fader.FadeOut();
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


    public void GoToLevel(Player player, string levelName)
    {
        StartCoroutine(StartNextLevelSequence(player, levelName));
    }

    private IEnumerator StartNextLevelSequence(Player player, string levelName)
    {
        player.FadePlayerOut();

        _fader.FadeIn();
        yield return new WaitForSeconds(_fader.FadeDuration);
        LoadingScene.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void InstantiatePlayer()
    {
        if (_playerPrefab == null || FindObjectOfType<Player>() != null)
            return;

		GameObject playerObject = Instantiate(_playerPrefab, _spawnPosition.position, Quaternion.identity);
    }

}
