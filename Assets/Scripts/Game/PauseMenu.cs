namespace Game
{
    using Multiplayer;
    using Riptide;
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

        public void Toggle()
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
            if (NetworkManager.Instance.playingOnline)
            {
                NetworkManager.Instance.client.Disconnect();
                NetworkManager.Instance.server.Stop();
                NetworkManager.Instance.playingOnline = false;
                NetworkManager.Instance.server = new Server();
                NetworkManager.Instance.client = new Client();
            }
            
            SceneManager.LoadScene("MainMenu");
        }
    }   
}