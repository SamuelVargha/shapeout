using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnPistolBullet : MonoBehaviour
{
    bool active = false;
    public GameObject hitParticle;
    public float timesVelocity = 1.5f;
    Rigidbody2D rb;
    Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.parent = null;
        GetComponent<Rigidbody2D>().velocity = (FindObjectOfType<PlayerSquare>().transform.position - transform.position).normalized * timesVelocity;
        velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y);
        Invoke("Activate", 0.08f);
    }

    void Activate()
    {
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y);
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
