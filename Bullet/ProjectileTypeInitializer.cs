using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTypeInitializer : MonoBehaviour
{
    [TextArea]
    [Tooltip("This component initializes default variable for behavior and movement projectile interfaces")]
    public string Notes = "This component initializes default variable for behavior and movement projectile interfaces";
    ProjectileManager mProjectileManager;

    //Constructor Values
    Rigidbody rb;
    ProjectileTargetDetection TargetDetected;

    public IProjectileBehavior TYPE_B_NORMAL { get; set; }
    public IProjectileBehavior TYPE_B_HOMING { get; set; }

    public IProjectileMovement TYPE_M_NORMAL { get; set; }
    public IProjectileMovement TYPE_M_WAVE { get; set; }
    public IProjectileMovement TYPE_M_THROW { get; set; }

    public ProjectileObjectPooler m_POP;

    public float speed;
    public float frequency = 5f;
    public float amplitude = 2f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        TargetDetected = GetComponent<ProjectileTargetDetection>();
        mProjectileManager = GetComponent<ProjectileManager>();

        //Initialize Behaviors here
        TYPE_B_NORMAL = new ProjectileNormal();
        TYPE_B_HOMING = new ProjectileHoming(TargetDetected);
            //..add more here




        //Initialize Movements here
        TYPE_M_NORMAL = new ProjectileForwardMovement(rb, transform, speed);
        TYPE_M_WAVE = new ProjectileWaveMove(rb, transform, speed, frequency, amplitude);
        TYPE_M_THROW = new ProjectileThrow(rb, transform, 0);
            //..add more here


            

        
    }
}
