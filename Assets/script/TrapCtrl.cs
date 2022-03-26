using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCtrl : MonoBehaviour
{
    public float MoveLimit = 0;
    public float MoveSpeed = 1;
    bool MoveSide = true;


    void Start()
    {
        MoveSpeed = Random.Range(MoveSpeed, MoveSpeed + 10);
        if (transform.position.x > MoveLimit)
            MoveSide = false;
        else
            MoveSide = true;
    }

    void Update()
    {

        transform.Translate(Time.deltaTime * -MoveSpeed * myobject.myChar.GameSpeed, 0, 0);
        if(transform.position.x > MoveLimit && MoveSide)
        {
            MoveSide = false;
            MoveSpeed = MoveSpeed * -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, 
                transform.localScale.y, transform.localScale.z);
        }
        else if (transform.position.x < -MoveLimit && !MoveSide)
        {
            MoveSide = true;
            MoveSpeed = MoveSpeed * -1;
            transform.localScale = new Vector3(transform.localScale.x * -1,
                transform.localScale.y, transform.localScale.z);
        }
    }
}
