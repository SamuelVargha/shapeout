using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocityX;
    public float velocityY;
    public GameObject hitParticle;
    bool active = false;
    Rigidbody2D rb;
    GunTrans gun;
    BurstGunTrans burst;
    ShotGunTrans shotGun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gun = FindObjectOfType<GunTrans>();
        burst = FindObjectOfType<BurstGunTrans>();
        shotGun = FindObjectOfType<ShotGunTrans>();
        if(FindObjectOfType<PlayerSquare>().velocity.x > 0)
        {
            velocityX += FindObjectOfType<PlayerSquare>().velocity.x * 0.7f;
        }
        else
        {
            velocityX -= FindObjectOfType<PlayerSquare>().velocity.x * 0.7f;
        }
        if (gun != null && !gun.isFacingRight)
        {
            velocityX *= -1;
        }
        if (burst != null && !burst.isFacingRight)
        {
            velocityX *= -1;
        }
        if (shotGun != null && !shotGun.isFacingRight)
        {
            velocityX *= -1;
        }
        velocityY += Random.Range(-0.1f, 0.4f);
        Invoke("DestroyDel", 2f);
        Invoke("Activate", 0.01f);
    }

    void Activate()
    {
        active = true;
    }

    void Update()
    {
        rb.velocity = new Vector2(velocityX, velocityY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
        {

            if (collision.gameObject.tag.Equals("Enemy"))
            {
                Instantiate(hitParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }

            if (collision.gameObject.tag.Equals("Foreground"))
            {
                Instantiate(hitParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }

    void DestroyDel()
    {
        Destroy(gameObject);
    }
}
