using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MamCtrl : MonoBehaviour
{
    myobject myChar;

    public GameObject Player;
    public PlayerCtrl player;
    public GameObject AttackArea;
    public GameObject Bullet;
    GameObject Area;
    public float RunSpeed;
    public float AttackDelay;
    public float BulletDelay;
    public Vector3 AttackPos;
    bool startAttack = false;
    public GameMgr mgr;
    bool bust = false;

    void Awake()
    {
        myChar = myobject.myChar;
        Player = GameObject.FindGameObjectWithTag("Player");
        transform.position = new Vector3(0, -1, 0);
        UICtrl.Mam = gameObject;
        UICtrl.is_connected = true;
        GameMgr.Mam = gameObject.GetComponent<MamCtrl>();
        UICtrl.Mam_transform = gameObject.GetComponent<Transform>();
        mgr = GameObject.Find("Mgr").GetComponent<GameMgr>();
        player = Player.GetComponentInChildren<PlayerCtrl>();
    }
    //public void Restart()
    //{
    //    StopCoroutine(InvokeAttack());
    //    StopCoroutine("shot");
    //    Destroy(Area);
    //    Destroy(gameObject);
    //}

    void shot()
    {
        GameObject bullet = Instantiate(Bullet);
        bullet.transform.position = AttackPos - new Vector3(0, 0, 50);
        Destroy(bullet, 10);
    }

    void Attack()
    {
        if(myChar.GameSpeed <= 0)
        {
            startAttack = false;
            StopCoroutine(InvokeAttack());
            return;
        }
        AttackPos = new Vector3(Player.transform.position.x, -2.9f, Player.transform.position.z);
        Area = Instantiate(AttackArea);
        Area.transform.position = AttackPos;
        Destroy(Area, (BulletDelay * 3) / myChar.GameSpeed);
        Invoke("shot", BulletDelay / myChar.GameSpeed);
    }

    IEnumerator InvokeAttack()
    {
        while (startAttack)
        {
            Attack();
            yield return new WaitForSeconds(AttackDelay / myChar.GameSpeed);
        }
    }
    void Move()
    {
        transform.Translate(0, 0, RunSpeed * Time.deltaTime * myChar.GameSpeed);
        transform.position = new Vector3(Player.transform.position.x, -1, transform.position.z);
        if (Player.transform.position.z - transform.position.z <= myChar.AttackDistance && !startAttack && myChar.GameSpeed > 0)
        {
            startAttack = true;
            StartCoroutine(InvokeAttack());
            mgr.TurnOnYA();
        }
        if(Player.transform.position.z - transform.position.z > myChar.AttackDistance && startAttack)
        {
            StopCoroutine(InvokeAttack());
            startAttack = false;
        }
        if (Player.transform.position.z - transform.position.z <= 200)
        {
            bust = true;
            AttackDelay = 0.5f;
        }
        else
        {
            bust = false;
            AttackDelay = 3;
        }

    }



    void Update()
    {
        if (bust)
            RunSpeed = player.RunSpeed;
        else
            RunSpeed = 170;
        Move();
    }
}
