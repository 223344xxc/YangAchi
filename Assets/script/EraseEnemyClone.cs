using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraseEnemyClone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="eraser")
            Destroy(this.gameObject);
    }
}
