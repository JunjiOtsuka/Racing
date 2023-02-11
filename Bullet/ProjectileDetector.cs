using UnityEngine;
using Photon.Pun;

public class ProjectileDetector : MonoBehaviour
{
    public ProjectileEnum.SPECIAL_TYPE m_SpecType;

    [Header("Ultimate")]
    //Four types of buff available
    //"ENGINE", "SPEED", "HANDLING", "BRAKE"
    public string m_BuffUpgradeType = "ENGINE";
    public float m_BuffBoostAmount = 25f;
    public string m_NerfUpgradeType = "BRAKE";
    public float m_NerfBoostAmount = -25f;
    public float m_CooldownLength = 2f;

    public PhotonView m_PV;
    UI_FP GET_FP;
    public OnPlayerHitManager m_OnPlayerHitManager;

    void Start()
    {
        GET_FP = GameObject.Find("Me").GetComponentInChildren<UI_FP>();
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.name != "Me" && collision.gameObject.tag == "Player" && collision.gameObject.tag != "Projectile")
    //     {
    //         UI_FP GET_FP = GameObject.Find("Me").GetComponentInChildren<UI_FP>();
    //         GET_FP.IncreaseFP();
    //         switch (m_SpecType) {
    //             case (ProjectileEnum.SPECIAL_TYPE.TACTICAL):
    //                 // collision.transform.gameObject.GetComponent<OnPlayerHitManager>().PlayerUponHit();
    //                 var photonView = collision.gameObject.GetComponent<PhotonView>(); 
    //                 if (photonView)
    //                 {
    //                     Debug.Log("Sending DestroyTest RPC");
    //                     photonView.RPC("PlayerUponHit", RpcTarget.All);
    //                     // other.GetComponent<OnPlayerHitManager>().PlayerUponHit();
    //                 }
    //                 break;
    //             case (ProjectileEnum.SPECIAL_TYPE.ULTIMATE):
    //                 break;
    //         }

    //         this.gameObject.SetActive(false);
    //     }
    // }

    void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<PhotonView>()) { return; }
        if (other.GetComponent<PhotonView>().ViewID != this.transform.GetComponent<ProjectileManager>().PhotonID && other.gameObject.tag == "Player" && other.gameObject.tag != "Projectile")
        {
            
            if (GET_FP) {
                GET_FP.IncreaseFP();
            } 
            switch (m_SpecType) {
                case (ProjectileEnum.SPECIAL_TYPE.TACTICAL):
                        Debug.Log("Sending DestroyTest RPC");
                        other.GetComponent<OnPlayerHitManager>().PlayerUponHit();
                        // m_PV = other.GetComponent<PhotonView>();
                        // m_PV.RPC("PlayerUponHit", RpcTarget.All);
                    break;
                case (ProjectileEnum.SPECIAL_TYPE.ULTIMATE):
                    other.transform.gameObject.GetComponent<UpgradeManager>().AddUpgradeTempBoost(m_BuffUpgradeType, m_BuffBoostAmount, m_CooldownLength);
                    other.transform.gameObject.GetComponent<UpgradeManager>().AddUpgradeTempBoost(m_NerfUpgradeType, m_NerfBoostAmount, m_CooldownLength);
                    break;
            }
            this.gameObject.SetActive(false);
        }
    }

    [PunRPC]
    public void PlayerUponHit() 
    {
        m_OnPlayerHitManager.m_MyPlayerInput.enabled = false;
        m_OnPlayerHitManager.m_SkillManager.enabled = false;
        m_OnPlayerHitManager.m_CooldownManager.CDStart(m_OnPlayerHitManager.MaxKnockedTimer);
    }

    [PunRPC]
    public void ResetPlayerUponHit()
    {
        m_OnPlayerHitManager.m_MyPlayerInput.enabled = true;
        m_OnPlayerHitManager.m_SkillManager.enabled = true;
        m_OnPlayerHitManager.m_CooldownManager.CDReset = false; //set to false to stop infite call
    }
}
