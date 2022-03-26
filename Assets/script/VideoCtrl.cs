using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    public VideoPlayer vp;
    void Start()
    {
        vp.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!vp.isPlaying)
        {
            SceneManager.LoadScene("Main");
        }
    }
}
