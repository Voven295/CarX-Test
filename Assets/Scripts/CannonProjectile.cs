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
    
    Stopwatch stopwatch = new Stopwatch();
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
        stopwatch.Start();
        IsReadyToReuse = false;
        t += Time.deltaTime;
        Vector3 p = launchPoint + launchVelocity * t;
        p.y -= 0.5f * 9.81f * t * t;
        
        transform.localPosition = p;
        
        Vector3 d = launchVelocity;
        d.y -= 9.81f * t;
        transform.localRotation = Quaternion.LookRotation(d);

        if (p.y <= 0f)
        {
            stopwatch.Stop();
            Debug.Log("Current time: " + (float)stopwatch.ElapsedMilliseconds);
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
