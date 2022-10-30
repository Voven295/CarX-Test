using UnityEngine;

namespace TowerDefence
{
    public class CannonProjectile : MonoBehaviour
    {
        private Vector3 launchVelocity;
        
        private bool isInit = false;

        public const int Damage = 15;
        public void Init(Vector3 launchVelocity)
        {
            isInit = true;
            this.launchVelocity = launchVelocity;
        }
    
        private void Update()
        {
            if (isInit) Move();
        }

        public void Move()
        {
            transform.position += launchVelocity * Time.deltaTime;
        }
    }
}