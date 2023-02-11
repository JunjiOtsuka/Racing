using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CharacterData", menuName = "ScriptableObjects/CharacterSO", order = 1)]
public class CharacterSO : ScriptableObject
{
    public string CharacterPrefabName;
    public GameObject CharacterModel;
    public Material CharacterMaterial;
    public Mesh CharacterMesh;
    public Sprite CharacterImage;
    public Avatar CharacterAvatar;

    [Header("CharAudio")]
    public AudioClip TacticalClip;
	public AudioClip UltimateClip;

    void OnEnable()
    {
        if (!CharacterModel) return;
        CharacterPrefabName = CharacterModel.name;
    }
}
