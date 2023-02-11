using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public enum UPGRADE {
        INCREASE, 
        DECREASE
    }
    public UI_FP m_UI_FP;
    public VehicleMovement m_VehicleMovement;
    public UPGRADE TypeEngine = UPGRADE.INCREASE;
    public float EngineCost = 50f;
    public TMPro.TMP_Text EngineText;
    public UPGRADE TypeHandling = UPGRADE.INCREASE;
    public float AngleOfRollCost;
    public TMPro.TMP_Text m_Handling;
    public UPGRADE TypeBrake = UPGRADE.INCREASE;
    public float BrakeCost;
    public TMPro.TMP_Text BrakeText;
    public UPGRADE TypeProjCooldown = UPGRADE.DECREASE;
    public float ProjCooldownCost;
    public TMPro.TMP_Text ProjCooldownText;
    public UPGRADE TypeSpeed = UPGRADE.INCREASE;
    public float MaxSpeedCost;
    public TMPro.TMP_Text MaxSpeedText;
    public float UltimateCost;
    public TMPro.TMP_Text UltimateText;
    public GameObject m_UIShop;
    public GameObject m_UltimateUI;
    public CooldownManager buffCooldown;
    public CooldownManager nerfCooldown;
    public float tempCurrPartUpgrade;
	[System.Serializable]
    public class TempBoost
    {
        public string Name;                            //"BOOST"
        public float m_Amount;                         //amount to boost
        // public UPGRADE m_TempBoost = UPGRADE.INCREASE; //type nerf or buff?
        public CooldownManager cooldown;               //if cooldown.reset is true... do something.
        public float m_CooldownLength;                 //cooldown length
        // public CooldownManager nerfCooldown;

        public TempBoost(string name, float amount, CooldownManager cooldown, float cooldownLength)
        {
            this.Name = name;
            this.m_Amount = amount;
            this.cooldown = cooldown;
            this.m_CooldownLength = cooldownLength;
        }
    }
    public List<TempBoost> m_TempBoostList;
    public float m_TempBoostListSize = 0;
    public float m_EngineTempBoost = 0;
    public float m_BrakeTempBoost = 0;
    public float m_AngleOfRollTempBoost = 0;
    public float m_MaxSpeedTempBoost = 0;

    void Start()
    {
        EngineText.text = EngineCost.ToString();
        m_Handling.text = AngleOfRollCost.ToString();
        BrakeText.text = BrakeCost.ToString();
        ProjCooldownText.text = ProjCooldownCost.ToString();
        MaxSpeedText.text = MaxSpeedCost.ToString();
        UltimateText.text = UltimateCost.ToString();
        buffCooldown = new CooldownManager();
        nerfCooldown = new CooldownManager();
    }

    void Update()
    {
        buffCooldown.UpdateCooldown(buffCooldown);

        nerfCooldown.UpdateCooldown(nerfCooldown);

        //when cooldown is over reset parameter
        if (nerfCooldown.CDReset)
        {
            //cooldown is over
            Debug.Log("nerf cooldown over");
            nerfCooldown.CDReset = false;
        }

        if (MyPlayerInput.ShopMenuKey.WasPressedThisFrame() && !m_UIShop.activeInHierarchy)
        {
            m_UIShop.SetActive(true);
        } 
        else if (MyPlayerInput.ShopMenuKey.WasPressedThisFrame() && m_UIShop.activeInHierarchy)
        {
            m_UIShop.SetActive(false);
        }

        //return when list is empty
        if (m_TempBoostList.Count < 1) return;

        //check each elements cooldown
        UpdateListCooldown();

        //Check list size
        if (m_TempBoostList.Count == m_TempBoostListSize) return;
        //update reference list size to current list size
        m_TempBoostListSize = m_TempBoostList.Count;

        //Recalculate temp boost
        UpdateTempBoost();
    }

    public void UpgradeOnButtonPress(string Upgradable)
    {
        switch (Upgradable.ToUpper()) {
            case ("ENGINE"): 
                if (IsUpgradeMaxed(m_VehicleMovement.driveForceMultiplier, m_VehicleMovement.MaxDriveForceMultiplier)) break;
                if (m_UI_FP.EnoughFP(EngineCost)){
                    Debug.Log("Bought " + Upgradable);
                    m_VehicleMovement.driveForceMultiplier = DoIncrementMultiplier(m_VehicleMovement.driveForceMultiplier, m_VehicleMovement.IncrementEngineUpgrade);
                    DoUpgradePart(TypeEngine, m_VehicleMovement.driveForceBase, m_VehicleMovement.driveForceDefault, m_VehicleMovement.driveForceMultiplier);
                    m_UI_FP.DecreaseFP(EngineCost);
                }
                break;
            case ("SPEED"):
                if (IsUpgradeMaxed(m_VehicleMovement.terminalVelocityMultiplier, m_VehicleMovement.MaxTerminalVelocityMultiplier)) break;
                if (m_UI_FP.EnoughFP(MaxSpeedCost)){
                    Debug.Log("Bought " + Upgradable);
                    m_VehicleMovement.terminalVelocityMultiplier = DoIncrementMultiplier(m_VehicleMovement.terminalVelocityMultiplier, m_VehicleMovement.IncrementTerminalVelocityUpgrade);
                    DoUpgradePart(TypeSpeed, m_VehicleMovement.terminalVelocityBase, m_VehicleMovement.terminalVelocityDefault, m_VehicleMovement.terminalVelocityMultiplier);
                    m_UI_FP.DecreaseFP(MaxSpeedCost);
                }
                break;
            case ("HANDLING"):
                if (IsUpgradeMaxed(m_VehicleMovement.angleOfRollMultiplier, m_VehicleMovement.MaxAngleOfRollMultiplier)) break;
                if (m_UI_FP.EnoughFP(AngleOfRollCost))
                {
                    Debug.Log("Bought " + Upgradable);
                    m_VehicleMovement.angleOfRollMultiplier = DoIncrementMultiplier(m_VehicleMovement.angleOfRollMultiplier, m_VehicleMovement.IncrementAngleOfRollUpgrade);
                    DoUpgradePart(TypeHandling, m_VehicleMovement.angleOfRollBase, m_VehicleMovement.angleOfRollDefault, m_VehicleMovement.angleOfRollMultiplier);
                    m_UI_FP.DecreaseFP(AngleOfRollCost);
                }
                break;
            case ("BRAKE"):
                if (IsUpgradeMaxed(m_VehicleMovement.brakeMultiplier, m_VehicleMovement.MaxBrakeMultiplier)) break;
                if (m_UI_FP.EnoughFP(BrakeCost))
                {
                    Debug.Log("Bought " + Upgradable);
                    m_VehicleMovement.brakeMultiplier = DoIncrementMultiplier(m_VehicleMovement.brakeMultiplier, m_VehicleMovement.IncrementBrakeUpgrade);
                    DoUpgradePart(TypeBrake, m_VehicleMovement.brakingVelFactorBase, m_VehicleMovement.brakingVelFactorDefault, m_VehicleMovement.brakeMultiplier);
                    m_UI_FP.DecreaseFP(BrakeCost);
                }
                break;
            case ("PROJ_COOLDOWN"):
                if (IsUpgradeMaxed(m_VehicleMovement.angleOfRollMultiplier, m_VehicleMovement.MaxAngleOfRollMultiplier)) break;
                if (m_UI_FP.EnoughFP(AngleOfRollCost))
                {
                    Debug.Log("Bought " + Upgradable);
                    m_VehicleMovement.angleOfRollMultiplier = DoIncrementMultiplier(m_VehicleMovement.angleOfRollMultiplier, m_VehicleMovement.IncrementProjCDUpgrade);
                    DoUpgradePart(TypeProjCooldown, m_VehicleMovement.angleOfRoll, m_VehicleMovement.angleOfRollDefault, m_VehicleMovement.angleOfRollMultiplier);
                    m_UI_FP.DecreaseFP(AngleOfRollCost);
                }
                break;
            case ("ULTIMATE"):
                if (m_UltimateUI.activeInHierarchy) break;
                if (m_UI_FP.EnoughFP(AngleOfRollCost))
                {
                    Debug.Log("Bought " + Upgradable);
                    m_UltimateUI.SetActive(true);
                }
                break;
            //add more cases below..

        }
    }

    public bool IsUpgradeMaxed(float CURR_UPGRADE_MULTIPLIER, float MAX_UPGRADE_MULTIPLIER)
	{
		if (CURR_UPGRADE_MULTIPLIER >= MAX_UPGRADE_MULTIPLIER) 
		{
			return true;
		}
		return false;
	}

	public float DoIncrementMultiplier(float CURR_UPGRADE_MULTIPLIER, float INCREMENT_UPGRADE)
	{
		CURR_UPGRADE_MULTIPLIER += INCREMENT_UPGRADE;
		return CURR_UPGRADE_MULTIPLIER;
	}

    public void DoUpgradePart(UPGRADE m_UPGRADE, float PART_UPGRADE, float DEFAULT_UPGRADE, float CURR_UPGRADE_MULTIPLIER)
    {
        switch (m_UPGRADE) {
            case (UPGRADE.INCREASE):
                DoIncreaseUpgradePart(PART_UPGRADE, DEFAULT_UPGRADE, CURR_UPGRADE_MULTIPLIER);
                break;
            case (UPGRADE.DECREASE):
                DoDecreaseUpgradePart(PART_UPGRADE, DEFAULT_UPGRADE, CURR_UPGRADE_MULTIPLIER);
                break;
        }
    }

	public void DoIncreaseUpgradePart(float PART_UPGRADE, float DEFAULT_UPGRADE, float CURR_UPGRADE_MULTIPLIER)
	{
		Debug.Log(CURR_UPGRADE_MULTIPLIER);
		PART_UPGRADE = DEFAULT_UPGRADE + (DEFAULT_UPGRADE * CURR_UPGRADE_MULTIPLIER);
	}

    public void DoDecreaseUpgradePart(float PART_UPGRADE, float DEFAULT_UPGRADE, float CURR_UPGRADE_MULTIPLIER)
	{
		Debug.Log(CURR_UPGRADE_MULTIPLIER);
		PART_UPGRADE = DEFAULT_UPGRADE - (DEFAULT_UPGRADE * CURR_UPGRADE_MULTIPLIER);
	}

    public void DoTempBuff(float PART_UPGRADE /*ex.) m_VehicleMovement.angleOfRoll*/,  float DEFAULT_UPGRADE, float CURR_UPGRADE_MULTIPLIER)
    {
        tempCurrPartUpgrade = PART_UPGRADE;
		PART_UPGRADE = DEFAULT_UPGRADE + (DEFAULT_UPGRADE * CURR_UPGRADE_MULTIPLIER);
        if (!nerfCooldown.CDReset)
        {
            buffCooldown.CDStart(1f);
        }
    }

    public void DoTempNerf(float PART_UPGRADE, float DEFAULT_UPGRADE, float CURR_UPGRADE_MULTIPLIER)
    {
        tempCurrPartUpgrade = PART_UPGRADE;
		PART_UPGRADE = DEFAULT_UPGRADE - (DEFAULT_UPGRADE * CURR_UPGRADE_MULTIPLIER);
        if (!buffCooldown.CDReset)
        {
            nerfCooldown.CDStart(1f);
        }
    }

    public void AddUpgradeTempBoost(string Upgrade, float amount, float cooldownLength)
    {
        var cooldown = new CooldownManager();
        var tempBoost = new UpgradeManager.TempBoost("ENGINE", amount, cooldown, cooldownLength);
        m_TempBoostList.Add(tempBoost);
    }

    public void UpdateListCooldown()
    {
        for(var i = 0; i < m_TempBoostList.Count; i++) //TempBoost boost in m_TempBoostList)
        {
            if (m_TempBoostList[i].cooldown.CDReset)
            {
                Debug.Log("Reset");
                RemoveUpdateTempBoost(m_TempBoostList[i].Name, m_TempBoostList[i].m_Amount);
                m_TempBoostList.RemoveAt(i);
                break;
            }
            if (m_TempBoostList[i].cooldown.CDStarted) 
            {
                m_TempBoostList[i].cooldown.UpdateCooldown(m_TempBoostList[i].cooldown);
            } 
            else
            {
                Debug.Log("Started");
                m_TempBoostList[i].cooldown.CDStart(m_TempBoostList[i].m_CooldownLength);
            }
        }
    }

    public void UpdateTempBoost()
    {
        Debug.Log("Recalculate buff and nerf");
        m_EngineTempBoost = 0f;
        m_BrakeTempBoost = 0;
        m_AngleOfRollTempBoost = 0;
        m_MaxSpeedTempBoost = 0;
        foreach (TempBoost boost in m_TempBoostList)
        {
            AddUpdateTempBoost(boost.Name, boost.m_Amount);
        }
        RecalculateUpdateTempBoost();
        
    }

    public void AddUpdateTempBoost(string UPGRADE_NAME, float UPGRADE_AMOUNT)
    {
        switch (UPGRADE_NAME.ToUpper()) {
            case ("ENGINE"): 
                // m_VehicleMovement.driveForceTempBoost += UPGRADE_AMOUNT;
                m_EngineTempBoost += UPGRADE_AMOUNT;
                break;
            case ("SPEED"):
                m_VehicleMovement.terminalVelocityTempBoost += UPGRADE_AMOUNT;
                break;
            case ("HANDLING"):
                m_VehicleMovement.angleOfRollTempBoost += UPGRADE_AMOUNT;
                break;
            case ("BRAKE"):
                m_VehicleMovement.brakingVelTempBoost += UPGRADE_AMOUNT;
                break;
        }
    }

    public void RemoveUpdateTempBoost(string UPGRADE_NAME, float UPGRADE_AMOUNT)
    {
        switch (UPGRADE_NAME.ToUpper()) {
            case ("ENGINE"): 
                m_VehicleMovement.driveForceTempBoost -= UPGRADE_AMOUNT;
                Debug.Log(m_VehicleMovement.driveForceTempBoost);
                break;
            case ("SPEED"):
                m_VehicleMovement.terminalVelocityTempBoost -= UPGRADE_AMOUNT;
                break;
            case ("HANDLING"):
                m_VehicleMovement.angleOfRollTempBoost -= UPGRADE_AMOUNT;
                break;
            case ("BRAKE"):
                m_VehicleMovement.brakingVelTempBoost -= UPGRADE_AMOUNT;
                break;
        }
    }

    public void RecalculateUpdateTempBoost()
    {
        if (m_EngineTempBoost != m_VehicleMovement.driveForceTempBoost)
        {
            Debug.Log(m_VehicleMovement.driveForceTempBoost);
            m_VehicleMovement.driveForceTempBoost = m_EngineTempBoost;
        }
        if (m_BrakeTempBoost != m_VehicleMovement.brakingVelTempBoost)
        {
            Debug.Log(m_VehicleMovement.brakingVelTempBoost);
            m_VehicleMovement.brakingVelTempBoost = m_BrakeTempBoost;
        }
        if (m_AngleOfRollTempBoost != m_VehicleMovement.angleOfRollTempBoost)
        {
            Debug.Log(m_VehicleMovement.angleOfRollTempBoost);
            m_VehicleMovement.angleOfRollTempBoost = m_AngleOfRollTempBoost;
        }
        if (m_MaxSpeedTempBoost != m_VehicleMovement.terminalVelocityTempBoost)
        {
            Debug.Log(m_VehicleMovement.terminalVelocityTempBoost);
            m_VehicleMovement.terminalVelocityTempBoost = m_MaxSpeedTempBoost;
        }
    }
}
