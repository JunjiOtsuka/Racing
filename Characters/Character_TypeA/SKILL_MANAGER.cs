using System;
using UnityEngine;
using Photon.Pun;

public class SKILL_MANAGER : MonoBehaviour, IUltimate, ITactical
{
    public MyPlayerInput input;
    public Action TacticalAction;
    public Action UltimateAction;
    public Action ItemAction;
    public ITactical ITactical;
    public IUltimate IUltimate;
    public UITactical UITactical;
    public UIUltimate UIUltimate;
    public UIItem UIItem;
    public ProjectileObjectPooler m_POP;
    public SelectedComboManager m_SCM;
    public VehicleMovement m_VehicleMovement;
    public UpgradeManager m_UpgradeManager;
    public float timer;
    public float speed;
    public float frequency;
    public float amplitude;
    public float test = 3;

	public PhotonView m_PV;
    public GameObject bulletPrefab;

    void Awake()
    {
		// if (!m_PV.IsMine) return;
        m_POP = GetComponentInChildren<ProjectileObjectPooler>();
    }

    void Start()
    {
		// if (!m_PV.IsMine) return;
        TestUpgrade();
        TestUpgrade();
    }

    void OnEnable()
    {
		// if (!m_PV.IsMine) return;
        TacticalAction += Tactical;
        UltimateAction += Ultimate;
        ItemAction += Item;
    }

    // Update is called once per frame
    void Update()
    {
		if (!m_PV.IsMine) return;
        if (MyPlayerInput.TacticalKey.IsPressed() && !UITactical.InCooldown)
        // if (MyPlayerInput.TacticalKey.WasPressedThisFrame())
        {
            TacticalAction?.Invoke();
        }

        if (MyPlayerInput.UltimateKey.IsPressed() && !UIUltimate.InCooldown)
        {
            UltimateAction?.Invoke();
        }

        if (MyPlayerInput.ItemKey.IsPressed() && !UIItem.InCooldown)
        {
            ItemAction?.Invoke();
        }
        
        //when cooldown is over reset parameter
        if (m_UpgradeManager.buffCooldown.CDReset)
        {
            Debug.Log("Nerf temp buff");
            DoItemNerf();
            m_UpgradeManager.buffCooldown.CDReset = false;
        }
    }
    
    public void Tactical()
    {
        //spawn projectile
        // m_PV.RPC("TacticalSpawnProjectile", RpcTarget.All);
        TacticalSpawnProjectile();
        //Update UI here
        UITactical.CDManager();
    }

