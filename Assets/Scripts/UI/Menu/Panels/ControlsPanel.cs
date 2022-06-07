using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ControlsPanel : MonoBehaviour, ISettingsPanel
{
    public TMP_Dropdown deviceDropdown;
    [SerializeField] private GameObject panel;
    [SerializeField] private Scrollbar actionScrollbar;
    [SerializeField] private RebindingTile rebindingTilePrefab;
    [SerializeField] private Transform rebindingTileParent;
    [SerializeField] private List<Button> actionButtons;
    private List<RebindingTile> rebindingTilesList = new List<RebindingTile>();

    private List<string> deviceNames = new List<string>();
    private DeviceController deviceController;
    private PlayerControls playerControls;
    private InputActionMap playerActionMap;
    private InputAction scrollAction;

    private const string KeyboardMouseBindingGroup = "Keyboard&Mouse";
    private const string GamepadBindingGroup = "Gamepad";
    [HideInInspector] public string currentBindingGroup;
    public bool IsRebinding { get; set; } = false;

    private void Awake()
    {
        FillDeviceDropdown();
        playerControls = new PlayerControls();
        playerActionMap = playerControls.Player;
        scrollAction = playerControls.UI.Scroll;
        deviceController = FindObjectOfType<DeviceController>();
    }
    private void Start()
    {
        CreateRebindingTiles();
        playerControls.UI.Enable();
        scrollAction.performed += ctx => actionScrollbar.value += (ctx.ReadValue<Vector2>().y * (deviceController.CurrentDevice == Keyboard.current ? 0.001f : 0.05f));
    }
    private void FillDeviceDropdown()
    {
        foreach (InputDevice device in InputSystem.devices)
        {
            deviceNames.Add(device.displayName);
        }

        deviceNames[0] += " + " + deviceNames[1];
        deviceNames.RemoveAt(1);
        deviceNames.Sort();
        deviceDropdown.AddOptions(deviceNames);
    }
    public void RefreshDropdownList(bool isNewDeviceAdded, string deviceName)
    {
        if (isNewDeviceAdded) deviceNames.Add(deviceName);
        else deviceNames.Remove(deviceName);

        deviceDropdown.ClearOptions();
        deviceDropdown.AddOptions(deviceNames);
        deviceDropdown.RefreshShownValue();
    }

    public void CreateRebindingTiles()
    {
        List<InputAction> actions = playerActionMap.actions.ToList();

        for (int i = 0; i < actions.Count; i++)
        {
            List<InputBinding> bindings = actions[i].bindings.Where(x => x.action == actions[i].name).ToList();
            
            for (int j = 0; j < bindings.Count; j++)
            {
                if (bindings[j].groups != currentBindingGroup) continue;

                RebindingTile tile = Instantiate(rebindingTilePrefab, transform.position, Quaternion.identity, rebindingTileParent);
                tile.SetStartValues(actions[i], bindings[j], j);
                rebindingTilesList.Add(tile);
            }
        }
    }
    public void RefreshRebindingTiles()
    {
        if (rebindingTilesList.Count > 0)
        {
            rebindingTilesList.ForEach(x => Destroy(x.gameObject));
            rebindingTilesList.Clear();
        }

        CreateRebindingTiles();
    }
    public void SetDropdownValue(int value)
    {
        deviceDropdown.value = value;
        deviceDropdown.RefreshShownValue();
    }
    public void SetScrollbarValue(float value)
    {
        actionScrollbar.value += value;
    }
    public void SetCurrentBindingGroup(InputDevice device)
    {
        if (device == Gamepad.current) currentBindingGroup = GamepadBindingGroup;
        else currentBindingGroup = KeyboardMouseBindingGroup;
    }

    public void ApplySettings()
    {
        PlayerPrefs.SetString("controlScheme", currentBindingGroup);
    }

    public void ResetSettings()
    {
        foreach (RebindingTile tile in rebindingTilesList)
        {
            tile.ResetInputToDefault();
        }
    }
    public void SaveInput(InputAction action, int bindingIndex)
    {
        PlayerPrefs.SetString(action.actionMap + action.name + bindingIndex, action.bindings[bindingIndex].overridePath);
        PlayerPrefs.Save();
    }
    public void LoadInput(InputAction action, int bindingIndex)
    {
        action.ApplyBindingOverride(bindingIndex, PlayerPrefs.GetString(action.actionMap + action.name + bindingIndex));
    }
    public void ResetButton(Button button)
    {
        button.interactable = false;
        button.interactable = true;
        button.Select();
    }
}
