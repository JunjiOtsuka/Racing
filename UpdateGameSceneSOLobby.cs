using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UpdateGameSceneSOLobby : MonoBehaviour
{
    public int PlayerID;
    // public string m_ChosenCharacter;
    // public string m_ChosenCar;
    // public string m_ChosenItem;
    // public string m_ChosenStage;
    public OnGameSceneLoadSO m_OnGameSceneLoadSO;
    public SelectedComboManager m_SelectedComboManager;
    public LobbyManager m_LobbyManager;

    // Start is called before the first frame update
    void OnEnable()
    {
        PlayerID = m_OnGameSceneLoadSO.PlayerID;
    }

    void Start()
    {
        m_LobbyManager.m_ChosenCharacter = m_SelectedComboManager.GetCharType().CharacterPrefabName;
        m_LobbyManager.m_ChosenCar = m_SelectedComboManager.GetCarType().CarPrefabName;
        m_LobbyManager.m_ChosenItem = m_SelectedComboManager.GetItemType().ItemPrefabName;
        m_LobbyManager.m_ChosenStage = m_OnGameSceneLoadSO._ChosenStage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
