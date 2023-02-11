using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointList : MonoBehaviour
{
    public List<GameObject> List_CheckPoint;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform) {
            List_CheckPoint.Add(child.gameObject);
        }
    }
}
