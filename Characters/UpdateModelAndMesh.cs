using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Photon.Pun;

public class UpdateModelAndMesh : MonoBehaviour
{
    public GameObject CharacterParent;
    public GameObject CarParent;
    public GameObject CharacterModelGO;
    public MeshFilter CharacterMesh;
    public MeshRenderer CharacterModel;
    public GameObject CarModelGO;
    public MeshFilter CarMesh;
    public MeshRenderer CarModel;
    public MeshFilter RefMeshFilter;
    public MeshFilter[] MyMeshFilter;
    public SkinnedMeshRenderer[] MySkinnedMeshFilter;
    public SelectedComboManager m_SelectedComboManager;
	public PhotonView m_PV;
    public bool IsModelSpawned;

    void Start()
    {
        //TEST 1 see if !PhotonNetwork.IsMasterClient work

        // if (!m_PV.IsMine) {
            //if its not mine grab the 3d model and disable them in the inspector
            // if (CarParent.transform.childCount > 0 && CarParent.transform.childCount < 2) 
            // {
            //     CarParent.transform.GetChild(0).gameObject.SetActive(false);
            // }
            // if (CharacterParent.transform.childCount > 0 && CharacterParent.transform.childCount < 2) 
            // {
            //     CharacterParent.transform.GetChild(0).gameObject.SetActive(false);
            // }
            // Load3DModel();
            // return;
        // }
        // Debug.Log("ViewID: " + m_PV.ViewID);
        // if (CarParent.transform.childCount > 0 && CarParent.transform.childCount < 2) 
        // {
        //     CarParent.transform.GetChild(0).gameObject.SetActive(false);
        // }
        // if (CharacterParent.transform.childCount > 0 && CharacterParent.transform.childCount < 2) 
        // {
        //     CharacterParent.transform.GetChild(0).gameObject.SetActive(false);
        // }
        // if (!CheckModelCountIsZero())
        // {
        //     return;
        // }
        // m_PV.RPC("RPCGetSelectedComboFromSO", RpcTarget.AllBufferedViaServer);

        // if (!m_PV.IsMine) return;

        // if (!PhotonNetwork.IsMasterClient) return;
            // Load3DModel();
        WaitUntilViewIdAllocated();
    }

    public void WaitUntilViewIdAllocated()
    {
        var ViewIDList = new List<int?>();

        // for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        // {
        //     ViewIDList.Add((int)PhotonNetwork.PlayerList[i].CustomProperties["ViewID"]);
        // }

        var j = 0;
        while (j <= PhotonNetwork.PlayerList.Length - 1)
        {
            if ((int)PhotonNetwork.PlayerList[j].CustomProperties["ViewID"] == null) 
            {
                Debug.Log("View ID is null. Waiting for view id");
                continue;
            } else {
                Debug.Log("Inside Loop Update model");
                Debug.Log((int)PhotonNetwork.LocalPlayer.CustomProperties["ViewID"]);
                Debug.Log((int)PhotonNetwork.PlayerList[j].CustomProperties["ViewID"]);
                Debug.Log(PhotonNetwork.PlayerList[j].CustomProperties["Car"]);
                Debug.Log(PhotonNetwork.PlayerList[j].CustomProperties["Character"]);
                ViewIDList.Add((int)PhotonNetwork.PlayerList[j].CustomProperties["ViewID"]);
                j++;
            }
        }
        if (!ViewIDList.Contains(null)) {
            Load3DModel();
        }
    }

    public bool CheckModelCount()
    {
        return CarParent.transform.childCount >= 2 && CharacterParent.transform.childCount >= 2;
    }

    public bool CheckModelCountIsZero()
    {
        return CarParent.transform.childCount == 0 && CharacterParent.transform.childCount == 0;
    }


    // Start is called before the first frame update
    public void Load3DModel()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        // if (CarParent.transform.childCount > 0) return;
        // if (CharacterParent.transform.childCount > 0) return;

        //TEST 2: THIS LINE HERE
        // m_PV.RPC("RPCGetSelectedComboFromSO", RpcTarget.AllBufferedViaServer);
        m_PV.RPC("RPCLoad3DModel", RpcTarget.AllViaServer);
        // m_PV.RPC("RPCGetModelAndAppendToChild", RpcTarget.AllBufferedViaServer, m_PV.ViewID);
        // Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["Character"]);
        // Debug.Log(PhotonNetwork.LocalPlayer.CustomProperties["Car"]);

        //Load Car Model
        // GameObject carPrefabResource = (GameObject)Resources.Load(PhotonNetwork.LocalPlayer.CustomProperties["Car"].ToString());
        // // var carPrefabResource = (GameObject)LoadPrefabFromFile(m_UpdateModelAndMesh.m_SelectedComboManager.GetCarType().CarModel.name);
        // CarModelGO = Instantiate(carPrefabResource, CarModelGO.transform.position, CarModelGO.transform.rotation);
        // CarModelGO.transform.localScale = CarModelGO.transform.localScale * 10;
        // CarModelGO.transform.SetParent(CarParent.transform);
        // //Load Character Model
        // // var charPrefabResource = (GameObject)LoadPrefabFromFile(m_SelectedComboManager.GetCharType().CharacterModel.name);
        // GameObject charPrefabResource = (GameObject)Resources.Load(PhotonNetwork.LocalPlayer.CustomProperties["Character"].ToString());
        // CharacterModelGO = Instantiate(charPrefabResource, CharacterModelGO.transform.position, CharacterModelGO.transform.rotation);
        // CharacterModelGO.transform.SetParent(CharacterParent.transform);
        // CharacterParent.GetComponent<Animator>().avatar = m_SelectedComboManager.GetCharType().CharacterAvatar;

    }

    public void resizeSkinnedMeshRenderer(SkinnedMeshRenderer[] goResizeList, MeshFilter referenceGO)
    {
        Vector3 refSize = referenceGO.mesh.bounds.size;

        foreach(SkinnedMeshRenderer go in goResizeList)
        {
            Debug.Log("Is working");
            float resizeX = refSize.x / go.bounds.size.x;
            float resizeY = refSize.y / go.bounds.size.y;
            float resizeZ = refSize.z / go.bounds.size.z;
        
            resizeX *= go.transform.localScale.x;
            resizeY *= go.transform.localScale.y;
            resizeZ *= go.transform.localScale.z;

            go.transform.localScale = new Vector3(resizeX, resizeY, resizeZ);
        }
    }
}
