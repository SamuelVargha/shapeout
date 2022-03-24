using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnMBullet : MonoBehaviour
{
    public float velocityX;
    public float velocityY;
    bool active = false;
    public GameObject hitParticle;
    Rigidbody2D rb;
    EnMGunTrans enMGun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.transform.rotation = Quaternion.Euler(0, 0, 0);
        enMGun = GetComponentInParent<EnMGunTrans>();
        gameObject.transform.parent = null;
        if (enMGun != null && !enMGun.isFacingRight)
        {
            velocityX *= -1;
        }
        velocityY += Random.Range(0.01f, 0.4f);
        Invoke("Activate", 0.05f);
    }

    void Activate()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velocityX, velocityY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (active)
        {
            if (collision.gameObject.tag.Equals("PlayerSquare"))
            {
                Instantiate(hitParticle, transform.position, transform.rotation);
                Destroy(gameObject);
            }

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
