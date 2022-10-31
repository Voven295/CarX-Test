using UnityEngine;

namespace TowerDefence
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private Transform towersRoot;
        [SerializeField] private Transform waypointsRoot;
        [SerializeField] private PoolManager poolManager;

        private Vector3[] path;

        private void Awake()
        {
            path = new Vector3[waypointsRoot.childCount];

            for (var i = 0; i < waypointsRoot.childCount; i++) path[i] = waypointsRoot.GetChild(i).position;

            foreach (Transform towerTransform in towersRoot)
            {
                var enemyFinder = towerTransform.GetComponentInChildren<EnemyFinder>();
                enemySpawner.OnEnemiesCreated += enemyFinder.Init;
            }
            
            enemySpawner.Init(path, poolManager);
        }
    }
}