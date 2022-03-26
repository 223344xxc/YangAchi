using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class ComboText : MonoBehaviour
{
    Text text;
    Color color;
    public float alpha = 0.5f;
    float posy;
    private void Awake()
    {
        color.r = 1;
        color.g = 1 - PlayerCtrl.Combo * 0.1f; ;
        color.b = 0;
        color.a = 1;
        text = GetComponent<Text>();
        posy = transform.position.y + 7f;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, posy, 0.1f), transform.position.z);
        alpha -= Time.deltaTime * 4;
        color.a = alpha;
        text.color = color;
    }
}
