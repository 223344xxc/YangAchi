using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCtrl : MonoBehaviour
{
    myobject myChar;
    public GameObject Player;
    public GameObject GroundPrefab;
    public GameObject CityGroundPrefab;
    public GameObject TunnelGroundPrefab;
    public GameObject SkyObject;
    public GameObject[] Grounds;
    public GroundCtrl[] Grounds_Ctrl;
    public int MapCount = 0;
    public int GroundCount = 1;
    public int GroundIndex = 0;

    bool Ins = true;
    bool IsTunnel = false;
    int TunnelIndex = -1;

    bool IsCity = false;
    int changeStack = 0;
    public int GroundLength = 100;

    void InitGround()
    {
        Grounds = new GameObject[GroundCount];
        Grounds_Ctrl = new GroundCtrl[GroundCount];
        for (int i = 0; i < GroundCount; i++)
        {
            Grounds[i] = Instantiate(GroundPrefab);
            Grounds[i].transform.position = new Vector3(0, -3, MapCount * GroundLength);
            Grounds_Ctrl[i] = Grounds[i].GetComponent<GroundCtrl>();
            MapCount++;
        }
    }

    //도시 맵으로 프리팹 변경
    public void ChangeGroundPrefab()
    {
        GroundPrefab = CityGroundPrefab;
        IsCity = true;
    }

    public void ChangeSky()
    {
        SkyObject.transform.eulerAngles = new Vector3(90, 0, 0);
    }

    void Awake()
    {
        myChar = myobject.myChar;
        //테스트
        //ChangeGroundPrefab();
        InitGround();
    }
    void Start()
    {
        GameObject Temp = Instantiate(GroundPrefab);
        Temp.transform.position = new Vector3(0, -3, 0);
        Destroy(Temp, 30);
    }

    void Update()
    {
        if (Player.transform.position.z % GroundLength < 10 && Ins && Player.transform.position.z > 300)
        {
            if (IsTunnel == true && TunnelIndex == GroundIndex)
            {
                Destroy(Grounds[GroundIndex]);
                Grounds[GroundIndex] = Instantiate(GroundPrefab);
                Grounds_Ctrl[GroundIndex] = Grounds[GroundIndex].GetComponent<GroundCtrl>();
                IsTunnel = false;
            }
            if (IsCity && changeStack < Grounds.Length)
            {
                Destroy(Grounds[GroundIndex]);

                if (IsTunnel == false)
                {
                    Grounds[GroundIndex] = Instantiate(TunnelGroundPrefab);
                    IsTunnel = true;
                    TunnelIndex = GroundIndex;
                    //myChar.tunnel = Grounds[TunnelIndex].transform.position.z;
                }
                else
                    Grounds[GroundIndex] = Instantiate(GroundPrefab);

                Grounds_Ctrl[GroundIndex] = Grounds[GroundIndex].GetComponent<GroundCtrl>();
                changeStack += 1;
            }
    
            Grounds[GroundIndex].transform.position = new Vector3(0, -9, MapCount * GroundLength);
            Grounds_Ctrl[GroundIndex].SetGround();
            GroundIndex = (GroundIndex + 1) % Grounds.Length;
            MapCount++;
            Ins = false;
        }
        else if (Player.transform.position.z % GroundLength > 90 && !Ins)
            Ins = true;
    }
}
