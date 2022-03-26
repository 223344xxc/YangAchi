using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCtrl : MonoBehaviour
{
    public Material AttackMt;
    public Mesh AttackMs;

    public bool follow = false;
    public bool use_collider = true;

    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    GameObject Player;
    MeshRenderer renderer;
    Shader shader_diffuse;
    myobject myChar;
    public GameObject collider;
    public Text text;
    void Awake()
    {
        myChar = myobject.myChar;
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        Player = GameObject.FindGameObjectWithTag("Player");
        shader_diffuse = Shader.Find("Legacy Shaders/Transparent/Diffuse");
    }
    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        text.gameObject.SetActive(false);
    }
    void Update()
    {
        if (follow)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, Player.transform.position.z + 5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (use_collider)
        {
            if (other.CompareTag("Player"))
            {
                text.text = PlayerCtrl.Combo + " Combo";
                text.gameObject.SetActive(true);
                Destroy(collider);
                meshFilter.mesh = AttackMs;
                meshRenderer.material = AttackMt;
                follow = true;
                use_collider = false;
                //renderer.material.shader = shader_diffuse;
                StartCoroutine("FadeOut");
            }
        }
    }
    IEnumerator FadeOut()
    {
        int i = 10;
        while (i > 0)
        {
            i -= 1;
            if (i == 10-myChar.Follow_Delay)
            {
                follow = false;
            }
            float f = i / 10.0f;
            Color c = renderer.material.color;
            c.a = f;
            renderer.material.color = c;
            yield return new WaitForSeconds(myChar.Anim_Delay/10);
        }
        Destroy(gameObject);
    }
}
