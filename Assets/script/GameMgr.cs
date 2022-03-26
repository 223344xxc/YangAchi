using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMgr : MonoBehaviour
{
    myobject myChar;
    GoogleMgr googleMgr;
    PlayerCtrl Player;
    EnemySpawn enemy_Spawn;
    MapCtrl map_Ctrl;
    public AudioClip PopupSound;
    public float dist_ChangeMap=10000;
    static public MamCtrl Mam;
    public GameObject pause_panel,over_panel,inplay_panel;
    public Transform MamSpawnPos;
    public GameObject MamPrefab;
    public GameObject Mam_Object;
    public GameObject Player_Object;
    public GameObject BackGround_Object;
    public GameObject OptionPanel;
    public string [] enemy_tag;
    UICtrl ui;
    UICtrl_over ui_over;
    bool is_map_change = false;
    public TunnelCamCtrl tCamCtrl;
    bool is_cam_change = false;
    public GameObject mam_ya;
    bool is_50 = false;
    bool is_100 = false;
    bool is_500 = false;
    
    public void InstantiateMam()
    {
        Mam_Object = Instantiate(MamPrefab);
        Mam_Object.transform.position = MamSpawnPos.position;
        Mam = Mam_Object.GetComponent<MamCtrl>();
    }

    private void Awake()
    {
        myChar = myobject.myChar;
        googleMgr = GoogleMgr.googleMgr;
        MamSpawnPos = GameObject.Find("MamSpawnPos").GetComponent<Transform>();
        Invoke("InstantiateMam", 10f);
        ui_over = over_panel.GetComponent<UICtrl_over>();
        Player_Object = GameObject.Find("Player");
        ui = Player_Object.GetComponent<UICtrl>();
        enemy_Spawn = Player_Object.GetComponent<EnemySpawn>();
        map_Ctrl = Player_Object.GetComponent<MapCtrl>();
    }
    private void Start()
    {
        Player = GameObject.Find("Model").GetComponent<PlayerCtrl>();
        myChar.GameSpeed = 1;
        ui.Init_SideWarning();
        myChar.kill_money = 0;
        //Invoke("ChangeCityMap", 3);
        //Invoke("ChangeCityEnemy", 6);
    }
    private void Update()
    {
        if (myChar.GameMode == 1)
        {
            float temp = dist_ChangeMap - map_Ctrl.GroundLength;
            if (is_map_change==false&&temp>0&&myChar.distance > temp)
            {
                is_map_change = true;
                map_Ctrl.ChangeGroundPrefab();
                //Invoke("ChangeCityEnemy", 4);

                ChangeCityEnemy();
            }
            if (!is_50&&myChar.distance >= 50000)
            {
                googleMgr.ReportAchievement(1);
                is_50 = true;
            }
            if (!is_100 && myChar.distance >= 100000)
            {
                googleMgr.ReportAchievement(2);
                is_100 = true;
            }
            if (!is_500 && myChar.distance >= 500000)
            {
                googleMgr.ReportAchievement(3);
                is_500 = true;
            }
        }
    }
    public void TurnOnYA()
    {
        mam_ya.SetActive(true);
        Invoke("TurnOffYA", 3);
    }
    public void TurnOffYA()
    {
        mam_ya.SetActive(false);
    }
    public void ChangeCityEnemy()
    {
        enemy_Spawn.ChangeEnemyPrefab();
    }
    public void ChangeCam()
    {
        tCamCtrl.TunnelCamMgr();
    }

    public void PlayPopupSound()
    {
        Player.As_Effect.PlayOneShot(PopupSound);
    }
    public void gameover()
    {
        myChar.GameMode = 2;
        myChar.GameSpeed = 0;
        float num_score = myChar.distance;
        //if (myChar.killcount != 0)
        //{
        //    num_score *= myChar.killcount;
        //}
        num_score += Player.AlphaScore;
        myChar.score = num_score;
        myChar.Set_Highest_Score();
        myChar.money += myChar.kill_money;
        myChar.kill_money = 0;
        over_panel.SetActive(true);
        inplay_panel.SetActive(false);
        ui.Init_SideWarning();
        myChar.Enemy_Spawn = false;
        myChar.is_Enemy_Spawn_Start = false;
        is_map_change = false;
        myChar.SaveGameData();
    }
    public void gamestop()
    {
        myChar.GameMode = 3;
        myChar.GameSpeed = 0;
        inplay_panel.SetActive(false);
        pause_panel.SetActive(true);
        myChar.Enemy_Spawn = false;
        PlayPopupSound();
    }

    void ResetGame()
    {
        UICtrl.is_connected = false;
        Player.Restart();
        Destroy(Player.gameObject);
        CancelInvoke("InstantiateMam");
        myChar.Reset_Player();
    }
    public void restart()
    {
        ResetGame();
        SceneManager.LoadScene("Loading");
    }

    public void Option()
    {
        PlayPopupSound();
        OptionPanel.SetActive(true);
    }

    public void hone()
    {
        ResetGame();
        SceneManager.LoadScene("Main");
    }
    public void realease_pause()
    {
        pause_panel.SetActive(false);
        inplay_panel.SetActive(true);
        myChar.GameSpeed = 1;
        myChar.GameMode = 1;
        myChar.Enemy_Spawn = true;
    }
}
