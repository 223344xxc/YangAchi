using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TunnelCamCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    myobject myChar;
    public PlayerCtrl Player;
    public CinemachineVirtualCamera vtCam;
    bool prev_invinci=false;
    private void Awake()
    {
        myChar = myobject.myChar;
    }
    public void TunnelCamMgr()
    {
        if (vtCam.Priority == 9)
        {
            vtCam.Priority = 11;
            Player.isTunnel = true;
            prev_invinci = myChar.invincibility;
            myChar.invincibility = true;
            //Invoke("TunnelCamMgr", 2.5f);
        }
        else
        {
            vtCam.Priority = 9;
            Player.isTunnel = false;
            myChar.invincibility = prev_invinci;
            Player.Give_TunelInv(3);
        }
    }
}
