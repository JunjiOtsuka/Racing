using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateSelectedCharacterUI : MonoBehaviour
{
    public OnGameSceneLoadSO GameScene;
    public SelectedComboManager m_SelectedComboManager;
    public TMP_Text CharacterTMP;
    public Image CharacterImage;
    public TMP_Text CarTMP;
    public Image CarImage;
    public TMP_Text Item;
    public Image ItemImage;

    public void UpdateInfo()
    {
        GameScene._ChosenCharacter = m_SelectedComboManager.SET_CHAR_TYPE;
        GameScene._ChosenCar = m_SelectedComboManager.SET_CAR_TYPE;
        // GameScene._ChosenItem = ;
        Debug.Log(m_SelectedComboManager.GetCharType().CharacterPrefabName);
        CharacterTMP.text = m_SelectedComboManager.GetCharType().CharacterPrefabName;
        CharacterImage.sprite = m_SelectedComboManager.GetCharType().CharacterImage;
        CarTMP.text = m_SelectedComboManager.GetCarType().CarPrefabName;
        CarImage.sprite = m_SelectedComboManager.GetCarType().CarImage;
    }
}
