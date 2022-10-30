using System.Collections;
using UnityEngine;

namespace TowerDefence
{
    public class CannonTower : MonoBehaviour
    {
        [SerializeField] private GameObject shellPrefab;
        [SerializeField] private Transform cannon;
        [SerializeField] private Transform cannonBase;
        [SerializeField] private Transform origin;
        [SerializeField] private float speedRotation = 4f;
        [SerializeField] private float shootPerSeconds = 1.25f;
        [SerializeField] private float maxCanonRotationX = 15f;
        [SerializeField] private float reloadDuration = 1f;
    
        private EnemyFinder enemyFinder;
        [SerializeField] private Enemy targetEnemy;

        private Quaternion cannonBaseStartRotation;
        private Quaternion cannonStartRotation;

        private Vector3 launchVelocity;
        
        private float reloadT;
        private float bulletSpeed;
        private const float TravelTime = 0.5f;

        private Vector3 direction;
        private Vector3 currentTargetPoint;
        
        private void Awake()
        {
            enemyFinder = GetComponentInChildren<EnemyFinder>();
            cannonBaseStartRotation = cannonBase.transform.rotation;
            cannonStartRotation = cannon.transform.rotation;
        }

        private void Update()
        {
            targetEnemy = enemyFinder.TargetEnemy;

            if (targetEnemy != null)
            {
                if (!CalculateTrajectory()) return;
                
                reloadT += Time.deltaTime * shootPerSeconds;

                if (!(reloadT >= reloadDuration)) return;
                
                Shoot();
                reloadT -= reloadDuration;
            }
            else
            {
                reloadT = 0;
                Rotate(cannonBaseStartRotation, cannonStartRotation);
            }
        }

        private void Rotate(Quaternion targetRot, Quaternion targetRot1)
        {
            cannonBase.transform.rotation = Quaternion.Lerp(cannonBase.transform.rotation, targetRot,
                speedRotation * Time.deltaTime);
            cannon.transform.rotation = Quaternion.Lerp(cannon.transform.rotation, targetRot1,
                speedRotation * Time.deltaTime);
        }

        public void Shoot()
        {
            var shellGo = Instantiate(shellPrefab, origin.transform.position, Quaternion.identity);
            var shell = shellGo.GetComponent<CannonProjectile>();
            shell.Init(launchVelocity);
            
            Destroy(shellGo, TravelTime);
            StartCoroutine(DealDamage());
        }

        private IEnumerator DealDamage()
        {
            yield return new WaitForSeconds(TravelTime);
            if (targetEnemy == null) yield break;
            targetEnemy.TakeDamage(CannonProjectile.Damage);
        }
        
        private bool GetGhostPosition(float seconds, out Vector3 result)
        {
            result = targetEnemy.GetFuturePosition(targetEnemy.Pp, seconds * Enemy.Speed).currentPosition;
            return Vector3.Distance(origin.position, result) <= enemyFinder.MaxDistance;
        }

        private bool CalculateTrajectory()
        {
            if (!GetGhostPosition(TravelTime, out currentTargetPoint)) return false;
            
            var dist = Vector3.Distance(origin.position, currentTargetPoint);
            
            bulletSpeed = dist / TravelTime;
            direction = (currentTargetPoint - origin.position).normalized;
            
            Quaternion lookRotation = Quaternion.LookRotation(direction);

            var lookRotationX = Mathf.Clamp(lookRotation.eulerAngles.x, 0, maxCanonRotationX);
            
            Quaternion canonBaseTargetRot = Quaternion.Euler(cannonBase.transform.rotation.eulerAngles.x,
                lookRotation.eulerAngles.y, cannonBase.transform.rotation.eulerAngles.z);
            Quaternion canonTargetRot = Quaternion.Euler(lookRotationX, cannon.transform.rotation.eulerAngles.y,
                cannon.transform.rotation.eulerAngles.z);

            Rotate(canonBaseTargetRot, canonTargetRot);

            launchVelocity = direction * bulletSpeed;

            return true;
        }

        private void OnDrawGizmos()
        {
            if (targetEnemy == null) return;

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetEnemy.transform.position, 0.5f);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(currentTargetPoint, 0.5f);
            Gizmos.DrawLine(origin.position, origin.position + direction * 5);
        }
    }
}
