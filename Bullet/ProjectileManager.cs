using UnityEngine;
using Photon.Pun;

public class ProjectileManager : MonoBehaviour
{
    /*********DO NOT EDIT THIS SCIRPT ********/ 
    /*********EDIT TYPE INITIALIZER OR SIMPLE FACTORY******/
    public GameObject m_Player;

    public ProjectileEnum.BEHAVIOR_TYPE GET_BEHAVIOR;
    public ProjectileEnum.MOVE_TYPE GET_MOVEMENT;

    //Target to chase
    ProjectileTargetDetection TargetDetected;

    //Projectile Behavior
    public IProjectileBehavior mPB;

    //Projectile Movement
    public IProjectileMovement mPM;

    //Grab the list of initialized movements and behaviors
    [HideInInspector]
    public ProjectileTypeInitializer mPTI;

    public ProjectileSimpleFactory mPSF;

    public int PhotonID;

    void Awake()
    {
        // m_PV = GetComponent<PhotonView>();
        mPTI = GetComponent<ProjectileTypeInitializer>();
        mPSF = new ProjectileSimpleFactory(mPB, mPM, mPTI);
    }

    [PunRPC]
    public void DeactivateProjectile()
    {
        transform.gameObject.SetActive(false);
    }

    public void GET_PM()
    {
        if (mPSF != null)
        mPM = mPSF.CheckMovement(GET_MOVEMENT);
    }

    public void DO_PM()
    {
        mPM?.ProjectileMovement();
    }

    // Update is called once per frame
    void Update()
    {
        //check current behavior
        mPB = mPSF.CheckBehavior(GET_BEHAVIOR);
        mPB?.ProjectileBehavior();
    }

    void FixedUpdate()
    {
        //checks current movement
        //also initializing projectile object pooling
        if (mPM == null) GET_PM();
        
        if (GET_MOVEMENT == ProjectileEnum.MOVE_TYPE.WAVE)
        {
            DO_PM();
        }
    }
}
