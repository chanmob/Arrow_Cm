using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using CodeStage.AntiCheat.ObscuredTypes;

public class MainSceneUI : MonoBehaviour
{
    private bool soundOff;
    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
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
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.ChanMob.DodgeArrow");
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
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    ShowRankig();
                }
                else
                {
                    Debug.LogError("로그인 실패");
                }
            });
        }
        else
        {
            ShowRankig();
        }
    }

    private void ShowRankig()
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

        if (uploadScore != score)
        {
            ObscuredPrefs.SetFloat("UPLOADSCORE", score);
            uploadScore = score;

            Social.ReportScore((long)uploadScore, GPGSIds.leaderboard, (bool leaderBoardSuccess) =>
            {
                if (leaderBoardSuccess)
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
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool sucess) =>
            {
                if (sucess)
                    ShowAchievements();
                else
                    Debug.LogError("로그인 실패");
            });
        }
        else
        {
            ShowAchievements();
        }
    }

    private void ShowAchievements()
    {
        Social.ShowAchievementsUI();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
