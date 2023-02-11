using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_CountdownManager : MonoBehaviour
{
	public static UI_CountdownManager instance;
	public TextMeshProUGUI currentSpeedText;
    public float m_CountDown = 3f;
    public string m_GOText = "GO";
    public UI_LAPTIME m_UILaptime;
    public bool m_IsCountdownComplete = false;

    void Awake()
    {
        if (instance == null) {
			instance = this;
		}
    }

    void Start()
    {
        currentSpeedText.text = m_CountDown.ToString();
    }

    void OnEnable()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_CountDown -= Time.deltaTime;
            // Debug.Log(m_CountDown);
        if (m_CountDown > 0) 
        {
            currentSpeedText.text = Mathf.Ceil(m_CountDown).ToString();
        }
        if (m_CountDown <= 0 && m_CountDown > -1)
        {
            currentSpeedText.text = m_GOText;
            m_UILaptime.gameObject.SetActive(true);
            m_IsCountdownComplete = true;
        }
        if (m_CountDown <= -1 && m_CountDown > -2)
        {
            transform.gameObject.SetActive(false);
        }
    }

    void OnDisable()
    {
        m_CountDown = 3;
    }
}
