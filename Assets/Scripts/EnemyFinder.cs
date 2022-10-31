using UnityEngine;

namespace TowerDefence
{
    public class EnemyFinder : MonoBehaviour
    {
        [SerializeField]
        private float maxDistance = 12f;

        private Enemy[] enemies;
        private bool isInit = false;
        public float MaxDistance => maxDistance;

        public Enemy TargetEnemy;

        public void Init(Enemy[] enemies)
        {
            this.enemies = enemies;
            isInit = true;
        }
        
        private Vector3 GetEnemyFuturePosition(Enemy enemy)
        {
            return enemy.GetFuturePosition(enemy.Pp, CanonTower.TravelTime * Enemy.Speed).currentPosition;
        }
        private Enemy EnemyInRange()
        {
            if (!isInit) return null;
            
            foreach (var enemy in enemies)
            {
                if (Vector3.Distance(transform.position, GetEnemyFuturePosition(enemy))
                    <= maxDistance && enemy.IsActive)
                {
                    return enemy;
                }
            }

            return null;
        }
        
        private void Update()
        {
            if (TargetEnemy == null) TargetEnemy = EnemyInRange();
            else if (Vector3.Distance(transform.position, GetEnemyFuturePosition(TargetEnemy)) > maxDistance || 
                     !TargetEnemy.IsActive)
            {
                TargetEnemy = null;
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
