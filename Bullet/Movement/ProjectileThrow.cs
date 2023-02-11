using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileThrow : IProjectileMovement
{
    Rigidbody rb;
    public Transform mTransform;
    public float speed;

    public ProjectileThrow(Rigidbody newRB, Transform newTransform, float newSpeed)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
        this.speed = newSpeed;
    }

    public void ProjectileMovement()
    {
        // rb.AddForce(mTransform.forward * speed, ForceMode.Force);
        rb.AddForce(mTransform.forward * speed + mTransform.up * speed, ForceMode.Impulse);
    }
}
