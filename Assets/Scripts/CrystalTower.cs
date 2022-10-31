using UnityEngine;

namespace TowerDefence
{
	public class CrystalTower : MonoBehaviour
	{
		[SerializeField] private float shootPerSeconds = 1.25f;
		[SerializeField] private float reloadDuration = 1f;
		[SerializeField] private GameObject guidedProjectile;
		[SerializeField] private Transform crystal;
		
		private EnemyFinder enemyFinder;
		private Enemy targetEnemy;

		private Vector3 launchVelocity;
		private Vector3 direction;
		private Vector3 currentTargetPoint;
		
		private float reloadT;
		private float bulletSpeed;

		//private float maxDistance => enemyFinder.MaxDistance;

		private void Awake()
		{
			enemyFinder = GetComponentInChildren<EnemyFinder>();
		}
		
		private void Update()
		{
			targetEnemy = enemyFinder.TargetEnemy;

			if (targetEnemy != null)
			{
				reloadT += Time.deltaTime * shootPerSeconds;

				if (!(reloadT >= reloadDuration)) return;
                
				Shoot();
				reloadT -= reloadDuration;
			}
			
		}
		
		public void Shoot()
		{
			var projectile = Instantiate(guidedProjectile, crystal.transform.position, Quaternion.identity)
				.GetComponent<GuidedProjectile>();
			projectile.Init(targetEnemy.transform);
		}
		/*public float m_shootInterval = 0.5f;
		public float m_range = 4f;
		public GameObject m_projectilePrefab;

		private float m_lastShotTime = -0.5f;
	
		void Update () {
			if (m_projectilePrefab == null)
				return;

			foreach (var monster in FindObjectsOfType<Enemy>()) {
				if (Vector3.Distance (transform.position, monster.transform.position) > m_range)
					continue;

				if (m_lastShotTime + m_shootInterval > Time.time)
					continue;

				// shot
				var projectile = Instantiate(m_projectilePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity) as GameObject;
				//var projectileBeh = projectile.GetComponent<GuidedProjectile> ();
				//projectileBeh.m_target = monster.gameObject;

				//m_lastShotTime = Time.time;
			}
	
		}*/

		
	}	
}

