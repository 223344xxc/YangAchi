using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Move_Img : MonoBehaviour
{
    // Start is called before the first frame update
    public float changedelay=0.2f;
    public GameObject hp_bar;
    public Sprite sp1, sp2;
    public Image img;
    RectTransform rt,bar_rt;
    float width,per;
    float size_x;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        bar_rt = hp_bar.GetComponent<RectTransform>();
        width = bar_rt.rect.width;
        size_x = bar_rt.rect.size.x;
        //InvokeRepeating("changeSprite", 0, changedelay);
    }

    // Update is called once per frame
    void Update()
    {
        per = hp_bar.GetComponent<Image>().fillAmount;
        if (per >= 0.99f)
        {
            per = 1;
        }
        float a = per * width;
        float b = width / 2;
        //Debug.Log(a-b+768);
        rt.position = new Vector2((per * width) - (width / 2) + 768, rt.position.y);
    }
    void changeSprite()
    {
        if (img.sprite == sp2)
        {
            img.sprite = sp1;
        }
        else
        {
            img.sprite = sp2;
        }
    }
}
