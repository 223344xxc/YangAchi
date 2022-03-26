using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl : MonoBehaviour
{   
    public enum Sign
    {
        Go,
        City,
    }
    myobject myChar;
    public GameObject Player;
    public GameObject PlayerSpeed;
    public GameObject PlayerDistance;
    public GameObject HpBar;
    public GameObject Model;

    public GameObject Sign_City;
    public GameObject Sign_Go;

    static public GameObject Mam;
    static public Image HpBarImage;
    static public Image SkillBarImage;
    static public bool is_connected = false;
    static public Transform Mam_transform;
    public Image side;

    static public PlayerCtrl Player_c;
    public Text PlayerDistance_T;
    public Text PlayerKillCount_T;

    public float distanceFromMam;
    Transform player_tr;


    void InitUI()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        player_tr = Player.GetComponent<Transform>();
        Model = GameObject.Find("Model");
        Player_c = Model.GetComponent<PlayerCtrl>();

        PlayerSpeed = GameObject.Find("PlayerSpeed");

        PlayerDistance = GameObject.Find("PlayerDistance");
        PlayerDistance_T = PlayerDistance.GetComponent<Text>();

        HpBar = GameObject.Find("HpBar");
        SkillBarImage = GameObject.Find("SkillBar").GetComponent<Image>();
        HpBarImage = HpBar.GetComponent<Image>();
        PlayerKillCount_T.gameObject.SetActive(false);
    }

    public IEnumerator drow_Sign(Sign sign, float Time, float Delay)
    {
        GameObject TempSign = null;

        yield return new WaitForSeconds(Delay);

        switch (sign)
        {
            case Sign.Go:
                TempSign = Sign_Go;
                Sign_Go.SetActive(true);
                break;
            case Sign.City:
                TempSign = Sign_City;
                Sign_City.SetActive(true);
                break;
        }

        yield return new WaitForSeconds(Time);

        TempSign.SetActive(false);
    }

    public void DrowSign(Sign sign = Sign.Go, float Time = 1, float Delay = 0)
    {
        StartCoroutine(drow_Sign(sign, Time, Delay));
    }

    static public void UpdateUI()
    {
        HpBarImage.fillAmount = Player_c.RunSpeed / PlayerCtrl.PlayerRunSpeed;
    }

    public void UpdateSkillBar(float Value)
    {
        SkillBarImage.fillAmount = Value;
    }

    void Awake()
    {
        myChar = myobject.myChar;
        InitUI();
    }
    void Update()
    {
        if (is_connected == true)
        {
            distanceFromMam = Vector3.Distance(Mam_transform.position, player_tr.position);
            if (distanceFromMam > myChar.AttackDistance*2)
            {
                Color c = side.color;
                c.a = 0;
                c.b = 255;
                c.r = 231;
                side.color = c;
            }
            else
            {
                Color c = side.color;
                c.a = (myChar.AttackDistance - distanceFromMam/2) / myChar.AttackDistance*2;
                c.b = (myChar.AttackDistance - distanceFromMam/2) / myChar.AttackDistance*2;
                c.r= c.r+3f;
                side.color = c;
            }
        }
    }

    public void Init_SideWarning()
    {
        Color a;
        a.r = a.b = a.g=0;
        a.a = 0;
        side.color=a;
    }

    public void SetPlayerKillCount()
    {
        PlayerKillCount_T.gameObject.SetActive(true);
        PlayerKillCount_T.text = Player_c.myChar.killcount + " Kill !";
        Invoke("KillCountFadeOut", 0.5f);
    }

    public void KillCountFadeOut()
    {
        PlayerKillCount_T.gameObject.SetActive(false);
    }

    public void PlayerDistanceSet(float Distance)
    {
        PlayerDistance_T.text = Distance.ToString("f0") + "m";
    }
}
