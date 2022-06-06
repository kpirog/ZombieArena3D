using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenKeyboard : MonoBehaviour
{
    public List<ScreenKey> keys;
    
    [HideInInspector] public bool isFirstOpen;
    private bool areLettersSmall = true;

    [SerializeField] private Button startGameButton;
    private InputFieldHandler inputFieldHandler;

    private void Awake()
    {
        inputFieldHandler = FindObjectOfType<InputFieldHandler>();
    }
    private void OnEnable()
    {
        if(keys.Count == 0)
            keys = new List<ScreenKey>(GetComponentsInChildren<ScreenKey>());

        isFirstOpen = true;
    }
    public void ChangeSizeOfLetters()
    {
        foreach (ScreenKey key in keys)
        {
            if (key.isLetterKey) key.ChangeLetterSize(areLettersSmall);
        }

        areLettersSmall = !areLettersSmall;
    }
    public void RemoveLetter()
    {
        inputFieldHandler.RemoveLetter();
    }
    public void ClearText()
    {
        inputFieldHandler.ClearInputField();
    }
    public void ConfirmText()
    {
        startGameButton.Select();
        inputFieldHandler.DeSelect(); 
        inputFieldHandler.QuitInputField();
    }
}
