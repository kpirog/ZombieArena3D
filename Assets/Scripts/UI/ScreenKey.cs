using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ScreenKey : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public TMP_Text letterText;
    public Button button;

    public bool isLetterKey = true;
    [SerializeField] private bool isNumberKey = false;
    [SerializeField] private bool isSymbolKey = false;
    [SerializeField] private bool isSpaceKey = false;

    private InputFieldHandler inputFieldHandler;
    private ScreenKeyboard screenKeyboard;

    private void Awake()
    {
        letterText = GetComponentInChildren<TMP_Text>();
        button = GetComponent<Button>();
        inputFieldHandler = FindObjectOfType<InputFieldHandler>();
        screenKeyboard = FindObjectOfType<ScreenKeyboard>();
    }
    private void OnEnable()
    {
        if (isLetterKey || isNumberKey || isSymbolKey) button.onClick.AddListener(WriteLetter);
    }
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
    public void OnDeselect(BaseEventData eventData)
    {
        letterText.color = Color.white;
    }

    public void OnSelect(BaseEventData eventData)
    {
        letterText.color = Color.green;
    }
    public void WriteLetter()
    {
        if(screenKeyboard.isFirstOpen)
        {
            screenKeyboard.isFirstOpen = false;
            return;
        }

        if(isSpaceKey) inputFieldHandler.AddLetter(" ");
        else inputFieldHandler.AddLetter(letterText.text);
    }
    public void ChangeLetterSize(bool isLetterSmall)
    {
        if (isLetterSmall) letterText.text = letterText.text.ToUpper();
        else letterText.text = letterText.text.ToLower();
    }
}
