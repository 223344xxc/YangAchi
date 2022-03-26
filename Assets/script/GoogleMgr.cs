using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GoogleMgr : MonoBehaviour
{
    private static GoogleMgr GgMgr = null;
    public static GoogleMgr googleMgr
    {
        get
        {
            if (GgMgr == null)
            {
                GgMgr = FindObjectOfType(typeof(GoogleMgr)) as GoogleMgr;
                if (GgMgr == null)
                {
                    GameObject obj = new GameObject("GoogleMgr");
                    GgMgr = obj.AddComponent(typeof(GoogleMgr)) as GoogleMgr;
                }
            }
            return GgMgr;
        }
    }
    void Awake()
    {
        //PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        //PlayGamesPlatform.DebugLogEnabled = true;
        //PlayGamesPlatform.Activate();
        DontDestroyOnLoad(this);
    }
    void Start()
    {
        OnLogin();
    }
    void Update()
    {

    }

    public void OnLogin()
    {
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool bSuccess) =>
            {
                if (bSuccess)
                {
                    Debug.Log("Success : " + Social.localUser.userName);
                }
                else
                {
                    Debug.Log("Login Fall");
                }
            });
        }
    }

    public void OnLogOut()
    {
        //((PlayGamesPlatform)Social.Active).SignOut();
    }
    public void ShowLeaderBoard_Rank()
    {
        //Debug.Log("try show LeaderBoard");
        Social.ShowLeaderboardUI();
    }
    public void ReportLeaderBoard(float value)
    {
        Social.ReportScore((int)value,GPGSIds.leaderboard,(bool Success)=> { Debug.Log("ReportProgress"); });
    }
    public void ReportAchievement(int type)
    {
        Debug.Log("ReportAchievement"+type.ToString());
        switch (type)
        {
            case 0:
                Social.ReportProgress(GPGSIds.achievement, 100f, (bool Success) => { Debug.Log("ReportAchievement Success"); });
                break;
            case 1:
                Social.ReportProgress(GPGSIds.achievement_2, 100f, (bool Success) => { Debug.Log("ReportAchievement Success"); });
                break;
            case 2:
                Social.ReportProgress(GPGSIds.achievement_3, 100f, (bool Success) => { Debug.Log("ReportAchievement Success"); });
                break;
            case 3:
                Social.ReportProgress(GPGSIds.achievement_4, 100f, (bool Success) => { Debug.Log("ReportAchievement Success"); });
                break;
            default:
                Debug.Log("Wrong Type");
                break;
        }
    }
}
