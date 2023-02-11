using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTESTSPAWN : MonoBehaviour
{
    public List<GameObject> m_PlayerList;
    public List<GameObject> m_SpawnPointList;


    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            m_SpawnPointList.Add(child.gameObject);
        }
        foreach (GameObject Player in m_PlayerList)
        {
            SetSpawnPoint(Player);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetSpawnPoint(GameObject Player)
    {
        var RandomNumber = Random.Range(0, m_SpawnPointList.Count);
		Player.transform.position = m_SpawnPointList[RandomNumber].transform.position;
		Player.transform.rotation = m_SpawnPointList[RandomNumber].transform.rotation;
        m_SpawnPointList.RemoveAt(RandomNumber);
    }
}
