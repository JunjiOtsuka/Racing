using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class SelectedComboManager : MonoBehaviour
{
    /***************CHARACTER********************/

    [Header("Character")]
    public CharTypeEnum SET_CHAR_TYPE;
    [HideInInspector]
	public CharacterSOHolder m_CharacterSOHolder;
    [HideInInspector]
    public VehicleMovement GetVehicleMovement;

    public void SetCharByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        SET_CHAR_TYPE = (CharTypeEnum)ParsedClickButtonName;

        // GetVehicleMovement.m_CharSO = GetCharType();

        var SelectedCharSO = GetCharType();

        Debug.Log("Change character material & mesh in Selected Combo Manager Script " + SelectedCharSO.CharacterPrefabName);
    }

    public CharacterSO GetCharType()
    {
        switch(SET_CHAR_TYPE)
        {
            case CharTypeEnum.TYPE1:
                return m_CharacterSOHolder.Character_Type1;
            case CharTypeEnum.TYPE2:
                return m_CharacterSOHolder.Character_Type2;
            case CharTypeEnum.TYPE3:
                return m_CharacterSOHolder.Character_Type3;
        }
        return null;
    }

    /***************CAR********************/
    [Header("Car")]
    public CarTypeEnum SET_CAR_TYPE;
    [HideInInspector]
	public CarSOHolder m_CarSOHolder;

    public void SetCarByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        SET_CAR_TYPE = (CarTypeEnum)ParsedClickButtonName;

        var SelectedCarSO = GetCarType();

        if (!GetVehicleMovement) { return; }

        //update vehicle movements default values to the new car SO.
        GetVehicleMovement.driveForce = SelectedCarSO.DriveForce;
        GetVehicleMovement.angleOfRoll = SelectedCarSO.AngleOfRoll;
        GetVehicleMovement.terminalVelocity = SelectedCarSO.TerminalVelocity;

        Debug.Log("Change car material & mesh in Selected Combo Manager Script ");
    }

    public CarSO GetCarType()
    {
        switch(SET_CAR_TYPE)
        {
            case CarTypeEnum.TYPE1:
                return m_CarSOHolder.Car_Type1;
            case CarTypeEnum.TYPE2:
                return m_CarSOHolder.Car_Type2;
            case CarTypeEnum.TYPE3:
                return m_CarSOHolder.Car_Type3;
        }
        return null;
    }

    /***************ITEM**************************/
    
    [Header("Item")]
    public ItemTypeEnum SET_ITEM_TYPE;
    [HideInInspector]
	public ItemSOHolder m_ItemSOHolder;

    public void SetItemByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        SET_ITEM_TYPE = (ItemTypeEnum)ParsedClickButtonName;

        var SelectedItemSO = GetItemType();

        if (!GetVehicleMovement) { return; }

        //update vehicle movements default values to the new Item SO.
        GetVehicleMovement.driveForce = SelectedItemSO.DriveForce;
        GetVehicleMovement.angleOfRoll = SelectedItemSO.AngleOfRoll;
        GetVehicleMovement.terminalVelocity = SelectedItemSO.TerminalVelocity;

        Debug.Log("Change Item material & mesh in Selected Combo Manager Script ");
    }

    public ItemSO GetItemType()
    {
        switch(SET_ITEM_TYPE)
        {
            case ItemTypeEnum.TYPE1:
                return m_ItemSOHolder.Item_Type1;
            case ItemTypeEnum.TYPE2:
                return m_ItemSOHolder.Item_Type2;
            case ItemTypeEnum.TYPE3:
                return m_ItemSOHolder.Item_Type3;
        }
        return null;
    }

    /***************PROJECTILE********************/
    [Header("Projectile")]
    public ProjectileEnum.BEHAVIOR_TYPE GET_BEHAVIOR;
    public ProjectileEnum.MOVE_TYPE GET_MOVEMENT;
    public float SET_PROJECTILE_SPEED = 10f;
    [HideInInspector]
    ProjectileObjectPooler m_POP;
    public MySceneManager m_MySceneManager;
    public OnGameSceneLoadSO m_OnGameSceneLoadSO;
    public UpdateModelAndMesh m_UpdateModelAndMesh;
	public PhotonView m_PV;
    public bool IsModelUpdated;

    void OnEnable()
    {
        GetSelectedComboFromSO();
    }

    void Start()
    {
		if (m_PV == null) return;
        // if (!m_PV.IsMine) {
            //ADDED HERE
            // m_PV.RPC("RPCGetSelectedComboFromSO", RpcTarget.All/*BufferedViaServer*/);
            // return;
        // }
        m_POP = transform.root.GetComponentInChildren<ProjectileObjectPooler>();
        m_MySceneManager = GameObject.Find("SceneManager").GetComponent<MySceneManager>();

        //ADDED HERE
        m_PV.RPC("RPCGetSelectedComboFromSO", RpcTarget.All/*BufferedViaServer*/, m_PV.ViewID);
        IsModelUpdated = true;
        // GetSelectedComboFromSO();

        // UpdateComboStringViaCustomProperties();
    }

    public void SetProjectileBehaviorByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        GET_BEHAVIOR = (ProjectileEnum.BEHAVIOR_TYPE)ParsedClickButtonName;

        SetProjectileInProjectileManager();
    }

    public void SetProjectileMovementByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        GET_MOVEMENT = (ProjectileEnum.MOVE_TYPE)ParsedClickButtonName;

        SetProjectileInProjectileManager();
    }

    void SetProjectileInProjectileManager()
    {
        if (m_POP == null) return;
        if (m_POP.projectileList.Count <= 0) return;

        foreach (var projectile in m_POP.projectileList) 
        {
            projectile.GetComponent<ProjectileManager>().GET_BEHAVIOR = GET_BEHAVIOR;
            projectile.GetComponent<ProjectileManager>().GET_MOVEMENT = GET_MOVEMENT;
            projectile.GetComponent<ProjectileHover>().driveForce = SET_PROJECTILE_SPEED;
        }
    }

    /******SceneManagerSO*******/
    public void UpdateGameSceneSO()
    {
        m_MySceneManager.GameSceneSO._ChosenCharacter = SET_CHAR_TYPE;
        m_MySceneManager.GameSceneSO._ChosenCar = SET_CAR_TYPE;
        m_MySceneManager.GameSceneSO._ChosenItem = SET_ITEM_TYPE;
    }

    public void UpdateGameSceneSOStage()
    {
        m_MySceneManager.GameSceneSO._ChosenStage = EventSystem.current.currentSelectedGameObject.name;
    }

    public void GetSelectedComboFromSO()
    {
        if (!m_MySceneManager) return;
        SET_CHAR_TYPE = m_MySceneManager.GameSceneSO._ChosenCharacter;
        SET_CAR_TYPE = m_MySceneManager.GameSceneSO._ChosenCar;
        SET_ITEM_TYPE = m_MySceneManager.GameSceneSO._ChosenItem;
        // if (!m_UpdateModelAndMesh) return;
        // m_UpdateModelAndMesh.Load3DModel();
    }

    /****************My Combo in String*******************/
    public string Car;
    public string Character;
    public string Item;

    void UpdateComboStringViaCustomProperties()
    {
        // if (!PhotonNetwork.IsMasterClient) return;
        m_PV.RPC("RPCUpdateComboStringViaCustomProperties", RpcTarget.All, m_PV.ViewID);
    }
}
