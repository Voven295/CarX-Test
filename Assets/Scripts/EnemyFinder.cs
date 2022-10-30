using UnityEngine;

namespace TowerDefence
{
    public class EnemyFinder : MonoBehaviour
    {
        [SerializeField]
        private float maxDistance = 12f;
        public float MaxDistance => maxDistance;

        public Enemy TargetEnemy { get; private set; } = null;

        private void OnTriggerEnter(Collider other)
        {
            if (TargetEnemy == null)
                TargetEnemy = other.GetComponent<Enemy>();
        }

        private void OnTriggerExit(Collider other)
        {
            TargetEnemy = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, MaxDistance);
        }
#endif
    }
}
