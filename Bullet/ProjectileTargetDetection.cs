using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTargetDetection : MonoBehaviour
{
    // [HideInInspector]
    public GameObject target;
    public bool m_LockedOn;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.name != "Me" && other.tag == "Player" && other.tag != "Projectile")
        {
            if (!m_LockedOn)
            {
                target = other.gameObject;
                m_LockedOn = true;
            }
        }
    }

    void OnDisable()
    {
        target = null;
        m_LockedOn = false;
    }
}
