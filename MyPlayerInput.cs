//This script handles reading inputs from the player and passing it on to the vehicle. We 
//separate the input code from the behaviour code so that we can easily swap controls 
//schemes or even implement and AI "controller". Works together with the VehicleMovement script

using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class MyPlayerInput : MonoBehaviour
{
	//We hide these in the inspector because we want 
	//them public but we don't want people trying to change them
	[HideInInspector] public float thruster;			//The current thruster value
	[HideInInspector] public float rudder;				//The current rudder value
	[HideInInspector] public bool isBraking;			//The current brake value
	[HideInInspector] public bool Tactical;			
	[HideInInspector] public bool Ultimate;			
	[HideInInspector] public bool Item;			

	public static InputAction MoveKey;
    public static InputAction BackMirrorKey;
    public static InputAction TacticalKey;
    public static InputAction UltimateKey;
    public static InputAction ItemKey;
    public static InputAction GasKey;
    public static InputAction BrakeKey;
    public static InputAction ShopMenuKey;
    public static InputAction OptionsMenuKey;
    // public static InputAction leftClick;
    // public static InputAction rightClick;
    // public static InputAction options;
    // public static InputAction Weapon1;
    // public static InputAction Weapon2;

	public PhotonView m_PV;

	private void Awake()
    {
		if (!m_PV.IsMine) return;
        //movement related variables
        MoveKey = MyInputActionManager.inputActions.Player.Move;
        BackMirrorKey = MyInputActionManager.inputActions.Player.Look;
        TacticalKey = MyInputActionManager.inputActions.Player.Tactical;
        UltimateKey = MyInputActionManager.inputActions.Player.Ultimate;
        ItemKey = MyInputActionManager.inputActions.Player.Item;
        GasKey = MyInputActionManager.inputActions.Player.Gas;
        BrakeKey = MyInputActionManager.inputActions.Player.Brake;
        ShopMenuKey = MyInputActionManager.inputActions.Player.Shop;
        OptionsMenuKey = MyInputActionManager.inputActions.Player.OptionsMenu;
        
        // interactClick = InputManager.inputActions.Player.Interact;
        // options = InputManager.inputActions.Player.Options;
        // Weapon1 = InputManager.inputActions.Player.Weapon1;
        // Weapon2 = InputManager.inputActions.Player.Weapon2;
        // InputManager.inputActions.Player.MouseX.performed += ctx => mouseInput.x = ctx.ReadValue<float>();
        // InputManager.inputActions.Player.MouseY.performed += ctx => mouseInput.y = ctx.ReadValue<float>();
        // InputManager.inputActions.Player.Run.performed += DoRun;
        // InputManager.inputActions.Player.Run.canceled += DoWalk;
    }

	void Update()
	{
		if (!m_PV.IsMine) return;
		//If the player presses the Escape key and this is a build (not the editor), exit the game
		if (Input.GetButtonDown("Cancel") && !Application.isEditor)
			Application.Quit();

		// //If a GameManager exists and the game is not active...
		// if (GameManager.instance != null && !GameManager.instance.IsActiveGame())
		// {
		// 	//...set all inputs to neutral values and exit this method
		// 	thruster = rudder = 0f;
		// 	isBraking = false;
		// 	return;
		// }

        if (OptionsMenuKey.WasPressedThisFrame())
        {
            transform.Find("UI/UIOptionsMenu").gameObject.SetActive(true);
        }

		//Get the values of the thruster, rudder, and brake from the input class
		thruster = MoveKey.ReadValue<Vector2>().y;
		rudder = MoveKey.ReadValue<Vector2>().x;
	}
}