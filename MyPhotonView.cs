using UnityEngine;
using Photon.Pun;

public class MyPhotonView : MonoBehaviour
{
    public static PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
    }
}
