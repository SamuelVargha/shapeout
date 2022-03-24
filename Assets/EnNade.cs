using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnNade : MonoBehaviour
{
    public float velocityX;
    public float velocityY;
    public float exploTime;
    public ExploCol exploCol;
    bool active = false;
    Rigidbody2D rb;
    EnNadeGunTrans nadeGun;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        nadeGun = FindObjectOfType<EnNadeGunTrans>();
        gameObject.transform.parent = null;
        velocityX += +Random.Range(0.5f, 2f);
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
        GetComponent<SpriteRenderer>().enabled = false;
        exploCol.Activate();
    }

    void Update()
    {
        
    }

    void DestroyDel()
    {
        Destroy(gameObject);
    }
}
