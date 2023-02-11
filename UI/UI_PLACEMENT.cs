using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_PLACEMENT : MonoBehaviour, InterfaceUIText
{
    public TMP_Text TMP;
    public float m_CurrentPlacement;
    public float m_PlayerInFrontIndex;
    // public List<GameManager.PlayerData> m_PlayerList;
    public GetClosestDistanceFromNextCheckPoint m_GetClosestDistanceFromNextCheckPoint;
    public float m_TotalCheckPoints;
    public float m_DistanceFromCheckPoint;
    public float m_TargetPlacement;
    public float m_TargetTotalCheckPoints;
    public float m_TargetDistanceFromCheckPoint;
    GameObject PlayerGO;

    void Start()
    {
        // m_TotalCheckPoints = m_GetClosestDistanceFromNextCheckPoint.TotalCheckPoints;
        m_DistanceFromCheckPoint = m_GetClosestDistanceFromNextCheckPoint.m_GetDistanceToNextCheckpoint;
    }

    void Update()
    {
        UpdatePlayerCurrentPosition();
        SetUIText();
    }

    public void SetUIText() {
        // m_CurrentPlacement = m_Placement;
        TMP.text = $"{m_CurrentPlacement.ToString()}";
    }

    public void UpdatePlayerCurrentPosition()
	{
        if (GameManager.instance.m_PlayerDataList.Count <= 1) return;
        m_PlayerInFrontIndex = Mathf.Clamp(GetPlayerOnePositionAbove(), 0, GameManager.instance.m_PlayerDataList.Count - 1);
        m_TargetPlacement = GameManager.instance.m_PlayerDataList[(int)m_PlayerInFrontIndex].m_CurrentPlacement;
        m_TargetTotalCheckPoints = GameManager.instance.m_PlayerDataList[(int)m_PlayerInFrontIndex].m_TotalCheckPointsPassed;
        m_TargetDistanceFromCheckPoint = GameManager.instance.m_PlayerDataList[(int)m_PlayerInFrontIndex].m_DistanceToCheckPoint;
        //Find player object in 1 placement above me
        if (m_CurrentPlacement > 1) {
            if (m_TotalCheckPoints > m_TargetTotalCheckPoints)
            {
                GameManager.instance.m_PlayerDataList[(int)GetMyPosition()].m_CurrentPlacement--;
                GameManager.instance.m_PlayerDataList[(int)m_PlayerInFrontIndex].m_CurrentPlacement++;
            }
            if (m_TotalCheckPoints == m_TargetTotalCheckPoints)
            {
                if (m_DistanceFromCheckPoint < m_TargetDistanceFromCheckPoint)
                {
                    GameManager.instance.m_PlayerDataList[(int)GetMyPosition()].m_CurrentPlacement--;
                    GameManager.instance.m_PlayerDataList[(int)m_PlayerInFrontIndex].m_CurrentPlacement++;
                }
            }
        }

        m_TotalCheckPoints = m_GetClosestDistanceFromNextCheckPoint.TotalCheckPoints;
        m_DistanceFromCheckPoint = m_GetClosestDistanceFromNextCheckPoint.m_GetDistanceToNextCheckpoint;
	}

    public float GetPlayerOnePositionAbove()
    {
        var i = 0;
        foreach(GameManager.PlayerData Player in GameManager.instance.m_PlayerDataList)
		{
            if (Player.m_CurrentPlacement != (m_CurrentPlacement - 1)) 
            {
                i++;
            } else {
                break;
            }
		}
        return i;
    }

    public float GetMyPosition()
    {
        var i = 0;
        foreach(GameManager.PlayerData Player in GameManager.instance.m_PlayerDataList)
		{
            if (Player.m_Player.name != transform.root.name) 
            {
                i++;
            } else {
                break;
            }
		}
        return i;
    }
}
