using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class change_Img : MonoBehaviour
{
    // Start is called before the first frame update
    myobject myChar;
    Image img;
    public Sprite[] arr_spr;
    private void Awake()
    {
        myChar = myobject.myChar;
    }
    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = arr_spr[(int)myChar.sheepType];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
