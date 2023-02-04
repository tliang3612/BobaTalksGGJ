using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPosition;
    [SerializeField] private bool _spawnPlayerOnStart;

    private CinemachineVirtualCamera _camera;

    private Fader _fader;

    private void Awake()
    {
        if (_spawnPlayerOnStart)
            InstantiatePlayer();

        if (FindObjectOfType<CinemachineVirtualCamera>() != null)
            _camera = FindObjectOfType<CinemachineVirtualCamera>();

        _fader = FindObjectOfType<Fader>();
    }

    private void Start()
    {
        _fader.FadeOut();

        if (FindObjectOfType<Player>() != null)
        {
            _camera.m_Follow = FindObjectOfType<Player>().transform;
            FindObjectOfType<Player>().PlayerDeathEvent += (player) => StartCoroutine(RespawnPlayer(player));
        }

    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator RespawnPlayer(Player player)
    {
        yield return player.PlayDeathAnimation();
        ReloadScene();
    }

    public void SpawnPlayerAtPoint(Vector3 position)
    {
        FindObjectOfType<Player>().transform.position = position;
    }


    public void GoToLevel(string levelName)
    {
        Time.timeScale = 1f;
        StartCoroutine(StartNextLevelSequence(levelName.TrimEnd()));
    }

    private IEnumerator StartNextLevelSequence(string levelName)
    {
        if (FindObjectOfType<Player>() != null)
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
