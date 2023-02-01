using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private Vector2 _projectileDirection;
    [SerializeField] private GameObject _projectileToSpawn;
    [SerializeField] private float _projectileSpawnDelay;
    [SerializeField] private AudioClip _audioToPlay;

    private AudioManager _audioManager;
    private AudioSource _audioSource;

    private float _lastProjectileSpawnTime;

    public bool CanSpawn { get; set; }

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void Start()
    {
        _lastProjectileSpawnTime = 0f;
        CanSpawn = true;
    }

    public void FixedUpdate()
    {
        if (!CanSpawn)
            return;

        var positionToSpawn = _projectileSpawnPoint.position + new Vector3(Random.Range(0, 15), 0, 0);
        
        if (Time.time >= _lastProjectileSpawnTime + _projectileSpawnDelay)
        {
            SpawnProjectile(positionToSpawn);
        }                       
    }

    public void SpawnProjectile(Vector2 spawnPoint)
    {
        if (_audioToPlay)
            _audioManager.PlaySound(_audioToPlay, _audioSource, TrackType.Sfx, false);
        
        _lastProjectileSpawnTime = Time.time;
        var projectile = Instantiate(_projectileToSpawn, spawnPoint, Quaternion.identity, transform).GetComponent<Projectile>();
        projectile.FireProjectile(_projectileDirection, spawnPoint, _projectileSpeed);       
    }
}
