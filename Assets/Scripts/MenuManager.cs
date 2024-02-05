using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject onlineMenu;
    [SerializeField] private GameObject aboutMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject joinMenu;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private float backgroundScrollSpeed = 1f;
    
    [Space]
    [Header("Main Menu Assignments")]
    [SerializeField] private TMP_InputField usernameInputField;

    [Space] 
    [Header("Options Menu Assignments")] 
    [SerializeField] private Slider MouseDistanceAtMax;
    [SerializeField] private Slider Volume;
    [SerializeField] private Toggle Music;
    [SerializeField] private Toggle SoundEffects;
    [SerializeField] private Toggle Background;

    [Space] 
    [Header("Join Menu Assignments")] 
    [SerializeField] private TMP_InputField IpText;
    [SerializeField] private TMP_InputField PortText;
    
    private bool showBackground = true;

    private void Start()
    {
        backgroundImage.material.mainTextureOffset = Vector2.zero;
        backgroundImage.material = new Material(backgroundImage.material);
        
        string username = PlayerPrefs.GetString("Username", "Guest");
        float mouseDistanceAtMax = PlayerPrefs.GetFloat("MouseDistanceAtMax", 8);
        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        bool music = PlayerPrefs.GetInt("Music", 1) == 1;
        bool soundEffects = PlayerPrefs.GetInt("SoundEffects", 1) == 1;
        bool background = PlayerPrefs.GetInt("Background", 1) == 1;

        showBackground = background;

        usernameInputField.contentType = TMP_InputField.ContentType.Standard;
        usernameInputField.text = username;
        usernameInputField.onValueChanged.AddListener(UsernameChanged);
        
        MouseDistanceAtMax.value = mouseDistanceAtMax;
        MouseDistanceAtMax.onValueChanged.AddListener(DistanceChanged);

        Volume.value = volume;
        Volume.onValueChanged.AddListener(VolumeChanged);

        Music.isOn = music;
        Music.onValueChanged.AddListener(MusicChanged);

        SoundEffects.isOn = soundEffects;
        SoundEffects.onValueChanged.AddListener(SoundEffectsChanged);

        Background.isOn = background;
        Background.onValueChanged.AddListener(BackgroundChanged);
    }

    private void LateUpdate()
    {
        if(showBackground)
        {
            backgroundImage.enabled = true;
            backgroundImage.material.mainTextureOffset += new Vector2(0, Time.deltaTime * (backgroundScrollSpeed / 10));
        }
        else
        {
            backgroundImage.enabled = false;
        }
    }

    public void UsernameChanged(string value)
    {
        PlayerPrefs.SetString("Username", value);
    }

    public void DistanceChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseDistanceAtMax", value);
    }

    public void VolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("Volume", value);
    }
    
    public void MusicChanged(bool value)
    {
        PlayerPrefs.SetInt("Music", value ? 1 : 0);
    }
    
    public void SoundEffectsChanged(bool value)
    {
        PlayerPrefs.SetInt("SoundEffects", value ? 1 : 0);
    }
    
    public void BackgroundChanged(bool value)
    {
        PlayerPrefs.SetInt("Background", value ? 1 : 0);
        showBackground = value;
    }

    public void OpenMain()
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(false);
        onlineMenu.SetActive(false);
        joinMenu.SetActive(false);
    }

    public void OpenAbout()
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(true);
        optionsMenu.SetActive(false);
        onlineMenu.SetActive(false);
        joinMenu.SetActive(false);
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(true);
        onlineMenu.SetActive(false);
        joinMenu.SetActive(false);
    }

    public void OpenOnline()
    {
        onlineMenu.SetActive(true);
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(false);
        joinMenu.SetActive(false);
    }

    public void OpenJoin()
    {
        onlineMenu.SetActive(false);
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        optionsMenu.SetActive(false);
        joinMenu.SetActive(true);
    }

    public void JoinGame()
    {
        NetworkManager.Instance.StartAsClient($"{IpText.text}:{PortText.text}");
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
