using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class RebindingTile : MonoBehaviour
{
    [SerializeField] private TMP_Text inputName;
    [SerializeField] private GameObject inputObj;
    [SerializeField] private GameObject waitingForInput;
    [SerializeField] private Button changeKeyButton;

    private PlayerControls playerControls;
    private ControlsPanel controlsPanel;

    public TMP_Text actionName;
    public InputAction action;
    public InputBinding inputBinding;
    public int bindingIndex;

    private void Awake()
    {
        if (playerControls == null)
            playerControls = new PlayerControls();

        controlsPanel = FindObjectOfType<ControlsPanel>();
    }
    private void Start()
    {
        Load();

        if (action != null)
        {
            if (inputBinding.isPartOfComposite) actionName.SetText(action.name + " " + inputBinding.name);
            else actionName.SetText(action.name);

            inputName.text = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }
    }
    public void SetStartValues(InputAction action, InputBinding inputBinding, int bindingIndex)
    {
        this.action = action;
        this.inputBinding = inputBinding;
        this.bindingIndex = bindingIndex;
    }
    public void RebindInput()
    {
        if (!controlsPanel.IsRebinding)
        {
            controlsPanel.IsRebinding = true;
            inputObj.SetActive(false);
            waitingForInput.SetActive(true);

            var rebind = action.PerformInteractiveRebinding(bindingIndex);

            bool mouseExcluded = actionName.text != "Aim" && actionName.text != "Shoot" && actionName.text != "Look";
            if (mouseExcluded) rebind.WithControlsExcluding("Mouse");

            rebind.OnComplete(operation =>
            {
                inputName.text = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
                inputObj.SetActive(true);
                waitingForInput.SetActive(false);
                rebind.Dispose();
                controlsPanel.IsRebinding = false;
            });

            rebind.Start();
            Save();
            controlsPanel.ResetButton(changeKeyButton);
        }
    }
    public void ResetInputToDefault()
    {
        action.RemoveBindingOverride(bindingIndex);
        inputName.text = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        Save();
    }
    public void Save()
    {
        controlsPanel.SaveInput(action, bindingIndex);
    }
    public void Load()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + bindingIndex)))
            controlsPanel.LoadInput(action, bindingIndex);
        else
            ResetInputToDefault();
    }
}
