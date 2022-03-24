using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NadeGunTrans : MonoBehaviour
{
    SpriteRenderer gunSprite;
    Vector2 lastPos;
    public bool isFacingRight = false;
    public Transform shootTrans;
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
            Shoot();
        }
        Mathf.Clamp(shotTimer, 1f, -1f);
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(-1, 1, 1);
            shootTrans.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (horizontal < -0 && isFacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
            shootTrans.transform.rotation = Quaternion.Euler(0, 0, 0);
            isFacingRight = false;
        }
    }

    private void Shoot()
    {
        gunAnim.Play("Shoot", -1, 0f);
        Instantiate(bullet, shootTrans.position, shootTrans.rotation);
        shootTrans.GetComponent<ParticleSystem>().Play();
        shotTimer = shootTimerVal;
        shot = false;
    }
}