    [PunRPC]
    public void TacticalSpawnProjectile() 
    {
        // avoid double inputs by checking cooldown

        //do wonders here
        Debug.Log("Tact");

        if (m_SCM.GET_MOVEMENT == ProjectileEnum.MOVE_TYPE.NORMAL
        && m_SCM.GET_BEHAVIOR == ProjectileEnum.BEHAVIOR_TYPE.NORMAL) 
        {
            // var refProj = Instantiate(bulletPrefab, m_POP.transform.position, m_POP.transform.rotation);
            // refProj.GetComponent<ProjectileManager>().PhotonID = m_PV.ViewID;
            
            //Spawn projectile from object pooling
            // m_POP.SpawnFromPool("BASE_NORMAL");

            // var currPro = m_POP.GetProjectile("BASE_NORMAL");
            
            // var projRB = m_POP.GetProjectile("BASE_NORMAL").GetComponent<Rigidbody>();
            // m_PV.RPC("SpawnTacticalProjectile", RpcTarget.All, projRB.position, projRB.rotation, m_POP.transform.position, m_POP.transform.rotation);
            m_PV.RPC("SpawnTacticalProjectile", RpcTarget.All, "BASE_NORMAL", m_POP.transform.position, m_POP.transform.rotation);
            
            // if (currPro != null)
            // {
            //     currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
            //     currPro.transform.position = m_POP.transform.position;
            //     currPro.transform.rotation = m_POP.transform.rotation;
            //     currPro.SetActive(true);
            //     currPro.GetComponent<ProjectileManager>().GET_PM();
            //     currPro.GetComponent<ProjectileManager>().DO_PM();
            // }
        }

        if (m_SCM.GET_MOVEMENT == ProjectileEnum.MOVE_TYPE.NORMAL
        && m_SCM.GET_BEHAVIOR == ProjectileEnum.BEHAVIOR_TYPE.HOMING) 
        {
            // var currPro = m_PV.RPC("GetProjectile", RpcTarget.All, "BASE_HOMING");
            var currPro = m_POP.GetProjectile("BASE_HOMING");
            
            // var currPro = m_POP.GetPooledObject();
            if (currPro != null)
            {
                currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
                currPro.transform.position = m_POP.transform.position;
                currPro.transform.rotation = m_POP.transform.rotation;
                currPro.SetActive(true);
                currPro.GetComponent<ProjectileManager>().GET_PM();
                currPro.GetComponent<ProjectileManager>().DO_PM();
            }
        }

        if (m_SCM.GET_MOVEMENT == ProjectileEnum.MOVE_TYPE.STATIONARY
        && m_SCM.GET_BEHAVIOR == ProjectileEnum.BEHAVIOR_TYPE.NORMAL) 
        {
            // var currPro = m_PV.RPC("GetProjectile", RpcTarget.All, "STATIONARY_NORMAL");
            var currPro = m_POP.GetProjectile("STATIONARY_NORMAL");
            
            // var currPro = m_POP.GetPooledObject();
            if (currPro != null)
            {

                currPro.GetComponent<ProjectileHover>().driveForce = 0f;
                currPro.GetComponent<CapsuleCollider>().material = null;
                // currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
                currPro.GetComponent<Rigidbody>().velocity = Vector3.zero;
                currPro.transform.position = m_POP.projectileHindSpawner.transform.position;
                currPro.transform.rotation = m_POP.transform.rotation;
                currPro.SetActive(true);
                currPro.GetComponent<ProjectileManager>().GET_PM();
                currPro.GetComponent<ProjectileManager>().DO_PM();
            }
        }

        // projectile type is wave
        // if (m_SCM.GET_MOVEMENT == ProjectileEnum.MOVE_TYPE.WAVE) 
        // {
        //     //Get projectiles
        //     var currPro = m_POP.GetProjectile();

        //     if (currPro != null)
        //     {
        //         currPro.gameObject.name = "middle";
        //         currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
        //         currPro.transform.position = m_POP.transform.position;
        //         currPro.transform.rotation = m_POP.transform.rotation;
        //         currPro.SetActive(true);
        //         currPro.GetComponent<ProjectileManager>().GET_PM();
        //         currPro.GetComponent<ProjectileManager>().DO_PM();
        //         // m_POP.projectileQueue.Enqueue(currPro);
        //     }

        //     //if wave
        //     // GameObject leftProjectile = m_POP.GetProjectile();
        //     // if (leftProjectile) {
        //     //     leftProjectile.GetComponent<ProjectileManager>().enabled = false;
        //     //     leftProjectile.gameObject.name = "left";
        //     //     leftProjectile.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
        //     //     leftProjectile.transform.position = m_POP.transform.position;
        //     //     leftProjectile.transform.rotation = m_POP.transform.rotation;
        //     //     leftProjectile.SetActive(true);

        //     //     timer += Time.deltaTime;
        //     //     // float x = Mathf.Sin(timer * frequency) * amplitude; 
        //     //     float x = Mathf.Cos(timer * frequency) * amplitude;
        //     //     float y = Mathf.Sin(timer * frequency) * amplitude;
        //     //     Vector3 wave = Vector3.Cross(new Vector3 (0f, -x, 0f), leftProjectile.transform.forward);
        //     //     leftProjectile.GetComponent<Rigidbody>().AddForce(leftProjectile.transform.forward * speed + wave * amplitude, ForceMode.Force);
        //     //     // rb.AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Impulse);
        //     // }

        //     // GameObject rightProjectile = m_POP.GetProjectile();
        //     // if (rightProjectile) {
        //     //     rightProjectile.GetComponent<ProjectileManager>().enabled = false;
        //     //     rightProjectile.gameObject.name = "right";
        //     //     rightProjectile.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
        //     //     rightProjectile.transform.position = m_POP.transform.position;
        //     //     rightProjectile.transform.rotation = m_POP.transform.rotation;
        //     //     rightProjectile.SetActive(true);

        //     //     timer += Time.deltaTime;
        //     //     // float x = Mathf.Sin(timer * frequency) * amplitude; 
        //     //     float x = Mathf.Cos(timer * frequency) * amplitude;
        //     //     float y = Mathf.Sin(timer * frequency) * amplitude;
        //     //     Vector3 wave = Vector3.Cross(new Vector3 (0f, x, 0f), rightProjectile.transform.forward);
        //     //     rightProjectile.GetComponent<Rigidbody>().AddForce(rightProjectile.transform.forward * speed + wave * amplitude, ForceMode.Force);
        //     //     // rb.AddForce(mTransform.forward * speed + wave * amplitude, ForceMode.Impulse);
        //     // }
        // }
    }

    [PunRPC]
    void SpawnTacticalProjectile(string PlayerProjectileTag, Vector3 ProjectilePosition, Quaternion ProjectileRotation)
    {
        var pooledProjectile = GetComponentInChildren<ProjectileObjectPooler>().GetProjectile(PlayerProjectileTag);
        if (!pooledProjectile) {
            Debug.Log("Out of projectile");
            return;
        }
        pooledProjectile.GetComponent<Rigidbody>().velocity = transform.root.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
        pooledProjectile.transform.position = ProjectilePosition;
        pooledProjectile.transform.rotation = ProjectileRotation;
        pooledProjectile.SetActive(true);
    }

    public void Ultimate()
    {
        //spawn projectile
        m_PV.RPC("UltimateSpawnProjectile", RpcTarget.All);
        // UltimateSpawnProjectile();
        //Updates cooldown UI here
        UIUltimate.CDManager();
    }

