using System;
using UnityEngine;

namespace TowerDefence
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] private float delayToSpawnInSec = 1f;
        [SerializeField, Range(1, 20)] private int enemiesCount = 10;
        [SerializeField] private GameObject enemyPrefab;

        private float passedTime = 0f;
        private bool isInit = false;
        private PoolManager poolManager;
        private Enemy[] createdEnemies;
        public event Action<Enemy[]> OnEnemiesCreated;
        
        public void Init(Vector3[] path, PoolManager poolManager)
        {
            this.poolManager = poolManager;
            createdEnemies = poolManager.CreatePool<Enemy>(enemyPrefab, transform, enemiesCount);

            foreach (var enemy in createdEnemies)
            {
                enemy.Init(path);
            }
        
            OnEnemiesCreated?.Invoke(createdEnemies);
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
                Spawn();
                passedTime -= delayToSpawnInSec;
            }
        }

        public void Spawn()
        {
            if (!isInit) return;
            
            foreach (var enemy in createdEnemies)
            {
                if (enemy.IsActive) continue;
                poolManager.ReuseObject(enemyPrefab);
                return;
            }
        }
    }
}