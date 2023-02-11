using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnPlayerHitManager : MonoBehaviour, IPlayerUponHit
{
    /****Remove Player inputs/controls for x sec. duration****/

    public VehicleMovement m_VehicleMovement;
    public MyPlayerInput m_MyPlayerInput;
    public SKILL_MANAGER m_SkillManager;
    public CooldownManager m_CooldownManager;
    public float MaxStartTimer = 5f;
    public float MaxKnockedTimer = 5f;
	public PhotonView m_PV;
    bool IsAtStartLine = false;

    void Start()
    {
        m_CooldownManager = new CooldownManager();
        PlayerUponStart();
    }

    void Update()
    {
        m_CooldownManager.UpdateCooldown(m_CooldownManager);
        if (m_CooldownManager.CDReset)
        {
            //Check if we can reset in-class variables.
            // photonView.RPC("ResetPlayerUponHit", RpcTarget.All);
            ResetPlayerUponHit();
        }
        if (UI_CountdownManager.instance.m_IsCountdownComplete && !IsAtStartLine)
        {
            Debug.Log("check");
            ResetPlayerUponHit();
            IsAtStartLine = true;
        }
    }

    //bad function name
    //function removes player input
    public void PlayerUponFinish() 
    {
        m_VehicleMovement.enabled = false;
        m_MyPlayerInput.enabled = false;
        m_SkillManager.enabled = false;
        // m_PV.RPC("RPCPlayerUponFinish", RpcTarget.All);
    }

    public void PlayerUponStart() 
    {
        Debug.Log("start");
        m_VehicleMovement.enabled = false;
        m_MyPlayerInput.enabled = false;
        m_SkillManager.enabled = false;
        // m_CooldownManager.CDStart(13f);
        // m_PV.RPC("RPCPlayerUponHit", RpcTarget.All);
    }

    //removes player input x seconds
    public void PlayerUponHit() 
    {
        m_VehicleMovement.enabled = false;
        m_MyPlayerInput.enabled = false;
        m_SkillManager.enabled = false;
        m_CooldownManager.CDStart(MaxKnockedTimer);
        // m_PV.RPC("RPCPlayerUponHit", RpcTarget.All);
    }

    //gives back player input
    public void ResetPlayerUponHit()
    {
        m_VehicleMovement.enabled = true;
        m_MyPlayerInput.enabled = true;
        m_SkillManager.enabled = true;
        m_CooldownManager.CDReset = false; //set to false to stop infite call
        // m_PV.RPC("RPCResetPlayerUponHit", RpcTarget.All);
    }

    
}
