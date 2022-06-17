using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceController : MonoBehaviour
{
    [SerializeField] private ControlsPanel controlsPanel;
    public InputDevice CurrentDevice { get; private set; }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += (device, change) =>
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    controlsPanel.RefreshDropdownList(true, device.displayName);
                    break;
                case InputDeviceChange.Disconnected:
                    controlsPanel.RefreshDropdownList(false, device.displayName);
                    break;
            }
        };
    }
    private void Start()
    {
        SetCurrentDevice(0);
    }
    public void SetCurrentDevice(int deviceIndex)
    {
        List<InputDevice> devices = new List<InputDevice>(InputSystem.devices);

        if (devices.Count == 0) return;

        controlsPanel.SetDropdownValue(deviceIndex);

        if (devices.Count == 1) deviceIndex = 0;
        else if (deviceIndex == 1) deviceIndex = 2;


        if (deviceIndex == 0)
        {
            devices.ForEach(x => InputSystem.DisableDevice(x));

            var mouseAndKeyboard = new List<InputDevice>(devices
                .Where(x => devices.IndexOf(x) == deviceIndex || devices.IndexOf(x) == deviceIndex + 1)
                .Select(x => x.device));

            mouseAndKeyboard.ForEach(x => InputSystem.EnableDevice(x));
        }
        else
        {
            InputSystem.EnableDevice(devices[deviceIndex]);

            for (int i = 0; i < devices.Count; i++)
            {
                if (i == deviceIndex) continue;

                InputSystem.DisableDevice(devices[i]);
            }
        }

        CurrentDevice = devices[deviceIndex];
        controlsPanel.SetCurrentBindingGroup(CurrentDevice);
        controlsPanel.RefreshRebindingTiles();
        MenuController.Instance.SwitchEventSystemForDevice(CurrentDevice);
    }
}
