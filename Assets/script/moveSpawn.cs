using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSpawn : MonoBehaviour
{
    public GameObject player;
    public PlayerCtrl p_c;
    public float pos_y,pos_z;

    public bool shakeRotate = false;
    private Vector3 originPos;
    private Quaternion originRot;
    public bool startnoise = false;


    void Awake()
    {
        transform.position = new Vector3(0, pos_y, player.transform.position.z + pos_z);

    }

    void Start()
    {
        if (gameObject.CompareTag("MainCam"))
            p_c.ms = gameObject.GetComponent<moveSpawn>();
    }

    void Update()
    {
        transform.position = new Vector3(0, pos_y, player.transform.position.z+pos_z);
        if (startnoise)
        {
            startnoise = false;
            StartShake();
        }
    }
   
    public void StartShake()
    {
        originPos = transform.position;
        originRot = transform.localRotation;
        StartCoroutine(ShakeCamera(0.1f, 3f, 1f));
    }

    public IEnumerator ShakeCamera(float duration = 0.05f, float magnitudePos = 0.03f, float magnitudeRot = 0.01f)
    {
        float passTime = 0.0f;

        while (passTime < duration)
        {
            Vector3 shakePos = transform.position +Random.insideUnitSphere;

            transform.localPosition = new Vector3(shakePos.x * magnitudePos, shakePos.y /** magnitudePos*/, shakePos.z /** magnitudePos*/);
            //Debug.Log(shakePos * magnitudePos);
            if (shakeRotate)
            {
                Vector3 shakeRot = new Vector3(0, 0, Mathf.PerlinNoise(Time.time * magnitudeRot, 0.0f));
                transform.localRotation = Quaternion.Euler(shakeRot);
            }

            passTime += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = new Vector3(transform.position.x, pos_y, player.transform.position.z + pos_z);
        transform.localRotation = originRot;
    }

}
