using Vector3 = System.Numerics.Vector3;

namespace TowerDefence
{
    internal class EnemyMovement
    {
        private float t;
        public Vector3 startPosition { get; private set; }
        public Vector3 currentPosition { get; private set; }
        
        public bool isMoving { get; private set; }
        
        private readonly float Speed;

        public EnemyMovement(Vector3 startPosition, float speed)
        {
            this.startPosition = startPosition;
            Speed = speed;
        }
        
        public void StartMoving()
        {
            t = 0;
            isMoving = true;
            currentPosition = startPosition;
        }

        public void StopMoving()
        {
            isMoving = false;
        }

        public bool MoveByPath(Vector3[] path, float deltaTime, ref int startIndex)
        {
            var targetPosition = path[startIndex + 1];
            startPosition = path[startIndex];

            if (currentPosition != targetPosition)
            {
                MoveTo(startPosition, targetPosition, deltaTime);
            }
            else
            {
                startIndex++;
            }

            if (startIndex == path.Length - 1)
            {
                StopMoving();
                return false;
            }
            
            return true;
        } 
        
        public void MoveTo(Vector3 startPos, Vector3 targetPosition, float deltaTime)
        {
            t += Speed * deltaTime;

            currentPosition = Utils.Lerp(startPos, targetPosition, t);
            
            if (t > 1) t -= 1;
        }
    }
}