using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnShotGunTrans : MonoBehaviour
{
    SpriteRenderer gunSprite;
    Vector2 lastPos;
    ShotGunEnemy gunEnemy;
    public Rigidbody2D rb;
    public bool isFacingRight = false;
    public Transform shootTrans1;
    public Transform shootTrans2;
    public Transform shootTrans3;
    public Transform shootTrans4;
    public GameObject bullet;
    public Animator gunAnim;
    bool shooting = false;


    private void Start()
    {
        gunEnemy = GetComponentInParent<ShotGunEnemy>();
        gunSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();

        if (!shooting && gunEnemy.aiming)
        {
            shooting = true;
            Invoke("Shoot", Random.Range(0.1f, 2.2f));
        }
    }

    private void Flip()
    {
        if (rb.velocity.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (rb.velocity.x < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = false;
        }
    }

    private void Shoot()
    {
        gunAnim.Play("EnShoot", -1, 0f);
        GameObject b1 = Instantiate(bullet, shootTrans1.position, shootTrans1.rotation);
        b1.transform.parent = gameObject.transform;
        GameObject b2 = Instantiate(bullet, shootTrans2.position, shootTrans2.rotation);
        b2.transform.parent = gameObject.transform;
        b2.GetComponent<EnBullet>().velocityY += Random.Range(0.2f, 1.5f);
        b2.GetComponent<EnBullet>().velocityX -= Random.Range(0.2f, 3f);
        GameObject b3 = Instantiate(bullet, shootTrans3.position, shootTrans3.rotation);
        b3.transform.parent = gameObject.transform;
        b3.GetComponent<EnBullet>().velocityY -= Random.Range(0.2f, 1.5f);
        b3.GetComponent<EnBullet>().velocityX -= Random.Range(0.2f, 3f);

        if (Random.Range(-1f, 0.8f) > 0)
        {
            GameObject b4 = Instantiate(bullet, shootTrans4.position, shootTrans3.rotation);
            b4.transform.parent = gameObject.transform;
            b4.GetComponent<EnBullet>().velocityY -= Random.Range(0.2f, 5f);
            b4.GetComponent<EnBullet>().velocityX -= Random.Range(0.2f, 3f);
        }
        float r = Random.Range(1f, -1f);
        shooting = false;
    }
}
