using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnGunTrans : MonoBehaviour
{
    SpriteRenderer gunSprite;
    Vector2 lastPos;
    GunEnemy gunEnemy;
    public Rigidbody2D rb;
    public bool isFacingRight = false;
    public ParticleSystem shootParts;
    public Transform shootTrans;
    public GameObject bullet;
    public Animator gunAnim;
    bool shooting = false;


    private void Start()
    {
        gunEnemy = GetComponentInParent<GunEnemy>();
        gunSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();

        if (!shooting && gunEnemy.aiming)
        {
            shooting = true;
            Invoke("Shoot", Random.Range(0.5f, 2f));
        }
    }

    private void Flip()
    {
        if (rb.velocity.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
            shootTrans.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (rb.velocity.x < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            shootTrans.transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = false;
        }
    }

    private void Shoot()
    {
        gunAnim.Play("EnShoot", -1, 0f);
        GameObject s = Instantiate(bullet, shootTrans.position, shootTrans.rotation);
        shootParts.Play();
        s.transform.parent = gameObject.transform;
        float r = Random.Range(1f, -1f);
        if(r > 0)
        {
            Invoke("Shoot2", 0.12f);
        }
        else
        {
            shooting = false;
        }
        
    }

    private void Shoot2()
    {
        gunAnim.Play("EnShoot", -1, 0f);
        GameObject s = Instantiate(bullet, shootTrans.position, shootTrans.rotation);
        shootParts.Play();
        s.transform.parent = gameObject.transform;
        shooting = false;
    }
}
