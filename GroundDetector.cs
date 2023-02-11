using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    public static Vector3 characterNormal;
    public Quaternion targetRotation;
    public static bool onSlope { get; set; }
    public float myGravity = 9.8f;
    public float rayDistance = 3.0f;
    public float angularSpeed = 10.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        OnSlopeChecker();
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, rayDistance)
            // || Physics.Raycast(transform.position, Vector3.up, out hit, rayDistance)
            // || Physics.Raycast(transform.position, -Vector3.right, out hit, rayDistance)
            // || Physics.Raycast(transform.position, Vector3.right, out hit, rayDistance)
            // || Physics.Raycast(transform.position, -Vector3.forward, out hit, rayDistance)
            // || Physics.Raycast(transform.position, Vector3.forward, out hit, rayDistance)
            )
        {
            if (hit.distance <= rayDistance && hit.transform.name != "Player") {
                Vector3 projection = Vector3.ProjectOnPlane(transform.forward, hit.normal);
                Quaternion rotation = Quaternion.LookRotation(projection, hit.normal);

                PlayerMovement.rb.MoveRotation(Quaternion.Lerp(PlayerMovement.rb.rotation, rotation, Time.deltaTime * 10f));


                // Quaternion rotation = Quaternion.AngleAxis(, transform.up);
                // Vector3 currentRotation = hit.transform.rotation.eulerAngles;
                // transform.rotation = rotation;


                // Vector3 target = hit.transform.rotation.eulerAngles;  
                // Quaternion rot = Quaternion.Euler(target.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); 
                // Debug.Log(rot.x);
                // transform.rotation = rot; 

                // transform.localRotation = Quaternion.Euler(hit.transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

                // characterNormal = hit.normal;

                // targetRotation = hit.transform.root.localRotation;
                // targetRotation.x = transform.localEulerAngles.x;
                // targetRotation.y = Mathf.Abs(transform.root.eulerAngles.y);
                // targetRotation.z = Mathf.Abs(transform.root.eulerAngles.z);
                // transform.root.localRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, angularSpeed);

                // Transform target = hit.transform;
                // float angle = Quaternion.Angle(transform.rotation, target.rotation);
                // Debug.Log(angle);
                // if (angle != 90f) {
                //     transform.rotation = Quaternion.AngleAxis(90, Vector3.right);
                // }

                // transform.root.localRotation = Quaternion.FromToRotation(transform.root.localRotation.eulerAngles, new Vector3(transform.root.localRotation.x, hit.transform.localRotation.y, transform.root.localRotation.z));

                
                // transform.root.localRotation = Quaternion.FromToRotation(Vector3.up, hit.transform.up);
                // Debug.Log(transform.localRotation.eulerAngles.y);
                // Debug.Log(transform.root.rotation.z);
                

                // transform.root.localRotation.eulerAngles.x = hit.normal.localRotation.eulerAngles.x;
                
                //Add gravity here

                // Debug.Log(hit.transform.name);
                //DO NOT USE LINE BELOW 
                // transform.normal = hit.normal;
            }
        } 

        PlayerMovement.rb.AddForce(transform.up * myGravity * PlayerMovement.rb.mass * -1, ForceMode.Force);
    }

    public void OnSlopeChecker()
    {
        RaycastHit slopeHit;

        if (Physics.Raycast(transform.position, -Vector3.up, out slopeHit, 10.0f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            onSlope = angle < 45;
        }

        onSlope = false;
    }

}

