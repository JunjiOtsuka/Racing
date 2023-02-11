using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GetClosestDistanceFromNextCheckPoint : MonoBehaviour
{
    public Transform PlayerTransform;
    CheckPointList m_CheckPointList;
    [HideInInspector]
    public float NextCheckPoint = 0;
    [HideInInspector]
    public float ResetNextCheckPoint = 1;
    // [HideInInspector]
    public GameObject targetGO;
    [HideInInspector]	public bool bIncrementLapCounter;	//Is the player ready to complete a lap? 
    [HideInInspector]
    public FinishLine m_FinishLine;
    [HideInInspector]
    public Vector3 closestPoint;
    public float m_GetDistanceToNextCheckpoint;
    Transform testSubject;
	public float TotalCheckPoints = 0f;

    /***Update UI***/
	public UI_LAP m_UI_LAP;					//A reference to the ship's ShipUI script
	public PhotonView m_PV;

    void Start()
    {
        /*Turned off all ismine off*/

		// if (!m_PV.IsMine) return;


        // targetGO = m_CheckPointList.List_CheckPoint[(int)NextCheckPoint].gameObject;
        // m_FinishLine = targetGO.GetComponent<FinishLine>();

        m_CheckPointList = GameObject.Find("CheckPointList").GetComponent<CheckPointList>();
        // testSubject = GameObject.Find("testSubject").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
		// if (!m_PV.IsMine) return;

        NextCheckPoint = TotalCheckPoints;
        ResetNextCheckPoint = NextCheckPoint + 1;
        if (ResetNextCheckPoint >= m_CheckPointList.List_CheckPoint.Count)
        {
            NextCheckPoint = NextCheckPoint % m_CheckPointList.List_CheckPoint.Count;
        }
        targetGO = m_CheckPointList.List_CheckPoint[(int)NextCheckPoint].gameObject;
        m_FinishLine = targetGO.GetComponent<FinishLine>();

        closestPoint = m_FinishLine.m_Collider.ClosestPointOnBounds(PlayerTransform.position);

        if (testSubject != null)
        {
            testSubject.position = closestPoint;
        }

        m_GetDistanceToNextCheckpoint = Vector3.Distance(PlayerTransform.position, targetGO.transform.position);
    }

    void OnTriggerEnter(Collider other)
	{
		// if (!m_PV.IsMine) return;

        if (!targetGO) return;
		//If the player has passed through the LapChecker (isRead) OR if Debug Mode is enabled (debugMode)
		//AND the object passing through this trigger is tagged as "PlayerSensor"...
        // if (other.transform.Find("GoalDetector") == null) return;

		//bool check
		//compare the name of the current checkpoint to player target checkpoint
		if (targetGO.transform.name != other.transform.name) return;

		if (other.transform.CompareTag("CheckPoint"))
		{
			if (other.transform.name == "FinishLine")
			{
				//...let the Game Manager know that the player completed a lap...
                if (bIncrementLapCounter) 
                {
                    m_UI_LAP.PlayerCompletedLap();
                    GameManager.instance.PlayerCompletedLap();
                }
				TotalCheckPoints++;
                bIncrementLapCounter = false;
			}
            else
			{
                if (other.transform.name == "FinalCheckPoint") bIncrementLapCounter = true;
				TotalCheckPoints++;
			}
		}
	}
}
