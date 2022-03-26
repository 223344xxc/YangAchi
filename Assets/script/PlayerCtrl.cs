using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Particle
{
    Star,
    Poof,
    Mud,
    Change,
}

public enum Sound
{
    BackGroundMusic,
    BackGroundMusic_City,
    HitNpc,
    HitEnemy,
    HitMud,
    HitTracktor,
    UseSkill,
    ChangeGhost,
    GetShield,
}

public enum TrailParticleType
{
    normal,
    Dash,
    Shield,
    Ghost,
    AllFalse,
}

public class PlayerCtrl : MonoBehaviour
{
    [Header("PlayerScale")]
    public float playerScale = 0;
    public float posYOffset = 0;
    public Vector3 FinalYPos = Vector3.zero;
    public Vector2 ClickPos;
    Vector3 TempScale;

    public GameObject[] Particles;
    public GameObject TunnelParticle;
    public DefaultSheepCtrl sheepCtrl;

    public GameObject[] SheepPrefab;
    //public GameObject particle;
    //public GameObject dashParticle;

    public GameObject[] trailParticle;
    public GameObject[] TutorialUI;


    public Text t_t;
    public Text t_2;
    public GameObject TestImage;

    public float MoveOffset;
    public float ClickTime = 0;
    public bool JumpChack = false;
    float AxisValue;

    public bool isTunnel = false;
    public bool isRun = false;
    public bool useSkill = true;
    public bool cancleSkill = false;
    bool isClick = false;
    bool possibleJump = true;
    bool isJump = false;
    bool nowGrow = false;
    bool nowBig = false;
    float mobScore = 15;

    bool TunelInv = false;
    bool invincibility = false;
    bool shield = false;

    public bool isDodge = false;
    bool ctrl = false;
    int Tutorial = 0;
    int showIndex = 0;

    [Header("PlayerSkill")]
    public float DashCoolTime;
    public float DashSkillCoolTime = 0;
    public float SkillBarState = 0;
    public float MaxSkillBarState = 100;
    public float AlphaScore = 0;

    public myobject myChar;
    public GoogleMgr googleMgr;
    public UICtrl UI;
    public GameMgr mgr;

    [Header("Player Parents")]
    public GameObject parent;

    [Header("PlayerOption")]
    public float JumpPower = 3000;
    public float runSpeed = 150;
    public float rotSpeed = 1;
    public float tempSpeed;
    public SheepType sheepType;
    float KillDashSpeed = 0;


    public static float PlayerRunSpeed;
    public static int Combo = 0;
    public float Enemy_Damage = 15.0f;
    public float MoveLimit;
    float PlayerPosX = 0;
    float PressTime = 0;

    [Header("Camera")]
    public moveSpawn ms;

    public float[] posY;
    public float MoveSpeed;

    int clickStack = 0;
    public int JumpCount = 0;
    public int MaxJumpCount = 1;

    public AudioSource As_BGM, As_Effect,As_BGM_City;

    public AudioClip[] Sounds;
    public AudioClip[] NpcHitSound;

    public Rigidbody rigidbody;
    public MapCtrl mapCtrl;

    Touch touch;

    public float RunSpeed
    {
        get
        {
            return runSpeed;
        }
        set
        {
            runSpeed = value;
            UICtrl.UpdateUI();
        }
    }

    public float enemy_damage
    {
        get {
            return Enemy_Damage;
        }
        set {
            Enemy_Damage = value;
        }
    }


    void Awake()
    {
        myChar = myobject.myChar;
        googleMgr = GoogleMgr.googleMgr;
        InitPlayer();
    }

