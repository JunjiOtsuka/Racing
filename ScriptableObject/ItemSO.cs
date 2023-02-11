using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemSO", order = 3)]
public class ItemSO : ScriptableObject
{
    public string ItemPrefabName;
    public ItemTypeEnum m_ItemTypeEnum;
    public Material ItemMaterial;
    public Mesh ItemMesh;
    public Sprite ItemImage;

	//Audio
    [Header("ItemAudio")]
	public AudioClip ItemClip;

    
    [Header("Acceleration")]
    public float DriveForce;
	[Header("Handling")]
    public float AngleOfRoll;
	[Header("Max Speed")]
    public float TerminalVelocity;
}
