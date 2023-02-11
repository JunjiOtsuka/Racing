using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject NormalCam;
    public GameObject BackMirrorCam;

    // Update is called once per frame
    void Update()
    {
        if (MyPlayerInput.BackMirrorKey.IsPressed())
        {
            NormalCam.SetActive(false);
            BackMirrorCam.SetActive(true);
        } else {
            NormalCam.SetActive(true);
            BackMirrorCam.SetActive(false);
        }
    }
}
