using UnityEngine;

namespace TowerDefence
{
	public class GuidedProjectile : MonoBehaviour 
	{
		[SerializeField] private float speed = 2f;
		[SerializeField] private float minDistance = 0.01f;
		
		private Transform target;
		private bool isInit = false;
		public const int Damage = 10;

		public void Init(Transform target)
		{
			this.target = target;
			isInit = true;
		}
	
		void Update () {
		
			if (!isInit) return;
			
			Move();

			if (Vector3.Distance(transform.position, target.position) <= minDistance)
			{
				target.gameObject.GetComponent<Enemy>().TakeDamage(Damage);
				Destroy(gameObject);
			}
			
			if (transform == null) 
			{
				Destroy (gameObject);
			}
		}

		private void Move()
		{
			var translation = target.transform.position - transform.position;
			if (translation.magnitude > speed) 
			{
				translation = translation.normalized * speed;
			}
			transform.Translate (translation);
		}
	}
}