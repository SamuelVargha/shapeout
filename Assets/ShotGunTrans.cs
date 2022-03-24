using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunTrans : MonoBehaviour
{
    SpriteRenderer gunSprite;
    Vector2 lastPos;
    public bool isFacingRight = false;
    public Transform shootTrans1;
    public Transform shootTrans2;
    public Transform shootTrans3;
    public Transform shootTrans4;
    public GameObject bullet;
    public Animator gunAnim;
    public float shootTimerVal = 0.1f;
    float shotTimer;
    bool shot = false;

    private void Start()
    {
        gunSprite = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 turn = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Mathf.Abs(turn.x) < 0.8f)
        {
            turn.x = 0;
        }

        Flip(turn.x);
        shotTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Fire1") && !shot)
        {
            shot = true;
        }

        if (shot && shotTimer <= 0)
        {
            gunAnim.Play("Shoot", -1, 0f);
            Instantiate(bullet, shootTrans1.position, shootTrans1.rotation);
            GameObject b2 = Instantiate(bullet, shootTrans2.position, shootTrans2.rotation);
            b2.GetComponent<Bullet>().velocityY += Random.Range(0.2f, 1.5f);
            b2.GetComponent<Bullet>().velocityX -= Random.Range(0.2f, 3f);
            GameObject b3 = Instantiate(bullet, shootTrans3.position, shootTrans3.rotation);
            b3.GetComponent<Bullet>().velocityY -= Random.Range(0.2f, 1.5f);
            b3.GetComponent<Bullet>().velocityX -= Random.Range(0.2f, 3f);

            if (Random.Range(-1f, 0.8f) > 0)
            {
                GameObject b4 = Instantiate(bullet, shootTrans4.position, shootTrans3.rotation);
                b4.GetComponent<Bullet>().velocityY -= Random.Range(0.2f, 5f);
                b4.GetComponent<Bullet>().velocityX -= Random.Range(0.2f, 3f);
            }
            shootTrans1.GetComponent<ParticleSystem>().Play();
            shotTimer = shootTimerVal;
            shot = false;
        }
        Mathf.Clamp(shotTimer, 1f, -1f);
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
            shootTrans1.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (horizontal < -0 && isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            shootTrans1.transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = false;
        }
    }
}
