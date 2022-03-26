using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCtrl : MonoBehaviour
{
    public float InitYPos = 0;
    public float MoveTime = 0.01f;
    myobject myChar;

    void Awake()
    {
        myChar = myobject.myChar;
    }

    void Start()
    {
        
    }

    public void SetGround()
    {
        StartCoroutine(InitGround());
    }

    IEnumerator InitGround()
    {
        while (Mathf.Abs(transform.position.y) - Mathf.Abs(InitYPos) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, InitYPos, transform.position.z), MoveTime * myChar.GameSpeed);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, InitYPos, transform.position.z);
    }
}
