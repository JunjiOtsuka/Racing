using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
	//The game manager holds a public static reference to itself. This is often referred to
	//as being a "singleton" and allows it to be access from all other objects in the scene.
	//This should be used carefully and is generally reserved for "manager" type objects
	public static GameManager instance;

	public GameObject PlayerPrefab;
	//reference to a new player joining the game
	public GameObject m_NewCharacter;

	[Header("Race Settings")]
	public int numberOfLaps = 3;			//The number of laps to complete
	public OnPlayerHitManager m_OnPlayerHitManager;	//A reference to the ship's VehicleMovement script

	[Header("UI References")]
	public UI_LAP m_UI_LAP;					//A reference to the ship's ShipUI script
	public UI_LAPTIME m_UI_LAPTIME;				//A reference to the LapTimeUI script in the scene
	public GameObject UI_GameOver;			//A reference to the UI objects that appears when the game is complete

	float[] lapTimes;						//An array containing the player's lap times
	int currentLap = 0;						//The current lap the player is on
	public bool isGameOver;						//A flag to determine if the game is over
	bool raceHasBegun;						//A flag to determine if the race has begun
	bool IsPlayerInList;						//A flag to determine if the race has begun
	bool IsTimelinePlaying;						//A flag to determine if the race has begun
	MySceneManager m_MySceneManager;

	[System.Serializable]
	/*player list...  
	 	placement
		the next checkpoint
		distance from next checkpoint
		total checkpoints passed
	*/
    public class PlayerData {
		public GameObject m_Player;
		// [HideInInspector]
		public UI_PLACEMENT m_UI_Placement;
		[HideInInspector]
		public GetClosestDistanceFromNextCheckPoint m_GetClosestDistanceFromNextCheckPoint;	
		public float m_TotalCheckPointsPassed;
		public float m_DistanceToCheckPoint;
		public float m_CurrentPlacement;
		public GameObject m_SpawnPoint;
	}

	public PlayerData m_GameManagerPlayerData;
	public List<PlayerData> m_PlayerDataList;
	public Dictionary<string, PlayerData> m_PlayerDataDictionary;
	public List<float> SetNumberList;
	public List<bool> IsPlayerReadyList;
	public int tempCount;
	public float StartingPosition;
	public SpawnPointList m_SpawnPointList;
	public List<GameObject> m_CopySpawnPointList;
	public bool m_PopListComplete = false;
	public bool isPlayerDataListPopulated = false;
	public int totalPlayerInRoom;
	public bool IsMidGame;

	void Awake()
	{
		//If the variable instance has not be initialized, set it equal to this
		//GameManager script...
		if (instance == null) {

			instance = this;
		}
		//...Otherwise, if there already is a GameManager and it isn't this, destroy this
		//(there can only be one GameManager)
		else if (instance != this)
			Destroy(gameObject);
	}

	void OnEnable()
	{
		//When the GameManager is enabled, we start a coroutine to handle the setup of
		//the game. It is done this way to allow our intro cutscene to work. By slightly
		//delaying the start of the race, we give the cutscene time to take control and 
		//play out
		StartCoroutine(Init());
	}

	void Start()
	{
		totalPlayerInRoom = PhotonNetwork.PlayerList.Length;
		//creating new player on client side
		CreateNewPlayerAndData();
		if (PhotonNetwork.IsMasterClient) {
			// CreateListOfNumber();
			// Debug.Log("SetPlayerPlacement()");
			// Debug.Log(SetPlayerPlacement());
			// for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
			// {
			// 	PhotonNetwork.PlayerList[i].CustomProperties["RandomNumber"] = (int)SetPlayerPlacement();
			// }



			// SetSpawnPoint();
			// GetSpawnPoint(GetIndexSpawnLocation());
		}
	}

	IEnumerator Init()
	{
		//Update the lap number on the ship
		UpdateUI_LapNumber();

		//Wait a little while to let everything initialize
		yield return new WaitForSeconds(.1f);

		//Initialize the lapTimes array and set that the race has begun
		lapTimes = new float[numberOfLaps];
		raceHasBegun = true;
	}

	void Update()
	{
		//Update the speed on the ship UI
		UpdateUI_Speed ();

		//If we have an active game...
		if (IsActiveGame() && UI_CountdownManager.instance.m_IsCountdownComplete)
		{
			//...calculate the time for the lap and update the UI
			lapTimes[currentLap] += Time.deltaTime;
			UpdateUI_LapTime();
		}

		// //master client tasks from here on below.
		foreach(PlayerData Player in m_PlayerDataList)
		{
			Player.m_DistanceToCheckPoint = Player.m_GetClosestDistanceFromNextCheckPoint.m_GetDistanceToNextCheckpoint;
			Player.m_TotalCheckPointsPassed = Player.m_GetClosestDistanceFromNextCheckPoint.TotalCheckPoints;
			Player.m_UI_Placement.m_CurrentPlacement = Player.m_CurrentPlacement;
		}

		// Debug.Log(m_PlayerDataDictionary.Count + " " + totalPlayerInRoom);

		// if (m_PlayerDataDictionary.Count == totalPlayerInRoom && !isPlayerDataListPopulated)
		// {
		// 	// populate the list here
		// 	Debug.Log("populate list here");
		// 	PopulatePlayerDataList();
		// 	isPlayerDataListPopulated = true;
		// }

		if (PhotonNetwork.IsMasterClient)
		{
			if (m_MySceneManager.PlayerIsReady && !IsPlayerInList)
			{
				Debug.Log("All player ready.");
				m_NewCharacter.GetComponent<PhotonView>().RPC("RPCAllPlayerReady", RpcTarget.AllViaServer);
				IsPlayerInList = true;
			}
			if (m_PlayerDataList.Count == IsPlayerReadyList.Count && !IsTimelinePlaying && !IsMidGame)
			{
				Debug.Log("Starting game");
				m_NewCharacter.GetComponent<PhotonView>().RPC("RPCStartPlayableDirectorTimeline", RpcTarget.AllViaServer);
				IsTimelinePlaying = true;
				IsMidGame = true;
			}
		}
		// UpdatePlayerCurrentPosition();
	}

	//Called by the FinishLine script
	public void PlayerCompletedLap()
	{
		//If the game is already over exit this method 
		if (isGameOver)
			return;


		/*
		//Incremement the current lap
		currentLap++;

		//Update the lap number UI on the ship
		UpdateUI_LapNumber();
		*/

		//If the player has completed the required amount of laps...
		if (currentLap >= numberOfLaps)
		{
			//...the game is now over...
			isGameOver = true;
			//...update the laptime UI...
			UpdateUI_FinalTime();
			//...and show the Game Over UI
			UI_GameOver.SetActive(true);
            m_OnPlayerHitManager.PlayerUponFinish();
		}
	}

	public void UpdatePlayerCurrentPosition()
	{
		foreach(PlayerData Player in m_PlayerDataList)
		{
			// Player.m_UI_Placement.m_CurrentPlacement = Player.m_CurrentPlacement;
			Player.m_UI_Placement.SetUIText();
		}
	}

	void UpdateUI_LapTime()
	{
		//If we have a LapTimeUI reference, update it
		if (m_UI_LAPTIME != null)
			m_UI_LAPTIME.SetLapTime(currentLap, lapTimes[currentLap]);
	}

	public void UpdateUI_FinalTime()
	{
		//If we have a LapTimeUI reference... 
		if (m_UI_LAPTIME != null)
		{
			float total = 0f;

			//...loop through all of the lapTimes and total up an amount...
			for (int i = 0; i < lapTimes.Length; i++)
				total += lapTimes[i];

			//... and update the final race time
			m_UI_LAPTIME.SetFinalTime(total);
		}
	}

	void UpdateUI_LapNumber()
	{
		//If we have a ShipUI reference, update it
		if (m_UI_LAP != null) 
			m_UI_LAP.SetLapDisplay (currentLap + 1, numberOfLaps);
	}

	void UpdateUI_Speed()
	{
		//If we have a VehicleMovement and ShipUI reference, update it
		// if (vehicleMovement != null && shipUI != null) 
			// shipUI.SetSpeedDisplay (Mathf.Abs(vehicleMovement.speed));
	}

	public bool IsActiveGame()
	{
		//If the race has begun and the game is not over, we have an active game
		return raceHasBegun && !isGameOver;
	}

	public void Restart()
	{
		//Restart the scene by loading the scene that is currently loaded
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void CreateNewPlayerAndData()
	{
        m_PlayerDataDictionary = new Dictionary<string, GameManager.PlayerData>();
		CreateNewPlayer();                 //Create client character
		// CreateNewPlayerData();             //Create client player data
		// CreateListOfNumber();              //Create list with numbers
		// SetSpawnPoint(m_PlayerData);       //Set spawn location of players
		// AddNewPlayerToPlayerDataList();    //Add new player to PlayerData list.
		m_PopListComplete = true;
		m_MySceneManager = GameObject.Find("SceneManager").GetComponent<MySceneManager>();
		
		//add new player data class to the master client list
		// m_NewCharacter.GetComponent<PhotonView>().RPC("RPCAddNewPlayerToPlayerDataList", RpcTarget.AllViaServer);
	}

	public void CreateNewPlayer()
	{
		//Get spawn point here
		//remove used spawn point
		// var index = GetIndexSpawnLocation();
		// var SpawnPoint = GetSpawnPoint(index);
		// m_NewCharacter = PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPoint.transform.position, SpawnPoint.transform.rotation, 0);
		m_NewCharacter = PhotonNetwork.Instantiate(PlayerPrefab.name, new Vector3 (0f, 0f, 0f), Quaternion.identity, 0);
		// m_CopySpawnPointList.RemoveAt(index);
		m_NewCharacter.transform.Find("CameraAngles").gameObject.SetActive(true);
		m_NewCharacter.name = "Me";
		// m_NewCharacter.GetComponent<PhotonView>().RPC("RPCCreateNewPlayer", RpcTarget.AllViaServer);
	}

	// void CreateNewPlayerData()
	// {
	// 	m_PlayerData = new PlayerData();
	// 	m_PlayerData.m_Player = m_NewCharacter;
	// 	m_PlayerData.m_GetClosestDistanceFromNextCheckPoint = m_PlayerData.m_Player.GetComponentInChildren<GetClosestDistanceFromNextCheckPoint>();
	// 	m_PlayerData.m_UI_Placement = m_PlayerData.m_Player.GetComponentInChildren<UI_PLACEMENT>();
	// }

	void AddNewPlayerToPlayerDataList()
	{
		m_NewCharacter.GetComponent<PhotonView>().RPC("RPCAddNewPlayerToPlayerDataList", RpcTarget.MasterClient);
	}

	void PopulatePlayerDataList()
	{
		m_PlayerDataList = m_PlayerDataDictionary.Values.ToList();
	}

	public GameObject GetSpawnPoint(int SpawnIndex)
	{
		if (totalPlayerInRoom > 1) {
			m_CopySpawnPointList = m_SpawnPointList.List_SpawnPoint.GetRange(0, totalPlayerInRoom);
			Debug.Log(m_CopySpawnPointList.Count);
			var SpawnPoint = m_CopySpawnPointList[SpawnIndex];
			GameManager.instance.m_CopySpawnPointList.RemoveAt(SpawnIndex);
			// m_NewCharacter.GetComponent<PhotonView>().RPC("RPCGetSpawnPoint", RpcTarget.All, SpawnIndex);
			return SpawnPoint;
		} else {
			m_CopySpawnPointList = m_SpawnPointList.List_SpawnPoint.GetRange(0, 1);
			Debug.Log(m_CopySpawnPointList.Count);
			var SpawnPoint = m_CopySpawnPointList[0];
			GameManager.instance.m_CopySpawnPointList.RemoveAt(SpawnIndex);
			// m_NewCharacter.GetComponent<PhotonView>().RPC("RPCGetSpawnPoint", RpcTarget.All, SpawnIndex);
			return SpawnPoint;
		}
	}

	public int GetIndexSpawnLocation()
	{
		int RandomNumber = Random.Range(0, m_CopySpawnPointList.Count);
		return RandomNumber;
	}

	void SetSpawnPoint(PlayerData Player)
	{
		// Debug.Log(m_NewCharacter.GetComponent<PhotonView>().ViewID);

		// SetPlayerPlacement();
		// Player.m_SpawnPoint = m_SpawnPointList.List_SpawnPoint[(int)GetPlayerPosition()];
		Player.m_SpawnPoint = m_SpawnPointList.List_SpawnPoint[(int)SetPlayerPlacement()];
		Player.m_Player.transform.position = Player.m_SpawnPoint.transform.position;
		Player.m_Player.transform.rotation = Player.m_SpawnPoint.transform.rotation;
	}

	void CreateListOfNumber()
	{
		//Create list of numbers
		
		// for (var i = 0; i < m_PlayerDataList.Count; i++) {
		for (var i = 0; i < totalPlayerInRoom; i++) {
			SetNumberList.Add(i);
		}
		//[0, 1, 2] count = 3
	}

	public float SetPlayerPlacement()
	{
		Debug.Log("Master is setting player position");
		tempCount = SetNumberList.Count;
		Debug.Log("tempCount " + tempCount);
		if (tempCount > 0) {
			var RandomNumber = GetRandomNumber();
			StartingPosition = SetNumberList[RandomNumber];
			SetNumberList.RemoveAt((int)RandomNumber);
			Debug.Log("StartingPosition " + StartingPosition);
			return StartingPosition;
		} else {
			//return first index of the array
			Debug.Log("LastPositionAvailable " + (int)SetNumberList[0]);
			return (int)SetNumberList[0];
		}
	}

	public float GetPlayerPosition()
	{
		if (tempCount > 0) {
			Debug.Log("GetPlayerPosition " + StartingPosition);
			return StartingPosition;
		} else {
			Debug.Log("LastPositionAvailable " + SetNumberList[0]);
			return SetNumberList[0];
		}
	}

	public int GetRandomNumber()
	{
		return Random.Range(0, tempCount); //Note: int is exclusive [if tempCount = 2, range is 0-1]
	}
}