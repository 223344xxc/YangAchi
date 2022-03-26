using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICtrl_over : MonoBehaviour
{
    // Start is called before the first frame update
    myobject myChar;
    public Text Dist, count_k,score,money;
    bool is_change = false;
    private void Awake()
    {
        myChar = myobject.myChar;
        set_is_change();
    }
    //private void Update()
    //{
    //    if (this.gameObject.activeSelf && !is_change)
    //    {
    //        Set();
    //        is_change = true;
    //    }
    //}

    private void OnEnable()
    {
        Set();
    }
    public void set_is_change()
    {
        is_change = false;
    }
    public void Set()
    {
        Dist.text = myChar.distance.ToString("f0") + "m";
        count_k.text = myChar.killcount.ToString("f0") + " kill";
        
        score.text = myChar.score.ToString("f0")+"점";
        Debug.Log(myChar.killcount * 100);
        money.text = myChar.money.ToString();
    }
}
