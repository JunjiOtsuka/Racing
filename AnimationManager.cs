using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationManager : MonoBehaviour
{
    public Animator m_Animator;
	public PhotonView m_PV;
    public MyPlayerInput m_MyPlayerInput;

    // void OnEnable()
    // {
    //     m_Animator.applyRootMotion = true;
    //     m_Animator.applyRootMotion = false;
    // }

    // Update is called once per frame
    void Update()
    {
		if (!m_PV.IsMine) return;
        if (m_MyPlayerInput.thruster != 0 || m_MyPlayerInput.rudder != 0)
        {
            // m_PV.RPC("UpdateToStartDriving", RpcTarget.All);
            UpdateToStartDriving();
        } else if (m_MyPlayerInput.thruster == 0 || m_MyPlayerInput.rudder == 0){
            // m_PV.RPC("UpdateToStopDriving", RpcTarget.All);
            UpdateToStopDriving();
        }
    }

    void UpdateToStartDriving()
    {
        m_Animator.SetBool("IsDriving", true);
    }

    void UpdateToStopDriving()
    {
        m_Animator.SetBool("IsDriving", false);
    }
    
}
