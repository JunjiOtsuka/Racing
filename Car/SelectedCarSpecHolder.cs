using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectedCarSpecHolder : MonoBehaviour
{
    public enum CarTypeEnum {
        TYPE1,
        TYPE2,
        TYPE3
    }
    public CarTypeEnum SET_CAR_TYPE;
    [HideInInspector]
	public CarSOHolder SOHolder;
    [HideInInspector]
    public VehicleMovement GetVehicleMovement;

    public void SetCarByButtonPress()
    {
        string ClickButtonName = EventSystem.current.currentSelectedGameObject.name;
        
        var ParsedClickButtonName = int.Parse(ClickButtonName);

        SET_CAR_TYPE = (CarTypeEnum)ParsedClickButtonName;

        GetVehicleMovement.m_CarSO = GetCarType();
    }

    public CarSO GetCarType()
    {
        switch(SET_CAR_TYPE)
        {
            case CarTypeEnum.TYPE1:
                return SOHolder.Car_Type1;
            case CarTypeEnum.TYPE2:
                return SOHolder.Car_Type2;
            case CarTypeEnum.TYPE3:
                return SOHolder.Car_Type3;
        }
        return null;
    }
}
