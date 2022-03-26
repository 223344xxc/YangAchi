using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    myobject myChar;
    Slider Bgm, Speed, Effect;
    bool reset = false;
    private void Awake()
    {
        myChar = myobject.myChar;
    }

    void Start()
    {
        Bgm = GameObject.Find("BGMSound").GetComponent<Slider>();
        Effect = GameObject.Find("effectSound").GetComponent<Slider>();
        Speed = GameObject.Find("Vibration").GetComponent<Slider>();
        Bgm.value = myChar.BGMSound;
        Speed.value = myChar.MoveSpeed;
        Effect.value = myChar.EffectSound;
    }

    // Update is called once per frame
    void Update()
    {
        if (!reset)
        {
            if (myChar.BGMSound != Bgm.value)
            {
                myChar.BGMSound = Bgm.value;
                myChar.Set_Volume();
            }
            if (myChar.MoveSpeed != Speed.value)
            {
                myChar.MoveSpeed = Speed.value;
            }
            if (myChar.EffectSound != Effect.value)
            {
                myChar.EffectSound = Effect.value;
                myChar.Set_Volume();
            }
        }
    }
    public void resetPlayerPrefs()
    {
        reset = true;
        //myChar.Reset_PlayerPrefs();
    }
}
