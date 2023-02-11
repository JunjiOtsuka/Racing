using UnityEngine;

public class ProjectileForwardMovement : IProjectileMovement
{
    Rigidbody rb;
    public Transform mTransform;
    public float speed;

    public ProjectileForwardMovement(Rigidbody newRB, Transform newTransform, float newSpeed)
    {
        this.rb = newRB;
        this.mTransform = newTransform;
        this.speed = newSpeed;
    }

    public void ProjectileMovement()
    {
        rb.AddForce(mTransform.forward * speed, ForceMode.Force);
        // rb.AddForce(mTransform.forward * speed, ForceMode.Impulse);
    }
}
