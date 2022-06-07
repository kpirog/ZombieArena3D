using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class InputFieldHandler : MonoBehaviour
{
    [SerializeField] private ScreenKeyboard screenKeyboard;
    private TMP_InputField inputField;
    private bool isSelected;

    private PlayerControls playerControls;
    private DeviceController deviceController;

    private void Awake()
    {
        playerControls = new PlayerControls();
        deviceController = FindObjectOfType<DeviceController>();
        inputField = GetComponent<TMP_InputField>();
    }
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == inputField.gameObject)
        {
            if (!isSelected)
            {
                playerControls.UI.Submit.Enable();
                playerControls.UI.Cancel.Disable();
                playerControls.UI.Submit.performed += ctx => Select();
            }
            else
            {
                playerControls.UI.Submit.Disable();
                playerControls.UI.Cancel.Enable();
                playerControls.UI.Cancel.performed += ctx => DeSelect();
            }
        }
    }
    private void Select()
    {
        isSelected = true;
        inputField.interactable = true;
        playerControls.UI.Submit.Disable();

        if (deviceController.CurrentDevice == Keyboard.current)
        {
            inputField.Select();
        }
        else
        {
            screenKeyboard.gameObject.SetActive(true);
            screenKeyboard.keys[0].button.Select();
        }
    }
    public void DeSelect()
    {
        if (inputField != null)
        {
            isSelected = false;
            inputField.interactable = false;
            screenKeyboard.gameObject.SetActive(false);
        }
    }
    public void QuitInputField()
    {
        inputField.interactable = true;
        playerControls.UI.Submit.performed -= ctx => Select();
        playerControls.UI.Cancel.performed -= ctx => DeSelect();
        playerControls.UI.Submit.Disable();
        playerControls.UI.Cancel.Disable();
    }
    public void AddLetter(string letter)
    {
        inputField.text += letter;
    } 
    public void RemoveLetter()
    {
        if (inputField.text == string.Empty) return;
        
        List<char> currentLetters = new List<char>(inputField.text.ToCharArray());
        currentLetters.RemoveAt(currentLetters.Count - 1);
        char[] currentLettersArray = currentLetters.ToArray();  
        inputField.text = currentLettersArray.ArrayToString();
    }
    public void ClearInputField()
    {
        inputField.text = string.Empty;
    }
}
