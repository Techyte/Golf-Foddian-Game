namespace Menu
{
    using UnityEngine;
    using LootLocker.Requests;

    public class LootLockerManager : MonoBehaviour
    {
        private void Start()
        {
            LootLockerSDKManager.StartGuestSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error starting LootLocker session");
                    return;
                }
            
                Debug.Log("successfully started LootLocker session");

                string username = PlayerPrefs.GetString("Username", "Guest");
            
                LootLockerSDKManager.SetPlayerName(username, (nameResponse =>
                {
                    if (!nameResponse.success)
                    {
                        Debug.Log("error setting player name");
                    }
                    else
                    {
                        Debug.Log("set player name");
                    }
                }));
            });
        }
    }   
}