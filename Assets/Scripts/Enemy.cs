using System;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace TowerDefence
{
    public class Enemy : MonoBehaviour, IPooledObject
    {
        private Vector3[] path;
        private EnemyMovement movement;
        private int startIndex = 0;
        private bool isInit = false;

        public const float Speed = 1f;
        public bool IsReadyToReuse { get; private set; } = true;
        public event Action OnDisabled;

        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }
        
        public void Init(Vector3[] path)
        {
            this.path = path;
            movement = new EnemyMovement(path[0], Speed);
            transform.position = Utils.FromNumericsToUnity(movement.startPosition);
            movement.StartMoving();
            isInit = true;
        }
        
        private void Update()
        {
            if (!(isInit && movement.isMoving)) return;

            if (movement.MoveByPath(path, Time.deltaTime, ref startIndex))
            {
                transform.position = Utils.FromNumericsToUnity(movement.currentPosition);
                IsReadyToReuse = false;
            }
            else
            {
                IsReadyToReuse = true;
                startIndex = 0;
                gameObject.SetActive(false);
            }
        }

        public void ObjectReuse()
        {
            movement.StartMoving();
        }

    }
}