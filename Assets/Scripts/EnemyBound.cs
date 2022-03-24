using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBound : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null) collision.gameObject.GetComponent<Enemy>().ChangeDir();
    }
}
