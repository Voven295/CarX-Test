using System.Diagnostics;
using TowerDefence;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class CannonProjectile : MonoBehaviour, IPooledObject
{
    private bool isAlive;
    private float t;
    
    private Vector3 launchPoint;
    private Vector3 launchVelocity;
    
    public bool IsReadyToReuse { get; private set; } = true;
    public void Init(Vector3 launchPoint, Vector3 launchVelocity)
    {
        this.launchPoint = launchPoint;
        this.launchVelocity = launchVelocity;
        isAlive = true;
    }
    
    private void Update()
    {
        if (isAlive) MoveByTrajectory();
    }

    public void MoveByTrajectory()
    {
        IsReadyToReuse = false;
        transform.position += launchVelocity * Time.deltaTime;

        if (transform.position.y <= 0f)
        {
            isAlive = false;
            t = 0;
            gameObject.SetActive(false);
            IsReadyToReuse = true;
        }
    }

    public void ObjectReuse()
    {
        
    }

}
