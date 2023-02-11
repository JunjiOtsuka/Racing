using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [HideInInspector]	public bool isReady;	//Is the player ready to complete a lap? 
    [HideInInspector]	public bool isCheckPointReady;	//Is the player ready to complete a lap? 

	public bool debugMode;						//Debug variable that enables quick testing of laps
	[HideInInspector]
    public Collider m_Collider;
    public GetClosestDistanceFromNextCheckPoint m_GetClosestDistanceFromNextCheckPoint;

	void Start()
	{
		m_Collider = GetComponent<Collider>();
	}

	//Called when the player drives through the finish line
	// void OnTriggerEnter(Collider other)
	// {
	// 	//If the player has passed through the LapChecker (isRead) OR if Debug Mode is enabled (debugMode)
	// 	//AND the object passing through this trigger is tagged as "PlayerSensor"...
    //     if (other.transform.Find("GoalDetector") == null) return;

	// 	//bool check
	// 	//compare the name of the current checkpoint to player target checkpoint
	// 	if (m_GetClosestDistanceFromNextCheckPoint.targetGO.transform.name != other.transform.name) return;

	// 	if ((isReady || debugMode) && other.transform.Find("GoalDetector").CompareTag("PlayerSensor"))
	// 	{
	// 		if (transform.name == "FinishLine")
	// 		{
	// 			//...let the Game Manager know that the player completed a lap...
	// 			GameManager.instance.PlayerCompletedLap();
	// 			other.GetComponentInChildren<GetClosestDistanceFromNextCheckPoint>().TotalCheckPoints++;
	// 			//...and deactivate the finish line until the player completes another lap
	// 			isReady = false;
	// 		}
	// 		if (transform.CompareTag("CheckPoint"))
	// 		{
	// 			isCheckPointReady = true;
	// 			other.GetComponentInChildren<GetClosestDistanceFromNextCheckPoint>().TotalCheckPoints++;

	// 			//if this checkpoint is the last checkpoint before finish line
	// 			if(transform.name == "FinalCheckPoint")
	// 			{
	// 				isReady = true;
	// 			}
	// 		}
	// 	}
	// }
}
