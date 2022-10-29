using UnityEngine;

namespace TowerDefence
{
    public class CannonTower : MonoBehaviour, ITower
    {
        [SerializeField] private GameObject shellPrefab;
        [SerializeField] private Transform cannon;
        [SerializeField] private Transform cannonBase;
        [SerializeField] private Transform origin;
        [SerializeField] private float speedRotation = 4f;
        [SerializeField] private float shootPerSeconds = 1.25f;

        [SerializeField] private EnemyFinder enemyFinder;

        private PoolManager poolManager;

        private Vector3 shellVelocity;
        private Enemy targetEnemy;

        private Quaternion cannonBaseStartRotation;
        private Quaternion cannonStartRotation;

        private Vector3 launchPoint;
        private Vector3 launchVelocity;

        [SerializeField] private float reloadDuration = 1f;
        private float reloadT;
        private float bulletSpeed;
		private float travelTime = 0.5f;

		private Vector3 direction;
        private Vector3 currentTargetPoint;

        public void Init(PoolManager poolManager)
        {
            this.poolManager = poolManager;
        }

        private void Awake()
        {
            cannonBaseStartRotation = cannonBase.transform.rotation;
            cannonStartRotation = cannon.transform.rotation;
            launchPoint = cannon.position;
            OnValidate();
        }

        void OnValidate()
        {
            float x = enemyFinder.MaxDistance + 0.25f;
            float y = -cannon.position.y;
        }

        private void Update()
        {
            targetEnemy = enemyFinder.targetEnemy;

            if (targetEnemy != null)
            {
                if (CalculateTrajectory())
                {
                    reloadT += Time.deltaTime * shootPerSeconds;

                    if (reloadT >= reloadDuration)
                    {
                        Shoot();
                        reloadT -= reloadDuration;
                    }
                }
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
			CalculateTrajectory();
            var shell = poolManager.ReuseObject(shellPrefab, origin.transform.position, Quaternion.identity);
            shell.gameObject.GetComponent<CannonProjectile>().Init(launchPoint, launchVelocity);
        }

        private bool GetGhostPosition(float seconds, out Vector3 result)
        {
			// TODO(sqdrck): Proper ghost calculations based on waypoints.
            result = new Vector3(targetEnemy.transform.position.x - Enemy.Speed * seconds,
                targetEnemy.transform.position.y,
                targetEnemy.transform.position.z);

            return Vector3.Distance(transform.position, result) <= enemyFinder.MaxDistance;
        }

        private bool CalculateTrajectory()
        {
            if (!GetGhostPosition(travelTime, out currentTargetPoint)) return false;

			var dist = Vector3.Distance(origin.position, currentTargetPoint);
			bulletSpeed = dist / travelTime;
			Debug.Log("Bullet speed = " + bulletSpeed);

			direction = (currentTargetPoint - origin.position).normalized;

			// TODO(sqdrck): Rotation.
            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            //var cannonBaseEulerAngles = cannonBase.transform.rotation.eulerAngles;
            //Quaternion cannonBaseTargetRotation = Quaternion.Euler(cannonBaseEulerAngles.x, lookRotation.eulerAngles.y,
                //cannonBaseEulerAngles.z);
            //Quaternion cannonTargetRotation = Quaternion.Euler(lookRotation.eulerAngles.x, cannonBaseEulerAngles.y,
                //cannonBaseEulerAngles.z);
            //Rotate(cannonBaseTargetRotation, cannonTargetRotation);

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
