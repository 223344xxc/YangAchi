using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopCtrl : MonoBehaviour
{
    myobject myChar;
    public GameObject[] CharacterArr;
    public GameObject[] CharacterScrollArr;
    public int CharIndex = 0;
    public int WorldIndex = 0;

    public Vector3 MovePos = Vector3.zero;
    public Vector3 Vel = Vector3.zero;
    public Vector3[] cVel;
    public Vector3 CharMovePos = new Vector3(0, 15, 0);
    public Text Money_T;
    public GameObject Lock;
    public SheepType SelectSheep = SheepType.Normal;


    public float MoveDis;

    public Text cost;


    public void InitShop()
    {
        update_Scroll();
    }

    public void chr_buy()
    {
        Debug.Log(myChar.money);
        if (myChar.money < 0)
        {
            myChar.money = 0;
        }
        if (myChar.chr_lock[CharIndex] == true)
        {
            return;
        }
        if (myChar.money < myChar.chr_cost[CharIndex])
        {
            return;
        }
        myChar.money -= myChar.chr_cost[CharIndex];
        Debug.Log(myChar.chr_cost[CharIndex]);
        myChar.chr_lock[CharIndex] = true;
        update_Money_Text();
        update_lock();
        update_Select();
        Debug.Log(myChar.money);
        myChar.sheepType = (SheepType)CharIndex;
        myChar.SaveGameData();
    }

    public void StartGame()
    {
        if (!myChar.chr_lock[CharIndex])
        {
            return;
        }
        myChar.sheepType = (SheepType)CharIndex;
        SceneManager.LoadScene("Loading");
    }

    public void update_Select()
    {
        SelectSheep = (SheepType)CharIndex;
    }
    public void ChangeRight()
    {
        MovePos += new Vector3(MoveDis, 0, 0);
        WorldIndex += 1;

        if (++CharIndex > CharacterArr.Length - 1) { CharIndex = 0; }
        CharacterArr[Mathf.Abs(CharIndex + 1) % CharacterArr.Length].transform.position = new Vector3((WorldIndex + 1) * 2.5f, 0, 0);
        if (myChar.chr_cost[CharIndex] == 0)
        {
            cost.text = "Free";
        }
        else
        {
            cost.text = myChar.chr_cost[CharIndex].ToString("f0");
        }
        update_lock();
        update_Select();
        update_Scroll();
    }
    public void ChangeLeft()
    {
        MovePos -= new Vector3(MoveDis, 0, 0);
        WorldIndex -= 1;
        CharacterArr[Mathf.Abs(CharIndex + 2) % 4].transform.position = new Vector3((WorldIndex - 1) * 2.5f, 0, 0);
        if (--CharIndex < 0)
        {
            CharIndex = 3;
        }
        CharIndex = Mathf.Abs(CharIndex);
        if (myChar.chr_cost[CharIndex] == 0)
        {
            cost.text = "Free";
        }
        else
        {
            cost.text = myChar.chr_cost[CharIndex].ToString("f0");
        }
        update_lock();
        update_Select();
        update_Scroll();
    }

    void Awake()
    {
        myChar = myobject.myChar;
        InitShop();
        MovePos = transform.position;
        cVel = new Vector3[CharacterArr.Length];
        for(int i = 0; i< cVel.Length; i++)
        {
            cVel[i] = Vector3.zero;
        }
        CharacterArr[CharacterArr.Length - 1].transform.position = new Vector3((WorldIndex - 1) * 2.5f, 0, 0);
        Money_T = GameObject.Find("Money").GetComponent<Text>();
        if (myChar.chr_cost[CharIndex] == 0)
        {
            cost.text = "Free";
        }
        else
        {
            cost.text = myChar.chr_cost[CharIndex].ToString("f0");
        }
        update_Money_Text();
        update_lock();
        update_Select();
    }

    void update_Scroll()
    {
        for(int i = 0; i < CharacterScrollArr.Length; i++)
        {
            if(CharIndex == i)
            {
                CharacterScrollArr[i].SetActive(true);
                continue;
            }
            CharacterScrollArr[i].SetActive(false);
        }
    }

    void update_lock()
    {
        Lock.SetActive(!myChar.chr_lock[CharIndex]);
    }
    void update_Money_Text()
    {
        Money_T.text = myChar.money.ToString("f0");
    }
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, MovePos, ref Vel, 0.1f, 100);
        CheckNowSelect();
    }

    void CheckNowSelect()
    {
        for(int i = 0; i < CharacterArr.Length; i++)
        {
            if (Mathf.Abs(CharIndex) == i) {
                CharacterArr[i].transform.position = Vector3.SmoothDamp(CharacterArr[i].transform.position,
                    CharMovePos + new Vector3(CharacterArr[i].transform.position.x, 0, 4f),ref cVel[i], 0.1f, 1000);
            }
            else
            {
                CharacterArr[i].transform.position = Vector3.SmoothDamp(CharacterArr[i].transform.position,
                    CharMovePos + new Vector3(CharacterArr[i].transform.position.x, 0, 7), ref cVel[i], 0.1f, 1000);
            }
        }
    }
    
}
