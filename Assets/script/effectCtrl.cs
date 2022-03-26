using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class effectCtrl : MonoBehaviour
{
    public Vector3 offsetVector;
    public GameObject Player;
    void Awake()
    {
        if (Player == null)
            Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        transform.position = Player.transform.position - offsetVector;
    }
}
