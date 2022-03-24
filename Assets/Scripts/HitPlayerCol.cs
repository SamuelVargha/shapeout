using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPlayerCol : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("PlayerDamage"))
        {
            if(transform.position.x > collision.gameObject.transform.position.x)
            {
                collision.gameObject.GetComponentInParent<PlayerSquare>().TakeDamage(damage, true);
            }
            else
            {
                collision.gameObject.GetComponentInParent<PlayerSquare>().TakeDamage(damage, false);
            }
            
        }
    }
}
