using UnityEngine;

namespace TowerDefence
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private EnemySpawner enemySpawner;
        [SerializeField] private Transform towersRoot;
        [SerializeField] private Transform waypointsRoot;

        private Vector3[] path;
        private void Awake()
        {
            path = new Vector3[waypointsRoot.childCount];

            for (int i = 0; i < waypointsRoot.childCount; i++)
            {
                path[i] = waypointsRoot.GetChild(i).position;
            }
            enemySpawner.Init(path);
        }
    }
}
