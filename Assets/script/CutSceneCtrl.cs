using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneCtrl : MonoBehaviour
{
    public GameObject[] CutScene;
    public int CutSceneIndex = 0;


    void SetActiveCutScene(int SceneIndex)
    {
        if (SceneIndex >= CutScene.Length)
            return;
        for(int i = 0; i < CutScene.Length; i++)
        {
            if(i == SceneIndex)
            {
                CutScene[i].SetActive(true);
                continue;
            }
            CutScene[i].SetActive(false);
        }
    }


    private void Start()
    {
        SetActiveCutScene(CutSceneIndex);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CutSceneIndex += 1;
            SetActiveCutScene(CutSceneIndex);
        }
        if(CutSceneIndex >= CutScene.Length)
        {
            //씬 로딩 로직
            SceneManager.LoadScene("Loading");
        }
    }
}
