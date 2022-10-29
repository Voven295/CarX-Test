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

		private EnemyFinder enemyFinder;
		private PoolManager poolManager;
		private Vector3 shellVelocity;
		private Enemy targetEnemy;
		private Quaternion cannonBaseStartRotation;
		private Quaternion cannonStartRotation;
		private Vector3 launchPoint;
		private Vector3 launchVelocity;

		private float launchProgress;
		private float launchSpeed;
		
		public void Init(PoolManager poolManager)
		{
			this.poolManager = poolManager;
		}
		
		private void Awake()
		{
			cannonBaseStartRotation = cannonBase.transform.rotation;
			cannonStartRotation = cannon.transform.rotation;
			enemyFinder = GetComponentInChildren<EnemyFinder>();
			launchPoint = cannon.position;
			OnValidate();
		}
		
		void OnValidate ()
		{
			float x = EnemyFinder.MaxDistance + 0.25f;
			float y = -cannon.position.y;
			launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
		}
		
		private void Update()
		{
			targetEnemy = enemyFinder.targetEnemy;
			
			if (targetEnemy != null)
			{
				CalculateTrajectory();

				launchProgress += Time.deltaTime * shootPerSeconds;

				while (launchProgress >= 1f)
				{
					Shoot();
					launchProgress -= 1f;
				}
			}
			else
			{
				launchProgress = 0;
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
			var shell = poolManager.ReuseObject(shellPrefab, origin.transform.position, Quaternion.identity);
			shell.gameObject.GetComponent<CannonProjectile>().Init(launchPoint, launchVelocity);
		}

		private Vector3 GetGhostPosition(float seconds)
		{
			return new Vector3(targetEnemy.transform.position.x - Enemy.Speed * seconds, 
				targetEnemy.transform.position.y,
				targetEnemy.transform.position.z);
		}
		private void CalculateTrajectory()
		{
			//var offsetX = targetEnemy.transform.position.x - 0.7f * Enemy.Speed;
			Vector3 targetPoint = GetGhostPosition(3);
			Vector2 direction;
			
			direction.x = targetPoint.x - launchPoint.x;
			direction.y = targetPoint.z - launchPoint.z;
			float x = direction.magnitude;
			float y = -launchPoint.y;
			
			direction /= x;
			
			float g = 9.81f;
			float s = launchSpeed;
			float s2 = s * s;

			float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
			
			if (r < 0 ) return;
			float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
			float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
			float sinTheta = cosTheta * tanTheta;

			Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, tanTheta, direction.y));
			
			var cannonBaseEulerAngles = cannonBase.transform.rotation.eulerAngles;
			
			Quaternion cannonBaseTargetRotation = Quaternion.Euler(cannonBaseEulerAngles.x, lookRotation.eulerAngles.y, 
				cannonBaseEulerAngles.z); 
			Quaternion cannonTargetRotation = Quaternion.Euler(lookRotation.eulerAngles.x, cannonBaseEulerAngles.y, 
				cannonBaseEulerAngles.z); 
				
			Rotate(cannonBaseTargetRotation, cannonTargetRotation);

			float timeToHit = ((2 * launchSpeed * sinTheta) / g);
			//Debug.Log(timeToHit);

			launchVelocity = new Vector3(s * cosTheta * direction.x, s * sinTheta, s * cosTheta * direction.y);
			
			//Debug
			Vector3 prev = launchPoint;

			for (int i = 1; i <= 20; i++) {
				float t = i / 10f;
				float dx = s * cosTheta * t;
				float dy = s * sinTheta * t - 0.5f * g * t * t;
				var next = launchPoint + new Vector3(direction.x * dx, dy, direction.y * dx);
				Debug.DrawLine(prev, next, Color.blue);
				prev = next;
			}
		}
		
		private void OnDrawGizmos()
		{
			if (targetEnemy == null) return;

			Gizmos.DrawSphere(GetGhostPosition(3), 0.5f);
			//Gizmos.color = Color.gray;
			//Gizmos.DrawSphere(pos, 0.5f);
		}
	}
}