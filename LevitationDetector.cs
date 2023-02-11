using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitationDetector : MonoBehaviour
{
    public float levitateForce = 4.5f;
    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 1.0f))
        {
            PlayerMovement.rb.AddForce(transform.up * levitateForce);
        } 
    }
}
