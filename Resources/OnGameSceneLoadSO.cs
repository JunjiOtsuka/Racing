using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

[CreateAssetMenu(fileName = "New GameScene", menuName = "GameScene")]
public class OnGameSceneLoadSO : ScriptableObject
{
    [Header("PlayerID")]
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

    void OnEnable() {
        hideFlags = HideFlags.DontUnloadUnusedAsset;

        _OnLoadSetToDefault();
    }

    void OnDisable() {

        /*-solve it by calling EditorUtility.SetDirty() before AssetDatabase.SaveAssets()-*/
        _OnLoadSetToDefault();
    }

    void _OnLoadSetToDefault() {
        _ChosenCharacter = (CharTypeEnum)0;
        _ChosenCar = (CarTypeEnum)0;
        _ChosenItem = (ItemTypeEnum)0;
        _ChosenStage = "";
        _GameState = "";
        _GameMode = "";
    }
}
