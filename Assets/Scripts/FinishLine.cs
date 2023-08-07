using System.Collections.Generic;
using LootLocker.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GolfBallController controller;
    [SerializeField] private GameObject finishUI;
    [SerializeField] private TextMeshProUGUI finishedTimeText;
    [SerializeField] private int defaultScore = 10000;

    [SerializeField] private List<GameObject> leaderBoardPlaces;

    private float time;

    private bool finished;

    private void Update()
    {
        if(!finished)
        {
            time += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Finish();
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
        int score = CalculateScore(time);
        
        LootLockerSDKManager.SubmitScore(memberID, score, leaderBoardKey, (response) =>
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
        finishedTimeText.text = time.ToString();
    }

    private void SetupLeaderBoard(LootLockerLeaderboardMember[] members)
    {
        List<LeaderBoardPosition> positions = new List<LeaderBoardPosition>();
        
        for (int i = 0; i < members.Length; i++)
        {
            LeaderBoardPosition position = new LeaderBoardPosition(members[i].player.name, members[i].score, i+1);
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

    private int CalculateScore(float time)
    {
        float adjustedTime = time * 15;
        float adjustedBallHits = controller.ballHits * 8;

        return (int)(defaultScore - adjustedBallHits * adjustedTime);
    }
    
    private void GetLeaderboard()
    {
        string leaderBoardKey = "mainLeaderBoardKeyHere";
        int count = 10;

        LootLockerLeaderboardMember[] members = new LootLockerLeaderboardMember[0];
        
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
