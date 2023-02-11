using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITactical : MonoBehaviour, InterfaceUICooldown
{
    public float CDSeconds;
    public float currentCD;
    public GameObject Tactical;
    public GameObject TacticalBG;
    public GameObject SkillTimer;
    public static bool InCooldown { get; set; }
	public MyPlayerInput input;						//A reference to the player's input		

    void Start()
    {
        //put skills in CD
        InCooldown = true;
    }

    void Update()
    {
        if (InCooldown) 
        {
            CDManager();
        }    
    }

    public void CDManager()
    {
        StartCD();
    }

    //start cooldown

    public void StartCD()
    {
        InCooldown = true;
        SkillTimer.SetActive(true);
        currentCD += Time.deltaTime / CDSeconds;
        currentCD = Mathf.Clamp(currentCD, 0, 1);
        SkillTimer.GetComponent<TMP_Text>().text = $"{Mathf.Floor((CDSeconds - currentCD * CDSeconds) + 1)}s";
        Tactical.GetComponent<Image>().fillAmount = currentCD;

        if (currentCD == 1) {
            StopCD();
        }
    }

    //stop cooldown
    public void StopCD()
    {
        InCooldown = false;
        currentCD = 1;
        Tactical.GetComponent<Image>().fillAmount = currentCD;
        SkillTimer.SetActive(false);

        ResetCD();
    }

    public void ResetCD()
    {
        currentCD = 0;
    }
}
