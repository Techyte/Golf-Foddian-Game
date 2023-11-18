using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject aboutMenu;
    [SerializeField] private GameObject optionsMenu;
    
    [Space]
    [Header("Main Menu Assignments")]
    [SerializeField] private TMP_InputField usernameInputField;

    [Space] 
    [Header("Options Menu Assignments")] 
    [SerializeField]
    private Slider MouseDistanceAtMax;

    private void Start()
    {
        string username = PlayerPrefs.GetString("Username", "Guest");
        int mouseDistanceAtMax = PlayerPrefs.GetInt("MouseDistanceAtMax", 8);

        usernameInputField.text = username;
        usernameInputField.onValueChanged.AddListener(UsernameChanged);
        MouseDistanceAtMax.value = mouseDistanceAtMax;
        MouseDistanceAtMax.onValueChanged.AddListener(DistanceChanged);
    }

    public void UsernameChanged(string value)
    {
        PlayerPrefs.SetString("Username", value);
    }

    public void DistanceChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseDistanceAtMax", value);
    }

    public void OpenMain()
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    public void OpenAbout()
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(true);
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
