using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] _projectileSpawnPoints;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private Vector2 _projectileDirection;
    [SerializeField] private Projectile _projectileToSpawn;
    private Vector2 _currentSpawnPosition;

    public void Update()
    {
        foreach(var spawnPoint in _projectileSpawnPoints)
        {

        }
    }

    public void SpawnProjectiles()
    {
        /*var projectile = Instantiate(_projectileToSpawn, transform).GetComponent<Projectile>();
        projectileScript.FireProjectile(stateData.projectileSpeed, stateData.projectileTravelDistance, stateData.projectileDamage);*/
    }
}
