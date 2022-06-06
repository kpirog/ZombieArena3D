using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class RebindingTile : MonoBehaviour
{
    [SerializeField] private TMP_Text inputName;
    [SerializeField] private GameObject inputObj;
    [SerializeField] private GameObject waitingForInput;

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
    }
    private void Start()
    {
        LoadInput();
        
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

        });

        rebind.Start();
        SaveInput();
    }
    public void ResetInputToDefault()
    {
        action.RemoveBindingOverride(bindingIndex);
        inputName.text = InputControlPath.ToHumanReadableString(action.bindings[bindingIndex].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        SaveInput();
    }
    private void SaveInput()
    {
        PlayerPrefs.SetString(action.actionMap + action.name + bindingIndex, inputBinding.overridePath); //nie dziala
        PlayerPrefs.Save();
    }
    public void LoadInput()
    {
        if (!string.IsNullOrEmpty(action.actionMap + action.name + bindingIndex))
        {
            Debug.Log("JEST");
        }
        else
        {
            ResetInputToDefault();
        }
    }
}
