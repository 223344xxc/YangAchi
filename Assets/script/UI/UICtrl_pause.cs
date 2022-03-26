using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl_pause : MonoBehaviour
{
    // Start is called before the first frame update
    myobject myChar;
    public Text Dist, count_k;
    public Sprite[] CharImage;
    public Image thisImage;
    bool is_change = false;
    private void Awake()
    {
        myChar=myobject.myChar;
    }
    private void Update()
    {
        if(this.gameObject.activeSelf&&!is_change)
        {
            Set();
            is_change = true;
        }
    }
    private void start()
    {
        is_change = false;
    }
    public void Set()
    {
        Dist.text = myChar.distance.ToString("f0") + "m";
        count_k.text = myChar.killcount.ToString("f0")+" kill";
        thisImage.sprite = CharImage[(int)myChar.sheepType];
    }
}
