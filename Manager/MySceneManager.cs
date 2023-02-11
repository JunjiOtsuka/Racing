using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;

public class MySceneManager : MonoBehaviour
{
    public GameObject playerPrefab;
    Transform playerSpawner;
    public static GameObject player;
    public static string savedScene;
    public static string currentScene;
    // public static string currentSubScene;
    public PlayerData playerData;
    public AudioMixerManager _AudioMixerManager;
    public OnGameSceneLoadSO GameSceneSO;
    PlayerData data;
    public bool PlayerIsReady = false;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        // data = SaveAndLoad.LoadData();
        // _AudioMixerManager = GameObject.Find("UI:Audio").GetComponent<AudioMixerManager>();
        if (SaveAndLoad.playerData == null) return;
        savedScene = SaveAndLoad.playerData.SavedScene;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case ("MainMenu"): 
                MyInputActionManager.ToggleActionMap(MyInputActionManager.inputActions.UI);
                break;
            case ("BabyPark"): 
                MyInputActionManager.ToggleActionMap(MyInputActionManager.inputActions.Player);

                PlayerIsReady = true;

                // playerSpawner = GameObject.Find("PlayerSpawner").GetComponent<Transform>();
                // player = Instantiate(playerPrefab, playerSpawner.position, playerSpawner.rotation);
                // currentScene = scene.name;
                break;
        }
        
        //outside main menu do the following
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    OnGameSceneLoadSO _GameScene;

    void Awake() {
        _GameScene = Resources.Load<OnGameSceneLoadSO>("GameScene");
    }

    public void OnClickSceneChange() {
        if (EventSystem.current.currentSelectedGameObject == null) {
            return;
        }

        if (EventSystem.current.currentSelectedGameObject.name == "Versus") {
            _GameScene._GameMode = "Offline";
            SceneManager.LoadScene("SideSelectionV2");
        }
        if (EventSystem.current.currentSelectedGameObject.name == "ScreenSelection") {
            // _GameScene._GameMode = "Offline";
            SceneManager.LoadScene("ScreenSelection");
            // SceneManager.LoadScene("BabyPark");
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Online") {
            _GameScene._GameMode = "Online";
            SceneManager.LoadScene("ConnectToInternet");
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Connect") {
            SceneManager.LoadScene("OnlineLobby");
        }
        
        if (EventSystem.current.currentSelectedGameObject.name == "ReturnToMainMenu") {
            SceneManager.LoadScene("MainMenu");
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Option") {
            //do something here
            Debug.Log("Option");
        }
        if (EventSystem.current.currentSelectedGameObject.name == "Quit") {
            Application.Quit();
        }
    }

    public void LoadTrackScene()
    {
        SceneManager.LoadScene(GameSceneSO._ChosenStage);
    }
}
