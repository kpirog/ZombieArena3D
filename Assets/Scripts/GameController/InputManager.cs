using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerControls playerControls;
    private PlayerInput playerInput;
    private MovementStateManager movementStateManager;

    public string CurrentBindingGroup => movementStateManager.startControlScheme == "Gamepad" ? "Gamepad" : "Keyboard&Mouse";

    private void Awake()
    {
        if (playerControls == null)
            playerControls = new PlayerControls();

        movementStateManager = FindObjectOfType<MovementStateManager>();
        playerInput = movementStateManager.GetComponent<PlayerInput>();
    }
    private void OnEnable()
    {
        playerControls.UI.Disable();   
        LoadInputs();
    }
    private void LoadInputs()
    {
        playerInput.actions.Disable();
        InputActionMap actionMap = playerInput.actions.actionMaps[0];
        List<InputAction> actions = actionMap.actions.ToList();

        for (int i = 0; i < actions.Count; i++)
        {
            List<InputBinding> bindings = actions[i].bindings.Where(x => x.action == actions[i].name).ToList();

            for (int j = 0; j < bindings.Count; j++)
            {
                if (bindings[j].groups != CurrentBindingGroup) continue; 
                
                if (!string.IsNullOrEmpty(PlayerPrefs.GetString(actions[i].actionMap + actions[i].name + j)))
                {
                    actions[i].ApplyBindingOverride(j, PlayerPrefs.GetString(actions[i].actionMap + actions[i].name + j));
                }
            }
        }

        playerInput.actions.Enable();
    }
}
