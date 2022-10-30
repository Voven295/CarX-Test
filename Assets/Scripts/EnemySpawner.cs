using System;
using TowerDefence;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float delayToSpawnInSec = 1f;
    [SerializeField] private GameObject enemyPrefab;

    private float passedTime = 0f;
    private bool isInit = false;
    private Vector3[] enemyPath;
    public event Action<Enemy> OnEnemySpawn;
    
    public void Init(Vector3[] path)
    {
        enemyPath = path;
        isInit = true;
    }

    private void Update()
    {
        if (!isInit) return;
        
        if (passedTime <= delayToSpawnInSec)
        {
            passedTime += Time.deltaTime;
        }
        else
        {
            Spawn(enemyPath);
            passedTime -= delayToSpawnInSec;
        }
    }

    public void Spawn(Vector3[] path)
    {
        var enemyGo = Instantiate(enemyPrefab);
        var enemy = enemyGo.GetComponent<Enemy>();
        enemy.Init(path);
        
        OnEnemySpawn?.Invoke(enemy);
    }
}
