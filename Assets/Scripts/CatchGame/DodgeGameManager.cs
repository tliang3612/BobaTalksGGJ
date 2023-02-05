using UnityEngine;
using UnityEngine.UI;

public class DodgeGameManager : MonoBehaviour
{
    [SerializeField] private Image _meter;

    [SerializeField] private ProjectileSpawner _projectileSpawnerTop;
    [SerializeField] private ProjectileSpawner _projectileSpawnerRight;

    [SerializeField] private float _maxTime;
    [SerializeField] private float _startDelay;
    [SerializeField] private GameObject _victoryDoor;

    private float _currentFill;
    private bool _won;

    private void Start()
    {
        _meter.fillAmount = 0;
        _currentFill = 0;
    }

    private void Update()
    {
        if (_won)
            return;

        HandleTimerOnStart();
    }

    private void HandleTimerOnStart()
    {
        _currentFill += Time.deltaTime;
        var newFill = 1 / _maxTime * _currentFill;
        _meter.fillAmount = Mathf.MoveTowards(_meter.fillAmount, newFill, Time.deltaTime);

        if (_meter.fillAmount >= 1)
        {
            HandleVictory();
        }
    }


    public void HandleVictory()
    {
        _won = true;
        _victoryDoor.SetActive(true);

        if (_projectileSpawnerTop)
            _projectileSpawnerTop.StopSpawn();

        if (_projectileSpawnerRight)
            _projectileSpawnerRight.StopSpawn();
        Debug.Log("You Win!");

    }
}
