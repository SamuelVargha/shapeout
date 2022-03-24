using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnMGunTrans : MonoBehaviour
{
    SpriteRenderer gunSprite;
    Vector2 lastPos;
    MGunEnemy mGunEnemy;
    public Rigidbody2D rb;
    public bool isFacingRight = false;
    public Transform shootTrans;
    public GameObject bullet;
    public Animator gunAnim;
    int shot = 0;
    bool shooting = false;


    private void Start()
    {
        mGunEnemy = GetComponentInParent<MGunEnemy>();
        gunSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();

        if (!shooting && mGunEnemy.aiming)
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
        }
        else if (rb.velocity.x < 0 && isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = false;
        }
    }

    private void Shoot()
    {
        shot = 0;
        gunAnim.Play("EnMShoot", -1, 0f);
        GameObject s = Instantiate(bullet, shootTrans.position, shootTrans.rotation);
        s.transform.parent = gameObject.transform;
        float r = Random.Range(1f, -0.1f);
        if (r > 0)
        {
            Invoke("Shoot2", 0.08f);
        }
        else
        {
            shooting = false;
        }

    }

    private void Shoot2()
    {
        shot++;
        if(shot > Random.Range(5, 7))
        {
            shooting = false;
            return;
        }
        gunAnim.Play("EnMShoot", -1, 0f);
        GameObject s = Instantiate(bullet, shootTrans.position, shootTrans.rotation);
        s.transform.parent = gameObject.transform;
        float r = Random.Range(1f, -0.1f);
        Invoke("Shoot2", 0.08f);
    }
}
