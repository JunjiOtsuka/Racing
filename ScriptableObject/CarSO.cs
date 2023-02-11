using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CarData", menuName = "ScriptableObjects/CarSO", order = 2)]
public class CarSO : ScriptableObject
{
    public string CarPrefabName;
    public GameObject CarModel;
    public Material CarMaterial;
    public Mesh CarMesh;
    public Sprite CarImage;

	//Audio
    [Header("CarAudio")]
	public AudioClip CarClip;

    
    [Header("Acceleration")]
    public float DriveForce;
	[Header("Handling")]
    public float AngleOfRoll;
	[Header("Max Speed")]
    public float TerminalVelocity;

    void OnEnable()
    {
        if (!CarModel) return;
        CarPrefabName = CarModel.name;
    }
}
