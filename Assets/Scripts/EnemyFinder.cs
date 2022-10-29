using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerDefence
{
    public class EnemyFinder : MonoBehaviour
    {
        [SerializeField]
        private float maxDistance = 12f;
        public float MaxDistance
        {
            get { return maxDistance; }
            private set { maxDistance = value; }
        }
        
        private Queue<Enemy> activeEnemies;

        public Enemy targetEnemy { get; private set; } = null;

        private void Awake()
        {
            activeEnemies = new Queue<Enemy>();
        }

        public void AddEnemy(Enemy enemy)
        {
            activeEnemies.Enqueue(enemy);
        }

        public void RemoveEnemy()
        {
            activeEnemies.Dequeue();
        }

        private Enemy GetEnemyInRange()
        {
            return activeEnemies
                .FirstOrDefault(enemy => Vector3
                    .Distance(transform.position, enemy.transform.position) <= MaxDistance);
        }

        private void Update()
        {
            if (activeEnemies.Count == 0) return;

            targetEnemy = GetEnemyInRange();

            if (targetEnemy != null)
            {
                Debug.DrawLine(transform.position, targetEnemy.transform.position, Color.green);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, MaxDistance);

        }
#endif
    }
}
