using System;
using TowerDefence;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float delayToSpawnInSec = 3f;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesCount;

    private PoolManager poolManager;
    private Vector3 enemyStartPosition;
    private Quaternion enemyStartRotation;
    private float currentTime = 0f;

    public event Action<Enemy> OnEnemySpawn;
    public event Action OnEnemyDisable;
    public void Init(Vector3[] path)
    {
        var enemyGo = GameObject.Instantiate(enemyPrefab, path[0], Quaternion.identity);

        var enemy = enemyGo.GetComponent<Enemy>();
        enemy.Init(path);

        OnEnemySpawn?.Invoke(enemy);
    }
}
