namespace Game
{
    using System.Collections.Generic;
    using LootLocker.Requests;
    using Multiplayer;
    using TMPro;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class FinishLine : MonoBehaviour
    {
        [SerializeField] private GolfBallController controller;
        [SerializeField] private GameObject finishUI;
        [SerializeField] private TextMeshProUGUI finishedTimeText;

        [SerializeField] private List<GameObject> leaderBoardPlaces;

        private float _time;

        public static bool finished;

        private void Update()
        {
            if(!finished)
            {
                _time += Time.deltaTime;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (NetworkManager.Instance.playingOnline)
            {
                col.transform.position = PlayerManager.Instance.PlayerStartPosition.position;
            }
            else
            {
                Finish();
            }
        }

        private void Finish()
        {
            finished = true;
            controller.SetCanHit(false);
            finishUI.SetActive(true);
            SendScore();
        }

        private void SendScore()
        {
            string memberID = "";
            string leaderBoardKey = "mainLeaderBoardKeyHere";

            int storedTime = (int)(_time * 1000);
            
            Debug.Log(storedTime);

            LootLockerSDKManager.SubmitScore(memberID, -storedTime, leaderBoardKey, (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("score submission successful");
                    GetLeaderboard();
                }
                else
                {
                    Debug.Log("score submission unsuccessful: " + response.Error);
                }
            });
        }

        private void SetUI(LootLockerLeaderboardMember[] members)
        {
            SetupLeaderBoard(members);
            finishedTimeText.text = _time.ToString();
        }

        private void SetupLeaderBoard(LootLockerLeaderboardMember[] members)
        {
            List<LeaderBoardPosition> positions = new List<LeaderBoardPosition>();
            
            for (int i = 0; i < members.Length; i++)
            {
                Debug.Log(members[i].score);
                float convertedTime = members[i].score / 1000f;
                Debug.Log(convertedTime);
                
                LeaderBoardPosition position = new LeaderBoardPosition(members[i].player.name, -convertedTime, i+1);
                positions.Add(position);
            }
            
            for (int i = 0; i < leaderBoardPlaces.Count; i++)
            {
                GameObject currentPlace = leaderBoardPlaces[i];
                
                if (i + 1 > positions.Count)
                {
                    currentPlace.SetActive(false);
                    continue;
                }

                TextMeshProUGUI usernameText = currentPlace.GetComponentsInChildren<TextMeshProUGUI>()[0];
                TextMeshProUGUI timeText = currentPlace.GetComponentsInChildren<TextMeshProUGUI>()[1];

                usernameText.text = positions[i].username;
                timeText.text = $"Time: {positions[i].time}";
            }
        }

        private void GetLeaderboard()
        {
            string leaderBoardKey = "mainLeaderBoardKeyHere";
            int count = 10;
            
            LootLockerSDKManager.GetScoreList(leaderBoardKey, count, 0, (response) =>
            {
                if (response.statusCode == 200)
                {
                    Debug.Log("got the leaderboard info");

                    SetUI(response.items);
                }
                else
                {
                    Debug.Log("failed to get the leaderboard info: " + response.Error);
                }
            });
        }

        public void Menu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public class LeaderBoardPosition
    {
        public string username;
        public float time;
        public int place;

        public LeaderBoardPosition(string username, float time, int place)
        {
            this.username = username;
            this.time = time;
            this.place = place;
        }
    }   
}