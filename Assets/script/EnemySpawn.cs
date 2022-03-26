using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] EnemyPrefab;
    public GameObject[] CityEnemyPrefab;
    public GameObject[] Enemy_Pos;  
    public float delay=1.5f;
    public float pos_y;
    public int random_max = 1000;
    float Gamespeed_temp;
    myobject myChar;
    private void Awake()
    {
        myChar = myobject.myChar;
        //테스트
        //ChangeEnemyPrefab();
    }
    void Start()
    {
        Gamespeed_temp = myChar.GameSpeed;
    }
    //도시로 넘어갈때 장애물 프리팹 바꿔주기
    public void ChangeEnemyPrefab()
    {
        for(int i = 0; i < CityEnemyPrefab.Length; i++)
        {
            EnemyPrefab[i] = CityEnemyPrefab[i];
        }
    }

    void Spawn()
    {
        int pos = Random.Range(0, random_max);
        pos %= Enemy_Pos.Length;
        int kind_of_Enemy = Random.Range(0, random_max);
        kind_of_Enemy %= EnemyPrefab.Length;
        GameObject enemy = Instantiate(EnemyPrefab[kind_of_Enemy]);
        enemy.transform.position = new Vector3(Enemy_Pos[pos].transform.position.x, enemy.transform.position.y, Enemy_Pos[pos].transform.position.z);
    }
    //스폰포인트를 개별 오브젝트로 작성하여 플레이어 x좌표 따라다니지 않도록하기
    void Update()
    {
        if (Gamespeed_temp != myChar.GameSpeed)
        {
            Gamespeed_temp = myChar.GameSpeed;
            CancelInvoke("Spawn");
            InvokeRepeating("Spawn", 1, delay / myChar.GameSpeed); //3초후 부터, SpawnEnemy함수를 1초마다 반복해서 실행 시킵니다.
        }
        if (myChar.GameMode != 1||myChar.Enemy_Spawn==false)
        {
            CancelInvoke("Spawn");
        }
        else if (myChar.GameMode == 1 && myChar.is_Enemy_Spawn_Start == false&&myChar.Enemy_Spawn)
        {
            InvokeRepeating("Spawn", 1, delay / myChar.GameSpeed); //3초후 부터, SpawnEnemy함수를 1초마다 반복해서 실행 시킵니다.
            myChar.is_Enemy_Spawn_Start = true;
        }
    }
}
