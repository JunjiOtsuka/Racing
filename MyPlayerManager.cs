using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MyPlayerManager : MonoBehaviour
{
	public GameManager.PlayerData m_PlayerData;
    public PhotonView m_PV;
	public int RandomNumber;
	public bool IsSpawned;

	void OnEnable()
	{
	}

    // Start is called before the first frame update
    void Start()
    {
		
        // if (!PhotonNetwork.IsMasterClient) return;
        //using IsMine resulted with 2 elements one empty and IsMine.

        // m_PV = GetComponent<PhotonView>();
        if (m_PV.IsMine)
		{
			PhotonNetwork.LocalPlayer.CustomProperties["ViewID"] = m_PV.ViewID;
		}
		// RenamePlayerGO();
        CreateNewPlayerData();
        AddNewPlayerToPlayerDataList();
		SetSpawnPoint(m_PlayerData);       //Set spawn location of players
		SetPlayerPlacement();
		// CreateListOfNumber();              //Create list with numbers
        // RemoveDuplicates();
    }

    // Update is called once per frame
    void Update()
    {
        //master client tasks from here on below.
		foreach(GameManager.PlayerData Player in GameManager.instance.m_PlayerDataList)
		{
			Player.m_DistanceToCheckPoint = Player.m_GetClosestDistanceFromNextCheckPoint.m_GetDistanceToNextCheckpoint;
			Player.m_TotalCheckPointsPassed = Player.m_GetClosestDistanceFromNextCheckPoint.TotalCheckPoints;
			Player.m_UI_Placement.m_CurrentPlacement = Player.m_CurrentPlacement;
		}
    }

    void RenamePlayerGO()
    {
		transform.name = "Me";
    }

    void CreateNewPlayerData()
	{
		m_PlayerData = new GameManager.PlayerData();
		m_PlayerData.m_Player = this.gameObject;
		m_PlayerData.m_GetClosestDistanceFromNextCheckPoint = m_PlayerData.m_Player.GetComponentInChildren<GetClosestDistanceFromNextCheckPoint>();
		m_PlayerData.m_UI_Placement = m_PlayerData.m_Player.GetComponentInChildren<UI_PLACEMENT>();
	}

    void AddNewPlayerToPlayerDataList()
	{
		// m_PlayerDataList.Add(m_PlayerData);
        GameManager.instance.m_GameManagerPlayerData = m_PlayerData;
        // if (!GameManager.instance.m_GameManagerPlayerData.m_Player) return;
        // foreach(GameManager.PlayerData PlayerData in GameManager.instance.m_PlayerDataList)
        // {
        //     if(PlayerData.m_Player.name == GameManager.instance.m_GameManagerPlayerData.m_Player.name)
        //     {
        //         Debug.Log("List Already contains this info");
        //         return;
        //     }
        // }
        GetComponent<PhotonView>().RPC("RPCAddNewPlayerToPlayerDataList", RpcTarget.All);
	}

    void CreateListOfNumber()
	{
		//Create list of numbers
		
		// for (var i = 0; i < m_PlayerDataList.Count; i++) {
		for (var i = 0; i < GameManager.instance.totalPlayerInRoom; i++) {
			GameManager.instance.SetNumberList.Add(i);
		}
		//[0, 1, 2] count = 3
	}

    void SetSpawnPoint(GameManager.PlayerData Player)
	{
		if (IsSpawned) return;
		// Debug.Log(m_NewCharacter.GetComponent<PhotonView>().ViewID);

		// SetPlayerPlacement();
		// Player.m_SpawnPoint = m_SpawnPointList.List_SpawnPoint[(int)GetPlayerPosition()];
		// Player.m_SpawnPoint = GameManager.instance.m_SpawnPointList.List_SpawnPoint[SetPlayerPlacement()];
		Debug.Log("RandomNumber");
		Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["RandomNumber"]);
		var hash = PhotonNetwork.LocalPlayer.CustomProperties;
		int SpawnIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["RandomNumber"];
		PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

		Player.m_SpawnPoint = GameManager.instance.m_SpawnPointList.List_SpawnPoint[SpawnIndex];
		Player.m_Player.transform.position = Player.m_SpawnPoint.transform.position;
		Player.m_Player.transform.rotation = Player.m_SpawnPoint.transform.rotation;
		// RemoveSpawnPoint();
		IsSpawned = true;
	}

    public void SetPlayerPlacement()
	{
		/***********give player the current placement on start***********/
		m_PlayerData.m_CurrentPlacement = (int)PhotonNetwork.LocalPlayer.CustomProperties["RandomNumber"] + 1;  //Allocate the player placement on spawn.
	}

	public float GetPlayerPosition()
	{
		if (GameManager.instance.tempCount > 1) {
			Debug.Log("GetPlayerPosition " + GameManager.instance.StartingPosition);
			return GameManager.instance.StartingPosition;
		} else {
			Debug.Log("LastPositionAvailable " + GameManager.instance.SetNumberList[0]);
			return GameManager.instance.SetNumberList[0];
		}
	}

	public int GetRandomNumber()
	{
		return Random.Range(0, GameManager.instance.tempCount); //Note: int is exclusive [if tempCount = 2, range is 0-1]
	}
}
