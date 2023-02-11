using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Photon.Pun;

public class RPCList : MonoBehaviour
{
    public UpdateModelAndMesh m_UpdateModelAndMesh;
    public AnimationManager m_AnimationManager;
    public OnPlayerHitManager m_OnPlayerHitManager;
    public ProjectileObjectPooler m_ProjectileObjectPooler;
    public SelectedComboManager m_SelectedComboManager;
    public PhotonView m_PV;
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    void Start()
    {
        m_PV = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void RPCAllPlayerReady()
    {
        GameManager.instance.IsPlayerReadyList.Add(true);
    }

    [PunRPC]
    public void RPCStartPlayableDirectorTimeline()
    {
        GameObject.Find("OpeningCutscene").GetComponent<PlayableDirector>().Play();
    }

    [PunRPC]
    public void RPCCreateNewPlayer()
	{
		GameManager.instance.m_NewCharacter = PhotonNetwork.Instantiate(GameManager.instance.PlayerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
		GameManager.instance.m_NewCharacter.transform.Find("CameraAngles").gameObject.SetActive(true);
		GameManager.instance.m_NewCharacter.name = "Me";
	}

    [PunRPC]
    void RPCAddNewPlayerToPlayerDataList()
	{
        //THIS IS FOR DICTIONARY
        Debug.Log("Is dictionary null: " + GameManager.instance.m_PlayerDataDictionary == null);
        if (!GameManager.instance.m_GameManagerPlayerData.m_Player) return;
        if (GameManager.instance.m_PlayerDataDictionary.ContainsKey(GameManager.instance.m_GameManagerPlayerData.m_Player.name))
        {
            Debug.Log("Player already exist");
            return;
        }
        GameManager.instance.m_PlayerDataDictionary.Add(GameManager.instance.m_GameManagerPlayerData.m_Player.name, GameManager.instance.m_GameManagerPlayerData);
		GameManager.instance.m_PlayerDataList = GameManager.instance.m_PlayerDataDictionary.Values.ToList();
	}

    private UnityEngine.Object LoadPrefabFromFile(string filename)
    {
        Debug.Log("Trying to load LevelPrefab from file ("+filename+ ")...");
        var loadedObject = Resources.Load(filename);
        if (loadedObject == null)
        {
            throw new FileNotFoundException("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    [PunRPC]
    public void RPCGetSelectedComboFromSO(int ViewID)
    {
        PhotonView playerView = PhotonView.Find(ViewID);
        if (!playerView) return;
        if (!m_SelectedComboManager) return;
        if (!m_SelectedComboManager.m_MySceneManager) return;
        if (!m_PV.IsMine) return;
        playerView.gameObject.GetComponentInChildren<SelectedComboManager>().SET_CHAR_TYPE = playerView.gameObject.GetComponentInChildren<SelectedComboManager>().m_MySceneManager.GameSceneSO._ChosenCharacter;
        playerView.gameObject.GetComponentInChildren<SelectedComboManager>().SET_CAR_TYPE = playerView.gameObject.GetComponentInChildren<SelectedComboManager>().m_MySceneManager.GameSceneSO._ChosenCar;
        playerView.gameObject.GetComponentInChildren<SelectedComboManager>().SET_ITEM_TYPE = playerView.gameObject.GetComponentInChildren<SelectedComboManager>().m_MySceneManager.GameSceneSO._ChosenItem;
    }

    [PunRPC]
    public void RPCLoad3DModel()
    {
        // Debug.Log(PhotonNetwork.LocalPlayer.UserId);
        // if (!m_UpdateModelAndMesh.m_SelectedComboManager.IsModelUpdated) return;

        // if (!PhotonView.Find(ViewID).IsMine) return;


        // Debug.Log(JsonUtility.ToJson(PhotonNetwork.PlayerList[0].CustomProperties.Keys));

        //***Iterates through every Car and spawns****
        //*** try if PhotonNetwork.IsMasterClient in UpdateModelAndMesh class.*****

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            var ViewID = Convert.ToInt32(PhotonNetwork.PlayerList[i].CustomProperties["ViewID"]);
            // if (!PhotonView.Find(ViewID)) return;
            Debug.Log("View ID in for loop");
            Debug.Log(PhotonNetwork.PlayerList[i].CustomProperties["ViewID"]);
            Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["ViewID"]);
            var target = PhotonView.Find(Convert.ToInt32(PhotonNetwork.PlayerList[i].CustomProperties["ViewID"]));
            var targetModelAndMesh = PhotonView.Find(Convert.ToInt32(PhotonNetwork.PlayerList[i].CustomProperties["ViewID"])).gameObject.GetComponentInChildren<UpdateModelAndMesh>();
            
            var car = (GameObject)Resources.Load(PhotonNetwork.PlayerList[i].CustomProperties["Car"].ToString());
            targetModelAndMesh.CarModelGO = Instantiate(car, targetModelAndMesh.CarModelGO.transform.position, targetModelAndMesh.CarModelGO.transform.rotation);
            targetModelAndMesh.CarModelGO.transform.SetParent(targetModelAndMesh.CarParent.transform);
            targetModelAndMesh.CarModelGO.transform.localScale = targetModelAndMesh.CarModelGO.transform.localScale * 10;
            // m_UpdateModelAndMesh.CarModelGO = Instantiate(car, m_UpdateModelAndMesh.CarModelGO.transform.position, m_UpdateModelAndMesh.CarModelGO.transform.rotation);
            // m_UpdateModelAndMesh.CarModelGO.transform.SetParent(m_UpdateModelAndMesh.CarParent.transform);
            // m_UpdateModelAndMesh.CarModelGO.transform.localScale = m_UpdateModelAndMesh.CarModelGO.transform.localScale * 10;
            var character = (GameObject)Resources.Load(PhotonNetwork.PlayerList[i].CustomProperties["Character"].ToString());
            targetModelAndMesh.CharacterModelGO = Instantiate(character, targetModelAndMesh.CharacterModelGO.transform.position, targetModelAndMesh.CharacterModelGO.transform.rotation);
            targetModelAndMesh.CharacterModelGO.transform.SetParent(targetModelAndMesh.CharacterParent.transform);
            targetModelAndMesh.CharacterParent.GetComponent<Animator>().avatar = targetModelAndMesh.m_SelectedComboManager.GetCharType().CharacterAvatar;
            // m_UpdateModelAndMesh.CharacterModelGO = Instantiate(character, m_UpdateModelAndMesh.CharacterModelGO.transform.position, m_UpdateModelAndMesh.CharacterModelGO.transform.rotation);
            // m_UpdateModelAndMesh.CharacterModelGO.transform.SetParent(m_UpdateModelAndMesh.CharacterParent.transform);
            // m_UpdateModelAndMesh.CharacterParent.GetComponent<Animator>().avatar = m_UpdateModelAndMesh.m_SelectedComboManager.GetCharType().CharacterAvatar;
        }

        //***Only spawn local models****
        /*
        //Load Car Model
        // Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["Car"]);
        Debug.Log(m_UpdateModelAndMesh.m_SelectedComboManager.GetCarType().CarModel.name);
        // GameObject carPrefabResource = (GameObject)Resources.Load(PhotonNetwork.LocalPlayer.CustomProperties["Car"].ToString());
        // GameObject carPrefabResource = (GameObject)Resources.Load(m_UpdateModelAndMesh.m_SelectedComboManager.GetCarType().CarModel.name);
        // var carPrefabResource = (GameObject)LoadPrefabFromFile(PhotonNetwork.LocalPlayer.CustomProperties["Car"].ToString());
        // var carPrefabResource = (GameObject)LoadPrefabFromFile(m_UpdateModelAndMesh.m_SelectedComboManager.GetCarType().CarModel.name);
        // GameObject carPrefabResource = (GameObject)Resources.Load(PhotonNetwork.LocalPlayer.CustomProperties["Car"].ToString());
        GameObject carPrefabResource = (GameObject)Resources.Load(selectedCombo.Car);
        m_UpdateModelAndMesh.CarModelGO = Instantiate(carPrefabResource, m_UpdateModelAndMesh.CarModelGO.transform.position, m_UpdateModelAndMesh.CarModelGO.transform.rotation);
        m_UpdateModelAndMesh.CarModelGO.transform.localScale = m_UpdateModelAndMesh.CarModelGO.transform.localScale * 10;
        m_UpdateModelAndMesh.CarModelGO.transform.SetParent(m_UpdateModelAndMesh.CarParent.transform);
        //Load Character Model
        Debug.Log(m_UpdateModelAndMesh.m_SelectedComboManager.GetCharType().CharacterModel.name);
        // GameObject charPrefabResource = (GameObject)Resources.Load(PhotonNetwork.LocalPlayer.CustomProperties["Character"].ToString());
        // GameObject charPrefabResource = (GameObject)Resources.Load(m_UpdateModelAndMesh.m_SelectedComboManager.GetCharType().CharacterModel.name);
        // var charPrefabResource = (GameObject)LoadPrefabFromFile(m_UpdateModelAndMesh.m_SelectedComboManager.GetCharType().CharacterModel.name);
        // var charPrefabResource = (GameObject)LoadPrefabFromFile(PhotonNetwork.LocalPlayer.CustomProperties["Character"].ToString());
        GameObject charPrefabResource = (GameObject)Resources.Load(selectedCombo.Character);
        m_UpdateModelAndMesh.CharacterModelGO = Instantiate(charPrefabResource, m_UpdateModelAndMesh.CharacterModelGO.transform.position, m_UpdateModelAndMesh.CharacterModelGO.transform.rotation);
        m_UpdateModelAndMesh.CharacterModelGO.transform.SetParent(m_UpdateModelAndMesh.CharacterParent.transform);
        m_UpdateModelAndMesh.CharacterParent.GetComponent<Animator>().avatar = m_UpdateModelAndMesh.m_SelectedComboManager.GetCharType().CharacterAvatar;
        */



        m_UpdateModelAndMesh.MyMeshFilter = m_UpdateModelAndMesh.CharacterModelGO.GetComponentsInChildren<MeshFilter>();
        m_UpdateModelAndMesh.MySkinnedMeshFilter = m_UpdateModelAndMesh.CharacterModelGO.GetComponentsInChildren<SkinnedMeshRenderer>();
        /* Change character size upon start */

        // If you want it to be 100% accurate, you should multiply RefSize here 
        // by RefMeshFilter.transform.lossyScale so that the other object's 
        // size also accounts for the reference object's scale.
        var RefSize = m_UpdateModelAndMesh.RefMeshFilter.mesh.bounds.size;
        // Debug.Log(RefSize);

        // if (MyMeshFilter != null) {
        //     var MySize = MyMeshFilter.mesh.bounds.size;  
        //     var NewScale = new Vector3(RefSize.x / MySize.x, RefSize.y / MySize.y, RefSize.z / MySize.z);

        //     // I'm un-parenting and re-parenting here as a quick way of setting global/lossy scale
        //     var Parent = MyMeshFilter.transform.parent;
        //     MyMeshFilter.transform.parent = null;
        //     MyMeshFilter.transform.localScale = NewScale;
        //     MyMeshFilter.transform.parent = Parent;
        // }

        if (m_UpdateModelAndMesh.MySkinnedMeshFilter != null) {
            m_UpdateModelAndMesh.resizeSkinnedMeshRenderer(m_UpdateModelAndMesh.MySkinnedMeshFilter, m_UpdateModelAndMesh.RefMeshFilter);
        }

        m_UpdateModelAndMesh.CharacterMesh.mesh = m_UpdateModelAndMesh.m_SelectedComboManager.GetCharType().CharacterMesh;
        m_UpdateModelAndMesh.CharacterModel.material = m_UpdateModelAndMesh.m_SelectedComboManager.GetCharType().CharacterMaterial;
        m_UpdateModelAndMesh.CarMesh.mesh = m_UpdateModelAndMesh.m_SelectedComboManager.GetCarType().CarMesh;
        m_UpdateModelAndMesh.CarModel.material = m_UpdateModelAndMesh.m_SelectedComboManager.GetCarType().CarMaterial;
        
    }

    [PunRPC]
    void RPCGetSpawnPoint(int SpawnIndex)
    {
		GameManager.instance.m_CopySpawnPointList.RemoveAt(SpawnIndex);
    }

    [PunRPC]
    void RPCGetModelAndAppendToChild(int ViewID)
    {
        //Grab the model in hierarchy and append child
        // if (!m_PV.IsMine) { return;}
        PhotonView playerView = PhotonView.Find(ViewID);
        // Debug.Log(playerView.gameObject.GetComponentInChildren<SelectedComboManager>().Car);
        // Debug.Log(playerView.gameObject.GetComponentInChildren<UpdateModelAndMesh>().CarParent.name);
        GameObject.Find(playerView.gameObject.GetComponentInChildren<SelectedComboManager>().Car + "(Clone)").transform.SetParent(playerView.gameObject.GetComponentInChildren<UpdateModelAndMesh>().CarParent.transform);
        GameObject.Find(playerView.gameObject.GetComponentInChildren<SelectedComboManager>().Character + "(Clone)").transform.SetParent(playerView.gameObject.GetComponentInChildren<UpdateModelAndMesh>().CharacterParent.transform);
        // m_UpdateModelAndMesh.CarModelGO.transform.SetParent(m_UpdateModelAndMesh.CarParent.transform);
        // m_UpdateModelAndMesh.CharacterModelGO.transform.SetParent(m_UpdateModelAndMesh.CharacterParent.transform);
    }

    [PunRPC]
    void RPCUpdateComboStringViaCustomProperties(int ViewID)
    {
        PhotonView playerView = PhotonView.Find(ViewID);

        // if (!playerView.IsMine) return;

        playerView.gameObject.GetComponentInChildren<SelectedComboManager>().Car = PhotonNetwork.LocalPlayer.CustomProperties["Car"].ToString();
        playerView.gameObject.GetComponentInChildren<SelectedComboManager>().Character = PhotonNetwork.LocalPlayer.CustomProperties["Character"].ToString();
        playerView.gameObject.GetComponentInChildren<SelectedComboManager>().Item = PhotonNetwork.LocalPlayer.CustomProperties["Item"].ToString();
        // m_SelectedComboManager.Car = PhotonNetwork.LocalPlayer.CustomProperties["Car"].ToString();
        // m_SelectedComboManager.Character = PhotonNetwork.LocalPlayer.CustomProperties["Character"].ToString();
        // m_SelectedComboManager.Item = PhotonNetwork.LocalPlayer.CustomProperties["Item"].ToString();
    }

    //**************ANIMATOR******************
    [PunRPC]
    void UpdateToStartDriving()
    {
        Debug.Log("Check Anim Drive");
        m_AnimationManager.m_Animator.SetBool("IsDriving", true);
    }

    [PunRPC]
    void UpdateToStopDriving()
    {
        Debug.Log("Check Anim Idle");
        m_AnimationManager.m_Animator.SetBool("IsDriving", false);
    }

    [PunRPC]
    public GameObject GetProjectile(string tag)
    {
        if (!m_ProjectileObjectPooler.poolDictionary.ContainsKey(tag)) 
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist");
        }

        List<GameObject> objectToSpawn = m_ProjectileObjectPooler.poolDictionary[tag];

        for(int i = 0; i < objectToSpawn.Count; i++)
        {
            if(!objectToSpawn[i].activeInHierarchy)
            {
                return objectToSpawn[i];
            }
        }
        return null;
    }

    [PunRPC]
    public void PoolProjectileGO()
    {
        m_ProjectileObjectPooler.poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (ProjectileObjectPooler.Pool pool in m_ProjectileObjectPooler.pools) 
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                // GameObject obj = Instantiate(pool.prefab);
                GameObject obj = PhotonNetwork.Instantiate(pool.prefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
                obj.transform.SetParent(m_ProjectileObjectPooler.transform);
                // StartCoroutine(WaitUntilProjectileManagerTrue(obj));
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            m_ProjectileObjectPooler.poolDictionary.Add(pool.tag, objectPool);
        }
    }

    //****************************************************************************
    [PunRPC]
    public void RPCPlayerUponFinish() 
    {
        m_OnPlayerHitManager.m_VehicleMovement.enabled = false;
        m_OnPlayerHitManager.m_MyPlayerInput.enabled = false;
        m_OnPlayerHitManager.m_SkillManager.enabled = false;
    }

    //removes player input x seconds
    [PunRPC]
    public void RPCPlayerUponHit() 
    {
        m_OnPlayerHitManager.m_VehicleMovement.enabled = false;
        m_OnPlayerHitManager.m_MyPlayerInput.enabled = false;
        m_OnPlayerHitManager.m_SkillManager.enabled = false;
        m_OnPlayerHitManager.m_CooldownManager.CDStart(m_OnPlayerHitManager.MaxKnockedTimer);
    }

    //gives back player input
    [PunRPC]
    public void RPCResetPlayerUponHit()
    {
        m_OnPlayerHitManager.m_VehicleMovement.enabled = true;
        m_OnPlayerHitManager.m_MyPlayerInput.enabled = true;
        m_OnPlayerHitManager.m_SkillManager.enabled = true;
        m_OnPlayerHitManager.m_CooldownManager.CDReset = false; //set to false to stop infite call
    }
}
