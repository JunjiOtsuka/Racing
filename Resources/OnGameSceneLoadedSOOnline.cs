using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New GameScene", menuName = "GameSceneOnline")]
public class OnGameSceneLoadedSOOnline : ScriptableObject
{
    [Serializable]
    public class PlayerInfo {
        [Header("Player")]
        public int PlayerID;
        [Header("CharacterSelection")]
        public CharTypeEnum _ChosenCharacter;
        [Header("CarSelection")]
        public CarTypeEnum _ChosenCar;
        [Header("ItemSelection")]
        public ItemTypeEnum _ChosenItem;
        [Header("StageSelection")]
        public string _ChosenStage;
        [Header("GameState")]
        public string _GameState;
        public string _GameMode;
    }
 
    public List<PlayerInfo> PlayerInfoList;

    void OnEnable() {
        hideFlags = HideFlags.DontUnloadUnusedAsset;

        _OnLoadSetToDefault();
    }

    void OnDisable() {
        /*-solve it by calling EditorUtility.SetDirty() before AssetDatabase.SaveAssets()-*/
        _OnLoadSetToDefault();
    }

    void _OnLoadSetToDefault() {
        foreach (PlayerInfo PlayerInfo in PlayerInfoList)
        {
            PlayerInfo._ChosenCharacter = (CharTypeEnum)0;
            PlayerInfo._ChosenCar = (CarTypeEnum)0;
            PlayerInfo._ChosenItem = (ItemTypeEnum)0;
            PlayerInfo._ChosenStage = "";
            PlayerInfo._GameState = "";
            PlayerInfo._GameMode = "";
        }
    }
}
