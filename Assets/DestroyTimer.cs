using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    public float destroyTime = 2f;


    void Start()
    {
        Invoke("Destroy", destroyTime);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
