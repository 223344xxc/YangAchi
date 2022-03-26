using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public enum SheepType
{
    Normal,
    Muscle,
    Jumper,
    Ghost,
}
[System.Serializable]
public class GameData
{
    public float money;
    public int sheepType;
    public int PlayerMeshIDX;
    public bool[] chr_lock;
}
public class myobject : MonoBehaviour
{
    private static myobject s_MyObject = null;
    public static myobject myChar
    {
        get
        {
            if (s_MyObject == null)
            {
                s_MyObject = FindObjectOfType(typeof(myobject)) as myobject;
                if (s_MyObject == null)
                {
                    GameObject obj = new GameObject("MyChar");
                    s_MyObject = obj.AddComponent(typeof(myobject)) as myobject;
                }
            }
            return s_MyObject;
        }
    }
    GoogleMgr googleMgr;
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        //Debug.Break();
        DontDestroyOnLoad(this);
        chr_lock = new bool[4];
        chr_cost = new float[4];
        chr_lock[0] = true;
        chr_cost[0] = 0;
        chr_cost[1] = 500;
        chr_cost[2] = 540;
        chr_cost[3] = 600;
        if (!PlayerPrefs.HasKey("CutScene") || PlayerPrefs.GetInt("CutScene") == 0)
        {
            PlayerPrefs.SetInt("CutScene", 1);
            PlayerPrefs.Save();
            SceneManager.LoadScene("CutScene");
        }
        Get_Volume_from_system();
        Get_Highest_Score();
        googleMgr = GoogleMgr.googleMgr;
        LoadGameData();
        
    }
    [Header("Game")]//변수 정리용 헤더
    public float Highest_Score;
    public int GameMode = 1;//1=진행,2=over,3=stop
    public float GameSpeed = 1;// 게임 속도 배속;
    public bool invincibility = false;
    public float money = 0;
    public float kill_money = 0;
    public float score;

    [Header("Sound")]
    public float BGMSound=0.5f;
    public float EffectSound=0.5f;
    //public float Vibration=0.5f;

    [Header("Player")]
    public float MoveSpeed = 5;
    public float StartSpeed = 150;
    public int PlayerMeshIDX = 0;
    public float distance;
    public int killcount = 0;
    public float pos_z = 0;
    public bool[] chr_lock;
    public float[] chr_cost;
    public SheepType sheepType = 0;

    [Header("Enemy")]
    public bool Enemy_Spawn = false;
    public float Anim_Delay = 0.3f;
    public float Follow_Delay = 2;// =n/10
    public bool is_Enemy_Spawn_Start=false;
    [Header("Mam")]
    public float AttackDistance = 1000;
    //[Header("BackGround")]
    //public float tunnel=100000;
    string key = "ehfmakqmfdiddkclsptakqmfrpdladkzkepal";
    public static string Decrypt(string textToDecrypt, string key)

    {

        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;



        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)

        {

            len = keyBytes.Length;

        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);

        return Encoding.UTF8.GetString(plainText);

    }
    public static string Encrypt(string textToEncrypt, string key)

    {

        RijndaelManaged rijndaelCipher = new RijndaelManaged();

        rijndaelCipher.Mode = CipherMode.CBC;

        rijndaelCipher.Padding = PaddingMode.PKCS7;



        rijndaelCipher.KeySize = 128;

        rijndaelCipher.BlockSize = 128;

        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);

        byte[] keyBytes = new byte[16];

        int len = pwdBytes.Length;

        if (len > keyBytes.Length)

        {

            len = keyBytes.Length;

        }

        Array.Copy(pwdBytes, keyBytes, len);

        rijndaelCipher.Key = keyBytes;

        rijndaelCipher.IV = keyBytes;

        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();

        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);

        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));

    }
    public void SaveGameData()
    {
        GameData gamedata=new GameData();
        gamedata.money = money;
        gamedata.chr_lock = chr_lock;
        gamedata.PlayerMeshIDX = PlayerMeshIDX;
        gamedata.sheepType = (int)sheepType;
        string str = JsonUtility.ToJson(gamedata);
        str = Encrypt(str, key);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "GameData.json"), str);
        Debug.Log("저장완료");
        Debug.Log(str);
    }
    public void LoadGameData()
    {
        string str = File.ReadAllText(Path.Combine(Application.persistentDataPath, "GameData.json"));
        str = Decrypt(str, key);
        GameData gamedata = JsonUtility.FromJson<GameData>(str);
        money = gamedata.money;
        chr_lock = gamedata.chr_lock;
        PlayerMeshIDX = gamedata.PlayerMeshIDX;
        sheepType = (SheepType)gamedata.sheepType;
        Debug.Log(Application.dataPath);
        Debug.Log("로드 완료");
        Debug.Log(str);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
    public void Reset_Player()
    {
        GameMode = 1;
        Enemy_Spawn = false;
        is_Enemy_Spawn_Start = false;
        distance = 0;
        killcount = 0;
        pos_z = 0;
        GameSpeed = 1;
    }
    public void Reset_PlayerPrefs()
    {
        Debug.Log("Reset data");
        PlayerPrefs.SetFloat("Highest_Dist", 0);
        PlayerPrefs.SetInt("CutScene", 0);
        PlayerPrefs.SetFloat("BGMSound", 0.5f);
        PlayerPrefs.SetFloat("EffectSound", 0.5f);
        PlayerPrefs.Save();
        Get_Volume_from_system();
        Get_Highest_Score();
        //googleMgr.ReportLeaderBoard(0);
    }
    public void Set_Highest_Score()
    {
        if (score > Highest_Score)
        {
            PlayerPrefs.SetFloat("Highest_Score", score);
            Get_Highest_Score();
            PlayerPrefs.Save();
            googleMgr.ReportLeaderBoard(Highest_Score);
        }
    }
    public void Get_Highest_Score()
    {
        if (PlayerPrefs.HasKey("Highest_Score"))
        {
            Highest_Score = PlayerPrefs.GetFloat("Highest_Score");
        }
    }
    public void Set_Volume()
    {
        PlayerPrefs.SetFloat("BGMSound", BGMSound);
        PlayerPrefs.SetFloat("EffectSound", EffectSound);
        PlayerPrefs.Save();
    }
    public void Get_Volume_from_system()
    {
        if (PlayerPrefs.HasKey("BGMSound"))
        {
            BGMSound = PlayerPrefs.GetFloat("BGMSound");
        }
        if (PlayerPrefs.HasKey("EffectSound"))
        {
            EffectSound = PlayerPrefs.GetFloat("EffectSound");
        }
    }
}
