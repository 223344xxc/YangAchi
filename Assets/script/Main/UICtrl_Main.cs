using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UICtrl_Main : MonoBehaviour
{
    // Start is called before the first frame update
    myobject myChar;
    GoogleMgr googleMgr;
    public Text BestScore;
    public Text Money;
    public AudioSource AS, AS_Effect;
    public AudioClip PopupSound;
    private void Awake()
    {
        myChar = myobject.myChar;
        googleMgr = GoogleMgr.googleMgr;
    }
    void Start()
    {
        BestScore = GameObject.Find("BestScoreText").GetComponent<Text>();
        Money = GameObject.Find("MoneyText").GetComponent<Text>();
        BestScore.text = myChar.Highest_Score.ToString("f0");
        Money.text = myChar.money.ToString("f0");
        AS.Play();
    }
    void Update()
    {
        AS.volume = myChar.BGMSound;
        AS_Effect.volume = myChar.EffectSound;

    }
    public void RankButton()
    {
        //Debug.Log("From RankButton");
        googleMgr.ShowLeaderBoard_Rank();
    }

    public void Option()
    {
        AS_Effect.PlayOneShot(PopupSound);
        SceneManager.LoadScene("Option");
    }
}
