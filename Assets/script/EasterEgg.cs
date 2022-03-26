using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEgg : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Developers;
    int count = 0;
    void Start()
    {
        count = 0;        
    }
    private void Update()
    {
        if (count >= 7)
        {
            Developers.SetActive(true);
            count = 0;
        }
    }
    public void countplus()
    {
        count++;
    }
    public void resetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
