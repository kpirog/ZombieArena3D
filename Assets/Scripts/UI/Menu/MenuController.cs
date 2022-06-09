using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance { get; private set; }
    
    [SerializeField] private EventSystem gamepadEventSystem;
    [SerializeField] private EventSystem keyboardEventSystem;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of this class");
        }

        Instance = this;
    }
    private void Start()
    {
        keyboardEventSystem.gameObject.SetActive(!gamepadEventSystem.gameObject.activeInHierarchy);
        gamepadEventSystem.gameObject.SetActive(!keyboardEventSystem.gameObject.activeInHierarchy);
    }
    public void SwitchEventSystemForDevice(InputDevice device)
    {
        if (device == Keyboard.current)
        {
            keyboardEventSystem.gameObject.SetActive(true);
            gamepadEventSystem.gameObject.SetActive(false);
            EventSystem.current = keyboardEventSystem;
        }
        else
        {
            gamepadEventSystem.gameObject.SetActive(true);
            keyboardEventSystem.gameObject.SetActive(false);
            EventSystem.current = gamepadEventSystem;
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
