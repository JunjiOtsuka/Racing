using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_FP : MonoBehaviour, InterfaceUIText
{
    public TMP_Text TMP;
    public static float CURRENT_FP;
    public GameObject NotEnoughFPImage;
    public bool ENOUGH_FP;

    void Start()
    {
        TMP.text = "0";
        SetUIText();
    }

    void Update()
    {
        SetUIText();
    }

    public void SetUIText() {
        TMP.text = $"{CURRENT_FP.ToString()} FP";
    }

    public void IncreaseFP()
    {
        CURRENT_FP += 1000f;
    }

    public void DecreaseFP(float FP_COST)
    {
        CURRENT_FP -= FP_COST;
    }

    public bool EnoughFP(float FP_COST)
    {
        if (CURRENT_FP < FP_COST) {
            Debug.Log("Not Enough FP");
            StartCoroutine(NOT_ENOUGH_FP());
            return false;
        }
        return CURRENT_FP > FP_COST;
    }

    IEnumerator NOT_ENOUGH_FP()
    {
        NotEnoughFPImage.SetActive(true);
        yield return new WaitForSeconds(2f);
        NotEnoughFPImage.SetActive(false);
    }
}
