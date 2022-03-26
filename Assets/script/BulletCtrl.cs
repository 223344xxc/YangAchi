using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    public float speed;
    public float rotSpeed;
    myobject myChar;
    Vector3 stackPos = Vector3.zero;
    public PlayerCtrl player;


    private void Awake()
    {
        myChar = myobject.myChar;
        player = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerCtrl>();
    }

    void Update()
    {
        transform.position += new Vector3(0, 0, (speed * Time.deltaTime * myChar.GameSpeed) + (player.RunSpeed * Time.deltaTime * myChar.GameSpeed));
        transform.Rotate(0, Time.deltaTime * rotSpeed * myChar.GameSpeed, 0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
