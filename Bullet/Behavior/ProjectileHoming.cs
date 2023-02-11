using UnityEngine;

public class ProjectileHoming : IProjectileBehavior
{
    [Header("TargetDetection GO")]
    ProjectileTargetDetection TargetDetected;

    public ProjectileHoming(ProjectileTargetDetection newTargetDetected)
    {
        this.TargetDetected = newTargetDetected;
    }

    public void ProjectileBehavior()
    {
        if (TargetDetected.target != null) 
        {
            TargetDetected.transform.LookAt(TargetDetected.target.transform);
        }
    }
}
