using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointList : MonoBehaviour
{
    public List<GameObject> List_SpawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            List_SpawnPoint.Add(child.gameObject);
        }
    }
}
