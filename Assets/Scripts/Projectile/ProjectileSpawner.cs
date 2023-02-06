using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private Vector2 _projectileDirection;
    [SerializeField] private Vector2 _projectileSpawnRange;
    [SerializeField] private GameObject _projectileToSpawn;
    [SerializeField] private float _projectileSpawnDelay;
    [SerializeField] private float _projectileDuration;
    [SerializeField] private bool _destroyOnGroundContact;
    [SerializeField] private AudioClip _audioToPlay;

    private AudioManager _audioManager;
    private AudioSource _audioSource;
    private bool _canSpawn;

    private float _lastProjectileSpawnTime;
    private List<Projectile> _projectileList;

   
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioManager = FindObjectOfType<AudioManager>();
    }

    public void Start()
    {
        _projectileList = new List<Projectile>();
        _lastProjectileSpawnTime = Time.time;
        _canSpawn = true;
    }

    public void Update()
    {
        if (!_canSpawn)
            return;

        var positionToSpawn = _projectileSpawnPoint.position + new Vector3(Random.Range(0, _projectileSpawnRange.x),
            Random.Range(0, _projectileSpawnRange.y), 0);


        if (Time.time >= _lastProjectileSpawnTime + _projectileSpawnDelay)
        {
            _projectileList.Add(SpawnProjectile(positionToSpawn));            
        }                       
    }

    public Projectile SpawnProjectile(Vector2 spawnPoint)
    {
        
        _lastProjectileSpawnTime = Time.time;
        var projectile = Instantiate(_projectileToSpawn, spawnPoint, Quaternion.identity, transform).GetComponent<Projectile>();
        projectile.FireProjectile(_projectileDirection, _projectileDuration, _projectileSpeed,  _destroyOnGroundContact);
        projectile.ProjectileDestroyedOnPlayerEvent += PlayProjectileSound;
        return projectile;
    }

    public void PlayProjectileSound()
    {
        if (_audioToPlay)
            _audioManager.PlaySound(_audioToPlay, _audioSource, TrackType.Sfx, false);
    }

    public void StopSpawn()
    {
        _canSpawn = false;

        if (_audioToPlay)
            _audioManager.StopSound(_audioSource);

        foreach (var projectile in GetComponentsInChildren<Projectile>())
        {
            projectile.DestroyProjectile();          
        }
    }

    private void OnDrawGizmos()
    {
       Gizmos.DrawLine(transform.position, transform.position + new Vector3(_projectileSpawnRange.x, _projectileSpawnRange.y, 0));
    }
}
