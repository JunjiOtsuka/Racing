using UnityEngine;

public class CameraSpawner : MonoBehaviour
{
    void OnEnable()
    {
        CreateNewCamera();
    }

    public void CreateNewCamera()
	{
		GameObject newCam = new GameObject("MainCamera");
		newCam.transform.SetParent(GameObject.Find("Me").transform);
 		newCam.AddComponent<Camera>();
 		newCam.AddComponent<AudioListener>();
		var cineBrain = newCam.AddComponent<Cinemachine.CinemachineBrain>();
		cineBrain.m_DefaultBlend.m_Time = 0;
	}
}
