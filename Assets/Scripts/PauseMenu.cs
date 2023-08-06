using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    
    [SerializeField] private GameObject pauseMenu;
    public bool _isOn;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Toggle();
        }
    }

    private void Toggle()
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        _isOn = !_isOn;
    }

    public void Resume()
    {
        Toggle();
    }

    public void Leave()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
