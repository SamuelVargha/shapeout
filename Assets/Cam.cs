using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float speed = 2f;

    Vector3 pos;
    bool toMove = true;
    bool moving = false;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != pos && toMove)
        {
            toMove = false;
            Invoke("Move", 0.4f);
        }

        if (moving)
        {
            print("mov");
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
        }

        if(transform.position == pos)
        {
            moving = false;
            toMove = true;
        }
    }

    void Move()
    {
        moving = true;
    }
}
