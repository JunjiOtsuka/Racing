using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProjectileObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static ProjectileObjectPooler Instance;
    public float movementSpeed = 50;
    public float multiplier = 10;
    void Awake() {
        Instance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, List<GameObject>> poolDictionary;

    public GameObject projectileSpawner;
    public GameObject projectileHindSpawner;
    public GameObject projectilePrefab;
    public float m_PoolSize = 0f;
    // public Queue<GameObject> projectileQueue;
    public List<GameObject> projectileList;

	public PhotonView m_PV;

    void Start()
    {
		if (!m_PV.IsMine) 
        {
            PoolProjectileGO();
            return;
        }

        PoolProjectileGO();
    }

    public void PoolProjectileGO()
    {
        poolDictionary = new Dictionary<string, List<GameObject>>();

        foreach (Pool pool in pools) 
        {
            List<GameObject> objectPool = new List<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.GetComponent<ProjectileManager>().PhotonID = m_PV.ViewID;
                // GameObject obj = PhotonNetwork.Instantiate(pool.prefab.name, new Vector3(0, 0, 0), Quaternion.identity, 0);
                if (m_PV.IsMine) 
                {
                    obj.transform.SetParent(this.transform);
                }
                // StartCoroutine(WaitUntilProjectileManagerTrue(obj));
                obj.SetActive(false);
                objectPool.Add(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject GetProjectile(string tag)
    {
        if (!poolDictionary.ContainsKey(tag)) 
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist");
        }

        List<GameObject> objectToSpawn = poolDictionary[tag];

        for(int i = 0; i < objectToSpawn.Count; i++)
        {
            if(!objectToSpawn[i].activeInHierarchy)
            {
                return objectToSpawn[i];
            }
        }

        return null;
    }

    public void SpawnFromPool(string tag)
    {
        var currPro = GetProjectile(tag);
        if (currPro != null)
        {
            currPro.GetComponent<Rigidbody>().velocity = transform.root.gameObject.GetComponent<Rigidbody>().velocity * 1.1f;
            currPro.transform.position = transform.position;
            currPro.transform.rotation = transform.rotation;
            currPro.SetActive(true);
            currPro.GetComponent<ProjectileManager>().GET_PM();
            currPro.GetComponent<ProjectileManager>().DO_PM();
        }
    }
}
