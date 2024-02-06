namespace Game
{
    using UnityEngine;

    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("Music", 1) == 1)
            {
                source.volume = PlayerPrefs.GetFloat("Volume", 0.5f);
                source.Play();
            }
        }
    }   
}