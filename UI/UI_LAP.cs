using UnityEngine;
using TMPro;

public class UI_LAP : MonoBehaviour
{
    [Header("UI Text References")]
	public TextMeshProUGUI currentSpeedText;	//The text element for the current speed
	public TextMeshProUGUI currentLapText;		//The text element for the current lap
	int currentLap = 0;						    //The current lap the player is on
	public OnPlayerHitManager m_OnPlayerHitManager;	//A reference to the ship's VehicleMovement script

	public void SetLapDisplay(int currentLap, int numberOfLaps)
	{
		//If we are trying to set a lap greater than the total number of laps, exit
		if (currentLap > numberOfLaps)
			return;

		//Update the current lap text
		currentLapText.text = "Lap" + currentLap + "/" + numberOfLaps;
	}

	public void SetSpeedDisplay(float currentSpeed)
	{
		//Turn the current speed into an integer and set it in the UI
		int speed = (int)currentSpeed;
		currentSpeedText.text = speed.ToString();
	}

	public void PlayerCompletedLap()
	{
		//Incremement the current lap
		currentLap++;

		//Update the lap number UI on the ship
		UpdateUI_LapNumber();

		if (currentLap >= GameManager.instance.numberOfLaps)
		{
			//...the game is now over...
			GameManager.instance.isGameOver = true;
			//...update the laptime UI...
			GameManager.instance.UpdateUI_FinalTime();
			//...and show the Game Over UI
			GameManager.instance.UI_GameOver.SetActive(true);

            m_OnPlayerHitManager.PlayerUponFinish();
		}
	}

	void UpdateUI_LapNumber()
	{
		SetLapDisplay (currentLap + 1, GameManager.instance.numberOfLaps);
	}

}
