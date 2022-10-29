using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace TowerDefence
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private PoolManager poolManager;
        [SerializeField] private GameObject shellPrefab;
        [SerializeField] private EnemySpawnManager enemySpawnManager;
        [SerializeField] private Transform towersRoot;
        [SerializeField] private Transform waypointsRoot;
        [SerializeField] private Transform projectilesRoot;

        private Vector3[] path;
        private void Awake()
        {
            path = new Vector3[waypointsRoot.childCount];

            for (int i = 0; i < waypointsRoot.childCount; i++)
            {
                path[i] = Utils.FromUnityToNumerics(waypointsRoot.GetChild(i).position);
            }

            poolManager.CreatePool<CannonProjectile>(shellPrefab, null, 10);
            foreach (Transform tower in towersRoot)
            {
                var currentTower = tower.GetComponent<ITower>();
                currentTower?.Init(poolManager);

                var enemyFinder = tower.GetComponentInChildren<EnemyFinder>();
                
                //BUG: GC spikes
                enemySpawnManager.OnEnemySpawn += enemyFinder.AddEnemy;
                enemySpawnManager.OnEnemyDisable += enemyFinder.RemoveEnemy;
            }

            enemySpawnManager.Init(poolManager, path);
            enemySpawnManager.Spawn();
        }
    }
}