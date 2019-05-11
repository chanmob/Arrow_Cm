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
    public UnityEngine.UI.Text txt;

    private void Awake()
    {
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

        txt.text = "Awake 완료";
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
        Social.localUser.Authenticate(AuthenticateHandler);
        txt.text += "\n랭킹 누름";
    }

    public void Achievements()
    {
        txt.text += "\n업적 누름";
        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Social.ShowAchievementsUI();
            }
            else
            {
                txt.text += "\n실패";
            }
        });
    }

    public void Quit()
    {
        if (Social.localUser.authenticated)
        {
            txt.text = "name : " + Social.localUser.userName; 
        }
        else
        {
            Social.localUser.Authenticate((bool suc) =>{
            if (suc)
                {
                    txt.text = "로그인 성공";
                }
                else
                {
                    txt.text = "로그인 실패";
                }
            });
        }
        //Application.Quit();
    }

    private void AuthenticateHandler(bool _sucess)
    {
        //if (_sucess)
        //{
        //    float timeScore = 0f;

        //    if (ObscuredPrefs.HasKey("INFINITYSCORE"))
        //    {
        //        timeScore = ObscuredPrefs.GetFloat("INFINITYSCORE");
        //    }

        //    Social.ReportScore((long)timeScore, GPGSIds.leaderboard_test_leaderboard, (bool sucess) => {
        //        if (sucess)
        //        {
        //            PlayGamesPlatform.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_test_leaderboard);
        //            txt.text += "\n랭킹 받기";
        //        }
        //        else
        //        {
        //            Debug.LogError("HighScore Failed");
        //            txt.text += "\n랭킹 받기 실패";
        //        }
        //    });
        //}
        //else
        //{
        //    Debug.Log("Login Failed");
        //    txt.text += "\n로그인 실패";
        //}
    }
}
