using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MyInputActionManager : MonoBehaviour
{
    public static MyInputActions inputActions = new MyInputActions();
    public static event Action<InputActionMap> actionMapChange;

    public static void ToggleActionMap (InputActionMap actionMap) 
    {
        if (actionMap.enabled) return;

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    } 
    
    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable() 
    {
        inputActions.Disable();
    }
}
