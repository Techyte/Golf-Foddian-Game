using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject aboutMenu;
    [SerializeField] private TMP_InputField usernameInputField;

    private void Start()
    {
        string username = PlayerPrefs.GetString("Username", "Guest");

        usernameInputField.text = username;
        usernameInputField.onValueChanged.AddListener(UsernameChanged);
    }

    public void UsernameChanged(string value)
    {
        PlayerPrefs.SetString("Username", value);
    }

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