    void InitPlayer()
    {
        sheepType = myChar.sheepType;
        rigidbody = GetComponent<Rigidbody>();
        sheepCtrl = Instantiate(SheepPrefab[(int)sheepType], transform).GetComponent<DefaultSheepCtrl>();
        PlayerRunSpeed = runSpeed;
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y + posY[myChar.PlayerMeshIDX], transform.position.z);
        posYOffset = gameObject.transform.position.y;
        //particle.SetActive(false);
        SetActiveTrailParticle(TrailParticleType.AllFalse);
        //ResetTutorial();
        //Debug.Log(PlayerPrefs.GetInt("nun"));
        Tutorial = PlayerPrefs.GetInt("Tutorial");
        if (Tutorial == 1)
        {
            Invoke("SetMesh", 2f);
            UI.DrowSign(UICtrl.Sign.Go, 2, 1.9f);
        }
        
    }

    void ResetTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    public void Restart()
    {

        //Destroy(particle);
    }
    public void SetMesh()
    {
        myChar.Enemy_Spawn = true;
        //particle.SetActive(true);
        SetActiveTrailParticle(TrailParticleType.normal);
        isRun = true;
        possibleJump = true;
        sheepCtrl.Change();
        PlayBackGround();
    }

    
    //스킬 함수들
    void Skill()
    {
        if (myChar.GameMode != 1)
            return;
        if (SkillBarState <= 99.99f)
            return;
        switch (sheepType)
        {
            case SheepType.Normal:
                //DashSkill();
                break;
            case SheepType.Muscle:
                Skill_Grow();
                break;
            case SheepType.Jumper:
                Skill_Jumper();
                break;
            case SheepType.Ghost:
                Skill_Ghost();
                break;
        }
        SkillBarState = 0;
        UI.UpdateSkillBar(SkillBarState / MaxSkillBarState);
    }

    IEnumerator GrowBig()
    {
        TempScale = gameObject.transform.localScale;
        float time = 1;
        nowBig = true;
        nowGrow = true;
        invincibility = true;
        DashSkill();
        for (time = 1; time <= 1.5f; time += Time.deltaTime)
        {
            gameObject.transform.localScale = TempScale * time;
            yield return null;
        }
        gameObject.transform.localScale = TempScale * 1.5f;
        nowGrow = false;
    }
    IEnumerator smaller()
    {
        float time = 1.5f;
   
        invincibility = false;
        for (time = 1.5f; time >= 1; time -= Time.deltaTime) {

            gameObject.transform.localScale = TempScale * time;
            yield return null;
        }
        gameObject.transform.localScale = TempScale; 
        nowBig = false;
        nowGrow = false;
    }
    public void Skill_Grow()
    {
        if (nowGrow)
            return;

        if (!nowBig)
        {
            StartCoroutine("GrowBig");
            Instantiate(Particles[(int)Particle.Change]);
            Invoke("Skill_Grow", 7f);
        }
        else
            StartCoroutine("smaller");
    }
    void DashSkill()
    {
        //myChar.invincibility = true;

        tempSpeed = RunSpeed;
        RunSpeed = runSpeed * 2;
        As_Effect.PlayOneShot(Sounds[(int)Sound.UseSkill]);
        //particle.SetActive(false);
        //dashParticle.SetActive(true);
        SetActiveTrailParticle(TrailParticleType.Dash);
        Invoke("ReturnRunSpeed", 7);
    }
    void ReturnRunSpeed()
    {
        //particle.SetActive(true);
        //dashParticle.SetActive(false);
        SetActiveTrailParticle(TrailParticleType.normal);
        useSkill = true;
        myChar.invincibility = false;
        RunSpeed = tempSpeed;
    }
    public void CInvoke()
    {
        CancelInvoke("ReturnRunSpeed");
        useSkill = true;
    }
    public void Give_invincibility(float time)
    {
        invincibility = true;
        Invoke("Remove_invincibility", time);
    }

    public void Give_TunelInv(float time)
    {
        TunelInv = true;
        Invoke("Remove_TunelInv", time);
    }

    public void Remove_TunelInv()
    {
        TunelInv = false;
    }

    public void Remove_invincibility()
    {
        invincibility = false;
    }

    public void Skill_Jumper()
    {
        As_Effect.PlayOneShot(Sounds[(int)Sound.GetShield]);
        StartJumperSkill();
    }
    void StartJumperSkill()
    {
        shield = true;
        SetActiveTrailParticle(TrailParticleType.Shield);
        Invoke("ResetJumperSkill", 7f);
    }
    void ResetJumperSkill()
    {
        SetActiveTrailParticle(TrailParticleType.normal);
        shield = false;
    }


    public void Skill_Ghost()
    {
        As_Effect.PlayOneShot(Sounds[(int)Sound.ChangeGhost]);
        StartGhostSkill();
    }
    void StartGhostSkill()
    {
        SetActiveTrailParticle(TrailParticleType.Ghost);
        Invoke("ResetGhostSkill", 7f);
        Give_invincibility(5);
    }
    void ResetGhostSkill()
    {
        SetActiveTrailParticle(TrailParticleType.normal);
    }
    void SetActiveTrailParticle(TrailParticleType particleType)
    {
        for(int i = 0; i < trailParticle.Length; i++)
        {
            if (i == (int)particleType)
            {
                trailParticle[i].SetActive(true);
                continue;
            }
            trailParticle[i].SetActive(false);
        }
    }
    //스킬 함수들

    void ChangeScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
        transform.position += new Vector3(0, (posYOffset - 1.5f) + (scale * 0.5f) - transform.position.y, 0);
    }

    void PlayerMove()
    {
        if (!isRun)
            return;
        if (!isTunnel)
        {
            if (isJump == false || sheepType == SheepType.Jumper)
            {
                PlayerPosX += AxisValue * Time.deltaTime * myChar.MoveSpeed * myChar.GameSpeed;
                t_t.text = "Axis" + AxisValue + " isjump" + possibleJump + " deltaTime" + Time.deltaTime;
            }
        }

        if (PlayerPosX >= MoveLimit)
            PlayerPosX = MoveLimit;
        else if (PlayerPosX <= MoveLimit * -1)
            PlayerPosX = -1 * MoveLimit;
        parent.transform.position = new Vector3(PlayerPosX, parent.transform.position.y, 
            (parent.transform.position.z + RunSpeed * Time.deltaTime * myChar.GameSpeed) + KillDashSpeed);
        transform.position += FinalYPos;
        FinalYPos = Vector3.zero;
        UI.PlayerDistanceSet(myChar.distance);
    }
    private void Start()
    {
        mgr = GameObject.Find("Mgr").GetComponent<GameMgr>();
        if(sheepType == SheepType.Jumper)
        {
            MaxJumpCount = 2;
        }
    }

    public void PlayBackGround()
    {
        As_BGM.Stop();
        As_BGM.clip = Sounds[(int)Sound.BackGroundMusic];
        As_BGM.loop = true;
        As_BGM.volume = myChar.BGMSound;
        As_BGM.Play();
    }
    public void ChangeBGM()
    {
        As_BGM.Stop();
        As_BGM_City.clip = Sounds[(int)Sound.BackGroundMusic_City];
        As_BGM_City.loop = true;
        As_BGM_City.volume = myChar.BGMSound;
        As_BGM_City.Play();
    }
    void JumpCancle()
    {
        JumpChack = true;
    }

    void Jump()
    {
        JumpChack = false;
        CancelInvoke("JumpCancle");
        Invoke("JumpCancle", 0.3f);
        isJump = true;
        rigidbody.velocity = Vector3.zero;
        rigidbody.velocity = Vector3.up * 50;
    }

    void PlayerUpdate()
    {
        if (RunSpeed > 0)
            RunSpeed -= Time.deltaTime * 0.6f * myChar.GameSpeed;
        else
        {
            RunSpeed = 0;
            As_BGM.Stop();
            As_Effect.Stop();
            mgr.gameover();
        }

        if(KillDashSpeed > 0)
        {
            KillDashSpeed -= Time.deltaTime;
            if(KillDashSpeed < 0)
            {
                KillDashSpeed = 0;
            }
        }

        SheepTypeUpdate();
    }

    //조작
    //void Ctrl()
    //{

    //    if (myChar.GameMode != 1)
    //        return;
    //    if (Input.GetMouseButton(0) && isClick == true)
    //    {
    //        ClickTime += Time.deltaTime;
    //        if(Input.GetAxisRaw("Mouse X") - MoveOffset > 0)
    //        {
    //            AxisValue = Input.GetAxis("Mouse X");
    //            if(Input.GetAxisRaw("Mouse X") - (MoveOffset * 4) > 0)
    //                possibleJump = false;
    //            cancleSkill = true;
    //            clickStack = 0;
    //        }
    //        else if (Input.GetAxisRaw("Mouse X") + MoveOffset < 0)
    //        {
    //            AxisValue = Input.GetAxis("Mouse X");
    //            if (Input.GetAxisRaw("Mouse X") + (MoveOffset * 4) > 0)
    //                possibleJump = false;
    //            cancleSkill = true;
    //            clickStack = 0;
    //        }
    //        else
    //        {
    //            AxisValue = 0;
    //        }
    //        if (possibleJump && (Input.mousePosition.y - ClickPos.y) > 50 && Mathf.Abs(ClickPos.x - Input.mousePosition.x) < MoveOffset * 130)
    //            if (!isJump || JumpCount < MaxJumpCount)
    //            {
    //                possibleJump = false;
    //                JumpCount += 1;
    //                clickStack = 0;
    //                cancleSkill = true;
    //                Jump();
    //            }
    //    }

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        ClickPos = Input.mousePosition;
    //        Debug.Log(ClickPos);
    //        isClick = true;
    //        cancleSkill = false;
    //        clickStack += 1;
    //    }

    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        if(ClickTime <= 0.5f)
    //        {
    //            if (clickStack == 2)
    //                if (!cancleSkill)
    //                    Skill();
    //        }
    //        AxisValue = 0;
    //        ClickTime = 0;
    //        ClickPos = Vector3.zero;
    //        isClick = false;
    //        possibleJump = true;
    //        cancleSkill = false;
    //        Invoke("ResetClickStack", 0.3f);
    //    }
       
    //}
    //조작

    void Ctrl()
    {
        if (myChar.GameMode != 1)
            return;

        if (isTunnel)
        {
            AxisValue = 0;
            return;
        }

        if (Input.touchCount > 0)
        {
            PressTime += Time.deltaTime;
            touch = Input.GetTouch(0);
            if (isClick == false)
            {
                ClickPos = touch.position;
                isClick = true;
            }

            TestImage.SetActive(true);

            //t_2.text = ClickPos.ToString("0f");

          

            if (Mathf.Abs(touch.position.x - ClickPos.x) > MoveOffset * 50)
                ctrl = true;

            if (Mathf.Abs(touch.deltaPosition.x) > MoveOffset)
            {
                AxisValue = touch.deltaPosition.x;
                t_2.text = AxisValue.ToString("0f") + " aaaa ";
            }
            else
                AxisValue = 0;

            if (touch.deltaPosition.x < 20 && touch.deltaPosition.y > 15f && JumpCount < MaxJumpCount && possibleJump && !ctrl)
            {
                JumpCount += 1;
                Jump();
                possibleJump = false;
            }


            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                AxisValue = 0;
            }
        }
        else if (isClick == true)
        {
            ctrl = false;
            possibleJump = true;
            isClick = false;
            TestImage.SetActive(false);
            ClickPos = Vector3.zero;
            if (PressTime <= 0.3f)
            {
                clickStack += 1;
                if (clickStack == 2)
                {
                    Skill();
                    clickStack = 0;
                    PressTime = 0;
                }
            }
            else
            {
                PressTime = 0;
                clickStack = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && (possibleJump || JumpCount < MaxJumpCount))
        {
            JumpCount += 1;
            Jump();
            possibleJump = false;
        }

        ////pc
        //AxisValue = Input.GetAxis("Horizontal") * 10;


        //if (Input.GetKeyDown(KeyCode.G))
        //    Skill();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Skill_Grow();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Skill_Jumper();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Skill_Ghost();
        }
    }

    void ResetClickStack()
    {
        clickStack = 0;
    }

    void TutorialUpdate()
    {
        if (Input.GetMouseButtonDown(0))
            showIndex += 1;

        if (showIndex == 5)
        {
            Invoke("SetMesh", 2f);
            UI.DrowSign(UICtrl.Sign.Go, 2, 1.9f);
            TutorialUI[4].SetActive(false);
            Tutorial = 1;
            PlayerPrefs.SetInt("Tutorial", 1);
            return;
        }

        for (int i = 0; i < TutorialUI.Length; i++) {
            if(i == showIndex)
            {
                TutorialUI[i].SetActive(true);
                continue;
            }
            TutorialUI[i].SetActive(false);
        }
    }

    void Update()
    {
        if (As_BGM.volume != myChar.BGMSound)
        {
            As_BGM.volume = myChar.BGMSound;
        }
        if (!myChar.invincibility)
            TunnelParticle.SetActive(false);
        else
            TunnelParticle.SetActive(true);
        if (Tutorial == 0)
        {
            TutorialUpdate();
            return;
        }

        t_2.text = isJump.ToString();
        if (myChar.GameMode == 1)
        {
            Ctrl();
            PlayerMove();
            PlayerUpdate();
        }
        myChar.distance = Vector3.Distance(transform.position, new Vector3(0, 0, myChar.pos_z));

        if (As_Effect.volume != myChar.EffectSound)
        {
            As_Effect.volume = myChar.EffectSound;
        }

        SheepTypeUpdate();
    }

    //양 타입 별 업데이트
    void SheepTypeUpdate()
    {
        switch (sheepType)
        {
            case SheepType.Normal:
                break;
            case SheepType.Muscle:
                break;
            case SheepType.Jumper:
                DodgiJump();
                break;
            case SheepType.Ghost:
                break;
        }
    }
    void DodgiJump()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.position - new Vector3(0, 1000, 0), out hit, 1000))
        {
            if(hit.transform.CompareTag("enemy") ||
               hit.transform.CompareTag("Mud") ||
               hit.transform.CompareTag("Tracktor"))
            {
                isDodge = true;
            }
        }
    }
    
    //양 타입 별 업데이트

    private void OnCollisionEnter(Collision collision)
    {
        if (!JumpChack)
        {
            return;
        }


        if (isJump/* && collision.transform.CompareTag("GROUND")*/)
        {
            isJump = false;
            //possibleJump = true;
            //particle.SetActive(true);
            JumpCount = 0;
            if (SheepType.Jumper == sheepType)
            {

                if (isDodge)
                {
                    SkillBarState += MaxSkillBarState / 12;
                    UI.UpdateSkillBar(SkillBarState / MaxSkillBarState);
                    isDodge = false;
                    AlphaScore += 10;
                }
                if (shield) {
                    Give_invincibility(1);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("enemy") && !invincibility && !shield&&!myChar.invincibility && !TunelInv)
        {
            switch (sheepType)
            {
                case SheepType.Normal:
                    break;
                case SheepType.Muscle:
                    if (Random.Range(1, 11) <= 2)
                        return;
                    break;
                case SheepType.Jumper:
                    break;
                case SheepType.Ghost:
                    break;
            }
            As_Effect.PlayOneShot(Sounds[(int)Sound.HitEnemy]);
            RunSpeed -= Enemy_Damage;
            GameObject Temp = Instantiate(Particles[(int)Particle.Star]);
            Temp.transform.position = transform.position;
            Destroy(Temp, 3);
            Combo = 0;
            ms.StartShake();
            isDodge = false;
            //ms.startnoise = true;
        }
        else if (other.CompareTag("Tracktor") && !invincibility&&!myChar.invincibility && !TunelInv)
        {
            As_Effect.PlayOneShot(Sounds[(int)Sound.HitTracktor]);
            RunSpeed = 0;
            GameObject Temp = Instantiate(Particles[(int)Particle.Star]);
            Temp.transform.position = transform.position;
            Destroy(Temp, 3);
            Combo = 0;
            ms.StartShake();
            isDodge = false;
            //ms.startnoise = true;
        }
        else if (other.CompareTag("Mud") && !invincibility&&!myChar.invincibility && !TunelInv)
        {
            As_Effect.PlayOneShot(Sounds[(int)Sound.HitMud]);
            RunSpeed = 0;
            GameObject Temp = Instantiate(Particles[(int)Particle.Mud]);
            Temp.transform.position = transform.position;
            Destroy(Temp, 3);
            Combo = 0;
            ms.StartShake();
            isDodge = false;
            //ms.startnoise = true;
        }
        else if (other.CompareTag("NPC"))
        {
            if(sheepType == SheepType.Muscle)
            {
                SkillBarState += MaxSkillBarState / 15f;
                
                UI.UpdateSkillBar(SkillBarState / MaxSkillBarState);

                if (invincibility)
                    AlphaScore += mobScore + (mobScore * 0.2f);
            }
            else if(sheepType == SheepType.Ghost)
            {
                SkillBarState += MaxSkillBarState / 10f;

                UI.UpdateSkillBar(SkillBarState / MaxSkillBarState);

                if (invincibility)
                    AlphaScore += mobScore + (mobScore * 0.2f);
                else
                {
                    if (Random.Range(1, 11) <= 1f)
                    {
                        AlphaScore += mobScore * 1.5f;
                    }
                }
            }
            else
            {
                AlphaScore += mobScore;
            }

            As_Effect.PlayOneShot(NpcHitSound[Random.Range(0, 5)]);
            GameObject Temp = Instantiate(Particles[(int)Particle.Poof]);
            Temp.transform.position = transform.position;
            Destroy(Temp, 3);
            myChar.killcount += 1;
            float random = Random.Range(1, 20);
            myChar.kill_money += random;
            UI.SetPlayerKillCount();
            if (nowBig == false)
            {
                RunSpeed = RunSpeed + 5;

                if (RunSpeed > 150)
                    RunSpeed = 150;
            }

            //if (RunSpeed >= 150 && useSkill) { RunSpeed = 150; }
            //else if (RunSpeed >= 300 && !useSkill) { RunSpeed = 300; }
                
            Combo += 1;
            ms.StartShake();

            //myChar.Change_Money(100);
         
        }
        else if (other.CompareTag("ChangeMap"))
        {
            Debug.Log("Compare");
            mapCtrl.ChangeSky();
            mgr.ChangeCam();
            mgr.ChangeCityEnemy();
            UI.DrowSign(UICtrl.Sign.City, 1);
            ChangeBGM();
            mobScore = 25;
            googleMgr.ReportAchievement(0);
        }
        else if (other.CompareTag("Tunnel"))
        {
            mgr.ChangeCam();
            //mgr.ChangeCityEnemy();
        }
    }

   
}
