using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;

public class GoogleTest : MonoBehaviour
{
    public Text loginText;
    public Text leaderBoard;

    public void Awake()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void LoginButton()
    {
        if (Social.localUser.authenticated)
        {
            loginText.text = "이미 로그인 : " + Social.localUser.userName;
        }
        else
        {
            Social.localUser.Authenticate((bool success) => 
            {
                if (success)
                {
                    loginText.text = "로그인 성공 : " + Social.localUser.userName;
                }
                else
                {
                    loginText.text = "로그인 실패";
                }
            });
        }
    }

    public void ShowAchievement()
    {
        Social.ShowAchievementsUI();
    }

    public void ShowLeaderBoard()
    {
        if (Social.localUser.authenticated)
        {
            float score = 0;
            Social.ReportScore((long)score, "", (bool success) =>
            {
                if (success)
                    PlayGamesPlatform.Instance.ShowLeaderboardUI("");
                else
                    leaderBoard.text = "리더보드 실패";
            });
        }
        else
        {
            loginText.text = "리더보드 로그인 실패";
        }
    }
}
