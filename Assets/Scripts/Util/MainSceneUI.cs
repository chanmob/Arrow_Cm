using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using CodeStage.AntiCheat.ObscuredTypes;

public class MainSceneUI : MonoBehaviour
{
    private bool soundOff;
    private string leaderboardId = "74943547710";

    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    Debug.Log("로그인 성공");
                }
                else
                {
                    Debug.LogError("로그인 실패");
                }
            });
        }

        if (PlayerPrefs.HasKey("SOUND"))
        {
            var sound = PlayerPrefs.GetString("SOUND");

            if (sound == "true")
            {
                soundOff = false;
                AudioListener.volume = 1;
            }
            else if (sound == "false")
            {
                soundOff = true;
                AudioListener.volume = 0;
            }
        }
    }

    public void Like()
    {
        Application.OpenURL("");
    }

    public void SoundOnOff()
    {
        if (soundOff)
        {
            soundOff = false;
            AudioListener.volume = 1;
            PlayerPrefs.SetString("SOUND", soundOff.ToString());
        }

        else
        {
            soundOff = true;
            AudioListener.volume = 0;
            PlayerPrefs.SetString("SOUND", soundOff.ToString());
        }
    }

    public void Ranking()
    {
        float score = 0;
        float uploadScore = 0;

        if (ObscuredPrefs.HasKey("INFINITYSCORE"))
        {
            score = ObscuredPrefs.GetFloat("INFINITYSCORE");            
        }

        if (ObscuredPrefs.HasKey("UPLOADSCORE"))
        {
            uploadScore = ObscuredPrefs.GetFloat("UPLOADSCORE");
        }

        if(uploadScore != score)
        {
            ObscuredPrefs.SetFloat("UPLOADSCORE", score);
            uploadScore = score;

            Social.ReportScore((long)uploadScore, GPGSIds.leaderboard, (bool success) =>
            {
                if (success)
                    PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard);
            });
        }
        else
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard);
        }

    }

    public void Achievements()
    {
        Social.ShowAchievementsUI();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
