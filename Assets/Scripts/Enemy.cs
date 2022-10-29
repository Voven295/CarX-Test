using System;
using UnityEngine;

namespace TowerDefence
{
    public class Enemy : MonoBehaviour, IPooledObject
    {
        private Vector3[] path;
        private bool isInit = false;
        private bool isMoving;

        public const float Speed = 5f;

        private float pathSegmentT;
        private int currentPathSegment;

        public bool IsReadyToReuse { get; private set; } = true;
        public event Action OnDisabled;

        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }

        public void Init(Vector3[] path)
        {
            isInit = true;
            this.path = path;
            StartMoving();
        }

        private void StartMoving()
        {
            currentPathSegment = 0;
            pathSegmentT = 0;
            transform.position = path[0];
            isMoving = true;
            IsReadyToReuse = false;
        }

        private void Update()
        {
            if (!(isInit && isMoving)) return;

            var segmentsDist = Vector3.Distance(path[currentPathSegment + 1], path[currentPathSegment]);

            pathSegmentT += Time.deltaTime * Speed / segmentsDist;
            transform.position = Vector3.Lerp(path[currentPathSegment], path[currentPathSegment + 1], pathSegmentT);

            if (pathSegmentT >= 1)
            {
                pathSegmentT -= 1f;
                currentPathSegment++;
                if (path.Length == currentPathSegment)
                {
                    // NOTE(sqdrck): Reached.
                    isMoving = false;
                    IsReadyToReuse = true;
                }
            }
        }

        public void ObjectReuse()
        {
        }
    }
}