    [PunRPC]
    public void UltimateSpawnProjectile() 
    {
        //spawn projectile by type

        //*****************************FOR CHAR TYPE 1*******************************************************
        if (m_SCM.SET_CHAR_TYPE == CharTypeEnum.TYPE1)
        {
            // var currPro = m_PV.RPC("GetProjectile", RpcTarget.All, "ULTIMATE_PROJECTILE_TYPE");
            var currPro = m_POP.GetProjectile("ULTIMATE_PROJECTILE_TYPE");
            
            // var currPro = m_POP.GetPooledObject();
            if (currPro != null)
            {
                currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
                currPro.transform.position = m_POP.transform.position;
                currPro.transform.rotation = m_POP.transform.rotation;
                currPro.SetActive(true);
                currPro.GetComponent<ProjectileManager>().GET_PM();
                currPro.GetComponent<ProjectileManager>().DO_PM();
                // m_POP.projectileQueue.Enqueue(currPro);
            }
        }

        //*****************************FOR CHAR TYPE 2*******************************************************
        if (m_SCM.SET_CHAR_TYPE == CharTypeEnum.TYPE2)
        {
            // var currPro = m_PV.RPC("GetProjectile", RpcTarget.All, "ULTIMATE_PROJECTILE_TYPE");
            var currPro = m_POP.GetProjectile("ULTIMATE_PROJECTILE_TYPE");
            
            // var currPro = m_POP.GetPooledObject();
            if (currPro != null)
            {
                currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
                currPro.transform.position = m_POP.transform.position;
                currPro.transform.rotation = m_POP.transform.rotation;
                currPro.SetActive(true);
                currPro.GetComponent<ProjectileManager>().GET_PM();
                currPro.GetComponent<ProjectileManager>().DO_PM();
                // m_POP.projectileQueue.Enqueue(currPro);
            }
        }

        //*****************************FOR CHAR TYPE 3*******************************************************
        if (m_SCM.SET_CHAR_TYPE == CharTypeEnum.TYPE3)
        {
            // var currPro = m_PV.RPC("GetProjectile", RpcTarget.All, "ULTIMATE_PROJECTILE_TYPE");
            var currPro = m_POP.GetProjectile("ULTIMATE_PROJECTILE_TYPE");
            
            // var currPro = m_POP.GetPooledObject();
            if (currPro != null)
            {
                currPro.GetComponent<Rigidbody>().velocity = transform.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
                currPro.transform.position = m_POP.transform.position;
                currPro.transform.rotation = m_POP.transform.rotation;
                currPro.SetActive(true);
                currPro.GetComponent<ProjectileManager>().GET_PM();
                currPro.GetComponent<ProjectileManager>().DO_PM();
                // m_POP.projectileQueue.Enqueue(currPro);
            }
        }
    }

    public void Item()
    {
        //spawn projectile
        DoItemBuff();
        //Update UI here
        UIItem.CDManager();
    }

    public void DoItemBuff() 
    {
        //buff drive force
        if (m_SCM.SET_ITEM_TYPE == ItemTypeEnum.TYPE1)
        {
            m_UpgradeManager.DoTempBuff(m_VehicleMovement.driveForce, m_VehicleMovement.driveForceDefault, m_VehicleMovement.driveForceMultiplier);
        }
        //buff AngleOfRoll
        else if (m_SCM.SET_ITEM_TYPE == ItemTypeEnum.TYPE2)
        {
            m_UpgradeManager.DoTempBuff(m_VehicleMovement.angleOfRoll, m_VehicleMovement.angleOfRollDefault, m_VehicleMovement.angleOfRollMultiplier);
        }
        //buff Terminal Velocity
        else if (m_SCM.SET_ITEM_TYPE == ItemTypeEnum.TYPE3)
        {
            m_UpgradeManager.DoTempBuff(m_VehicleMovement.terminalVelocity, m_VehicleMovement.terminalVelocityDefault, m_VehicleMovement.terminalVelocityMultiplier);
        }
    }

    public void DoItemNerf() 
    {
        //Nerf drive force
        if (m_SCM.SET_ITEM_TYPE == ItemTypeEnum.TYPE1)
        {
            m_UpgradeManager.DoTempNerf(m_VehicleMovement.driveForce, m_VehicleMovement.driveForceDefault, m_VehicleMovement.driveForceMultiplier);
        }
        //Nerf AngleOfRoll
        else if (m_SCM.SET_ITEM_TYPE == ItemTypeEnum.TYPE2)
        {
            m_UpgradeManager.DoTempNerf(m_VehicleMovement.angleOfRoll, m_VehicleMovement.angleOfRollDefault, m_VehicleMovement.angleOfRollMultiplier);
        }
        //Nerf Terminal Velocity
        else if (m_SCM.SET_ITEM_TYPE == ItemTypeEnum.TYPE3)
        {
            m_UpgradeManager.DoTempNerf(m_VehicleMovement.terminalVelocity, m_VehicleMovement.terminalVelocityDefault, m_VehicleMovement.terminalVelocityMultiplier);
        }
    }

    public void TestUpgrade()
    {
        m_UpgradeManager.AddUpgradeTempBoost("ENGINE", 5f, 2f);
    }

    void OnDisable()
    {
        TacticalAction -= Tactical;
        UltimateAction -= Ultimate;
        ItemAction -= Item;
    }
}
