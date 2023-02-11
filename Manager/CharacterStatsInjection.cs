using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsInjection : MonoBehaviour
{
    // [HideInInspector]
    // public SKILL_MANAGER mSkill_Manager;
    // [HideInInspector]
    // public UITactical UITactical;
    // [HideInInspector]
    // public UIUltimate UIUltimate;
    // public ProjectileObjectPooler m_POP;
    // public ProjectileManager m_PM;

    // void Awake()
    // {
    //     m_POP = GetComponentInChildren<ProjectileObjectPooler>();
    //     // m_PM = GetComponentInChildren<ProjectileManager>();
    // }

    // void OnEnable()
    // {
    //     mSkill_Manager.TacticalAction += Tactical;
    //     mSkill_Manager.UltimateAction += Ultimate;
    // }
    
    // public void Tactical()
    // {
    //     // avoid double inputs by checking cooldown
    //     if (UITactical.InCooldown) return;

    //     //do wonders here
    //     Debug.Log("Tact");

    //     //Update UI here
    //     UITactical.CDManager();

    //     Debug.Log(mSkill_Manager.TacticalAction.Method.Name);

    //     //Instantiate projectile
    //     var currPro = m_POP.GetProjectile();
    //     // var currPro = m_POP.GetPooledObject();
    //     if (currPro != null)
    //     {
    //         currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
    //         currPro.transform.position = m_POP.transform.position;
    //         currPro.transform.rotation = m_POP.transform.rotation;
    //         currPro.SetActive(true);
    //         currPro.GetComponent<ProjectileManager>().GET_PM();
    //         currPro.GetComponent<ProjectileManager>().DO_PM();
    //         // m_POP.projectileQueue.Enqueue(currPro);
    //     }
    // }

    // public void Ultimate()
    // {
    //     // avoid double inputs by checking cooldown
    //     if (UIUltimate.InCooldown) return;

    //     //do ultimate here
    //     Debug.Log("Ult");

    //     //Update UI here
    //     UIUltimate.CDManager();
        
    //     //Instantiate ultimate

    // }

    // void Disable()
    // {
    //     mSkill_Manager.TacticalAction -= Tactical;
    //     mSkill_Manager.UltimateAction -= Ultimate;
    // }
}
