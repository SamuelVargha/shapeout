using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Nade : MonoBehaviour
{
    public float velocityX;
    public float velocityY;
    public float exploTime;
    public ExploCol exploCol;
    bool active = false;
    Rigidbody2D rb;
    NadeGunTrans nadeGun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nadeGun = FindObjectOfType<NadeGunTrans>();
        gameObject.transform.parent = null;
        if (FindObjectOfType<PlayerSquare>().velocity.x > 0)
        {
            velocityX += FindObjectOfType<PlayerSquare>().velocity.x;
        }
        else
        {
            velocityX -= FindObjectOfType<PlayerSquare>().velocity.x;
        }
        if (nadeGun != null && !nadeGun.isFacingRight)
        {
            velocityX *= -1;
        }
        velocityY += Random.Range(0.01f, 0.4f);
        rb.velocity = new Vector2(velocityX, velocityY);
        Invoke("DestroyDel", 4f);
        Invoke("Activate", 0.1f);
    }

    void Activate()
    {
        active = true;
        Invoke("Explode", Random.Range(exploTime, exploTime + 0.8f));
    }

    void Explode()
    {
        GetComponentInChildren<ParticleSystem>().Play();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        Camera.main.transform.DOShakePosition(.1f, .4f, 12, 50, false, true);
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        exploCol.Activate();
    }

    void DestroyDel()
    {
        Destroy(gameObject);
    }
}
