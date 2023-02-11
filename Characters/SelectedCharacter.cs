using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedCharacter : MonoBehaviour
{
    public enum CharTypeEnum {
        TYPE1,
        TYPE2,
        TYPE3
    }
    public CharTypeEnum SET_CHAR_TYPE;
    [HideInInspector]
	public CharacterSOHolder SOHolder;
    [HideInInspector]
    public VehicleMovement GetVehicleMovement;

    public void SetCharByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        SET_CHAR_TYPE = (CharTypeEnum)ParsedClickButtonName;

        GetVehicleMovement.m_CharSO = GetCharType();
    }

    public CharacterSO GetCharType()
    {
        switch(SET_CHAR_TYPE)
        {
            case CharTypeEnum.TYPE1:
                GetVehicleMovement.m_CharSO = SOHolder.Character_Type1;
                break;
            case CharTypeEnum.TYPE2:
                GetVehicleMovement.m_CharSO = SOHolder.Character_Type2;
                break;
            case CharTypeEnum.TYPE3:
                GetVehicleMovement.m_CharSO = SOHolder.Character_Type3;
                break;
        }
        return GetVehicleMovement.m_CharSO;
    }
}
