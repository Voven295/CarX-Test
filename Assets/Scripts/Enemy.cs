using UnityEngine;

namespace TowerDefence
{
    public class Enemy : MonoBehaviour, IPooledObject
    {
        public struct PathPosition
        {
            public Vector3[] path;
            public Vector3 startPosition;
            public Vector3 currentPosition;
            public Vector3 targetPosition;
            public int currentSegment;
            public float currentT;
            public bool completed;
        }
    
        [SerializeField] private int maxHp = 100;
        
        private int hp;
        private bool isInit = false;
        private bool isMoving;
        
        public PathPosition Pp => pp;
        public bool IsActive => gameObject.activeInHierarchy;
        public const float Speed = 10f;
        private PathPosition pp;
        
        public void Init(Vector3[] path)
        {
            hp = maxHp;
            isInit = true;
            pp = new PathPosition
            {
                path = path
            };
            pp.startPosition = pp.path[0];
            StartMoving();
        }
        
        private void StartMoving()
        {
            pp.currentSegment = 0;
            pp.currentT = 0;
            pp.completed = false;
            pp.currentPosition = pp.startPosition;
            transform.position = pp.startPosition;
            isMoving = true;
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
                pp.targetPosition = pp.path[pp.currentSegment + 1];
                pp.currentPosition = Vector3.Lerp(pp.path[pp.currentSegment], pp.targetPosition, pp.currentT);
            }
            return pp;
        }

        public void TakeDamage(int damage)
        {
            hp -= damage;
            
            if (hp <= 0)
            {
                isMoving = false;
                gameObject.SetActive(false);
            }
        }
        
        public void ObjectReuse()
        {
            StartMoving();
            hp = maxHp;
        }
        
        private void Update()
        {
            if (!(isInit && isMoving)) return;

            var newPp = GetFuturePosition(pp, Time.deltaTime * Speed);
            pp = newPp;
            transform.position = pp.currentPosition;
            transform.LookAt(pp.targetPosition);
            
            if (pp.completed)
            {
                //Reached.
                isMoving = false;
                gameObject.SetActive(false);
            }
        }
    }
}
