using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public static Rigidbody rb;
    Vector3 moveDirection;
    Vector3 rotateDirection;
    float speed;
    public float rotateSpeed;
    float vertical;
    float horizontal;

    public float driveForce = 17f;
    public float terminalVelocity = 100f;
    float drag;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        drag = driveForce / terminalVelocity;
    }

    // Update is called once per frame
    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        // moveDirection = transform.forward * vertical;
        rotateDirection =  transform.up * horizontal;

        if (horizontal == 0 && vertical == 0) {
            return;
        }
        // if (vertical != 0) {
        //     rb.AddForce(moveDirection * speed, ForceMode.Acceleration);
        // }
        if (horizontal != 0) {
            transform.Rotate(rotateDirection * rotateSpeed * Time.deltaTime, Space.World);
            // Debug.Log(transform.localRotation.eulerAngles.y);
        }
        if (GroundDetector.onSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * driveForce, ForceMode.Acceleration);
        }
    }

    void FixedUpdate()
    {
        
        speed = Vector3.Dot(rb.velocity, transform.forward);

        CalculatePropultion();
    }

    void CalculatePropultion()
    {
        // float rotationTorque = horizontal - rb.angularVelocity.y;
        // rb.AddRelativeTorque(0f, rotationTorque, 0f, ForceMode.VelocityChange);

        float sidewaysSpeed = Vector3.Dot(rb.velocity, transform.right);

        Vector3 sideFriction = -transform.right * (sidewaysSpeed/Time.fixedDeltaTime);

        rb.AddForce(sideFriction, ForceMode.Acceleration);

        if (vertical <= 0f) {
            rb.velocity *= .95f; // slowingVelFactor;
        }

        float propulsion = driveForce * vertical - drag * Mathf.Clamp(speed, 0f, terminalVelocity);
        rb.AddForce(transform.forward * propulsion, ForceMode.Acceleration);
    }

    public Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, GroundDetector.characterNormal).normalized;
    }
}
