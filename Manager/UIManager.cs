using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UIManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject m_GameManager;
    public List<GameObject> m_UIList;
    public bool UIActive = false;

    public PhotonView m_PV;

    void Start()
    {
        m_PV = transform.root.GetComponent<PhotonView>();
        if (!m_PV.IsMine) transform.gameObject.SetActive(false); 
    }

    void Update()
    {
        if (UIActive) return;
        if (GameManager.instance.m_PopListComplete)
        {
            DisableUIList();
        }
        if (UI_CountdownManager.instance.m_IsCountdownComplete)
        {
            EnableUIList();
            UIActive = true;
        }
    }

    public void EnableUIList()
    {
        foreach (GameObject UIElement in m_UIList)
        {
            UIElement.SetActive(true);
        }
    }

    public void DisableUIList()
    {
        foreach (GameObject UIElement in m_UIList)
        {
            UIElement.SetActive(false);
        }
    }
}
