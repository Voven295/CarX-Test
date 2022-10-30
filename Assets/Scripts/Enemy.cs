using System;
using UnityEngine;

namespace TowerDefence
{
    public class Enemy : MonoBehaviour, IPooledObject
    {
        public struct PathPosition
        {
            public Vector3[] path;
            public Vector3 currentPosition;
            public int currentSegment;
            public float currentT;
            public bool completed;
        }

        public PathPosition Pp => pp;
        private bool isInit = false;
        private bool isMoving;

        public const float Speed = 10f;

        PathPosition pp;

        public bool IsReadyToReuse { get; private set; } = true;
        public event Action OnDisabled;

        private void OnDisable()
        {
            OnDisabled?.Invoke();
        }

        public void Init(Vector3[] path)
        {
            isInit = true;
            pp = new PathPosition();
            pp.path = path;
            StartMoving();
        }

        private void StartMoving()
        {
            pp.currentSegment = 0;
            pp.currentT = 0;
            pp.completed = false;
            pp.currentPosition = pp.path[0];
            transform.position = pp.path[0];
            isMoving = true;
            IsReadyToReuse = false;
        }


        public PathPosition GetFuturePosition(PathPosition pp, float skipT)
        {
            if (pp.completed) return pp;
            var segmentsDist = Vector3.Distance(pp.path[pp.currentSegment + 1], pp.path[pp.currentSegment]);
            pp.currentT += skipT / segmentsDist;

            if (pp.currentT >= 1)
            {
                pp.currentSegment++;
                pp.currentT -= 1;
            }

            if (pp.path.Length - 1 == pp.currentSegment)
            {
                pp.completed = true;
                pp.currentPosition = pp.path[pp.currentSegment];
            }
            else
            {
                pp.currentPosition = Vector3.Lerp(pp.path[pp.currentSegment], pp.path[pp.currentSegment + 1], pp.currentT);
            }
            return pp;
        }

        private void Update()
        {
            if (!(isInit && isMoving)) return;

            var newPp = GetFuturePosition(pp, Time.deltaTime * Speed);
            pp = newPp;
            transform.position = pp.currentPosition;
            if (pp.completed)
            {
                // NOTE(sqdrck): Reached.
                isMoving = false;
                IsReadyToReuse = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(pp.path[pp.currentSegment], 0.2f);
        }

        public void ObjectReuse()
        {
        }
    }
}
