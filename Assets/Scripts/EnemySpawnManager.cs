using System;
using TowerDefence;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
   [SerializeField, Range(1, 10)] private float delayToSpawnInSec = 3f;
   [SerializeField] private GameObject enemyPrefab;
   [SerializeField] private int enemiesCount;
   
   private PoolManager poolManager;
   private Vector3 enemyStartPosition;
   private Quaternion enemyStartRotation;
   private float currentTime = 0f;

   public event Action<Enemy> OnEnemySpawn;
   public event Action OnEnemyDisable;
   public void Init(PoolManager manager, System.Numerics.Vector3[] path)
   {
      poolManager = manager;
      var enemies =poolManager.CreatePool<Enemy>(enemyPrefab, transform, enemiesCount);

      foreach (var enemy in enemies)
      {
         enemy.Init(path);
         enemy.OnDisabled += () => OnEnemyDisable?.Invoke();
         enemyStartPosition = enemy.transform.position;
         enemyStartRotation = enemy.transform.rotation;
      }
   }

   public void Spawn()
   {
      if (currentTime < delayToSpawnInSec)
      {
         currentTime += Time.deltaTime;
      }
      else
      {
         Respawn();
         currentTime = 0;
      }
   }

   private void Update()
   {
      if (poolManager != null) Spawn();      
   }

   private void Respawn()
   {
      var enemyTransform = poolManager
         .ReuseObject(enemyPrefab, enemyStartPosition, enemyStartRotation);
      
      if (enemyTransform == null) return;

      var enemy = enemyTransform.gameObject.GetComponent<Enemy>();
      
      OnEnemySpawn?.Invoke(enemy);
   }
}
