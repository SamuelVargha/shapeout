using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCol : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Bullets"))
        {
            if(GetComponentInParent<Enemy>() != null)
            {
                GetComponentInParent<Enemy>().TakeDamage(1);
            }

            if (GetComponentInParent<GunEnemy>() != null)
            {
                GetComponentInParent<GunEnemy>().TakeDamage(1);
            }
            if (GetComponentInParent<MGunEnemy>() != null)
            {
                GetComponentInParent<MGunEnemy>().TakeDamage(1);
            }
            if (GetComponentInParent<PistolEnemy>() != null)
            {
                GetComponentInParent<PistolEnemy>().TakeDamage(1);
            }
            if (GetComponentInParent<NadeEnemy>() != null)
            {
                GetComponentInParent<NadeEnemy>().TakeDamage(1);
            }
            if (GetComponentInParent<ShotGunEnemy>() != null)
            {
                GetComponentInParent<ShotGunEnemy>().TakeDamage(1);
            }
        }
    }
}
