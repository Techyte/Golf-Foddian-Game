using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject aboutMenu;

    public void OpenMain()
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
    }

    public void OpenAbout()
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }
}
