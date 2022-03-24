using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Barrel : MonoBehaviour
{
    public ParticleSystem exploParticle;
    public ExploCol exploCol;
    float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddTorque(Random.Range(-50f, 50f));
        Invoke("Explode", Random.Range(4f, 8f));
    }

    void Explode()
    {
        exploParticle.Play();
        Camera.main.transform.DOShakePosition(.2f, .8f, 18, 90, false, true);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        Invoke("ExploDamage", 0.08f);
    }

    void ExploDamage()
    {
        exploCol.Activate();
        Invoke("SelfDestroy", 0.8f);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
