﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PistolEnemy : MonoBehaviour
{
    public SpriteRenderer sprite;
    public SpriteRenderer gunSprite;
    public Transform feetPos;
    public Transform feetPos2;
    public Transform shootPos;
    public LayerMask ground;
    public LayerMask walls;
    public LayerMask playerLayer;
    public LayerMask baseLayer;
    public GameObject damageCol;
    public float changeDirTime = 0.6f;
    public float distance = 0.2f;
    public float distanceS = 0.2f;
    public float playerDistance = 5f;
    public float accel = 3;
    public float maxSpeed = 5;
    public float jumpVel = 4;
    public float lives = 2;

    Animator animator;
    public Animator animator2;
    Rigidbody2D rb;
    PlayerSquare player;
    float constAccel;
    float groundTimer = 0.3f;
    bool wasSafe;
    bool safe;
    bool grounded;
    bool based;
    bool jump = false;
    bool changeDirEnabled = true;
    bool changeDir = true;
    bool changeDirDis = false;
    bool dead = false;


    // Start is called before the first frame update
    void Start()
    {
        constAccel = accel;
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerSquare>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float velX = 0;
        float velY = 0;
        wasSafe = safe;
        safe = false;
        grounded = false;
        based = false;
        if (dead)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        if (rb.velocity.x >= 0)
        {
            feetPos.transform.localPosition = new Vector3(0.6f, feetPos.transform.localPosition.y, feetPos.transform.localPosition.z);
            feetPos2.transform.localPosition = new Vector3(4.2f, feetPos.transform.localPosition.y, feetPos.transform.localPosition.z);
        }
        else
        {
            feetPos.transform.localPosition = new Vector3(-0.6f, feetPos.transform.localPosition.y, feetPos.transform.localPosition.z);
            feetPos2.transform.localPosition = new Vector3(-4.2f, feetPos.transform.localPosition.y, feetPos.transform.localPosition.z);
        }

        RaycastHit2D safeInfo = Physics2D.Raycast(feetPos.position, Vector2.down, distance, ground);
        RaycastHit2D groundInfo = Physics2D.Raycast(feetPos.position, Vector2.down, distanceS, ground);
        RaycastHit2D wallInfo = Physics2D.Raycast(feetPos.position, Vector2.right, distance, walls);
        RaycastHit2D baseInfo = Physics2D.Raycast(feetPos.position, Vector2.down, distanceS, baseLayer);
        RaycastHit2D playerInfo = Physics2D.Raycast(feetPos.position, Vector2.down, distanceS, baseLayer);

        if (groundInfo.collider)
        {
            grounded = true;
        }

        if (rb.velocity.x > 0)
        {
            playerInfo = Physics2D.Raycast(shootPos.position, Vector2.right, playerDistance, playerLayer);
        }
        else if (rb.velocity.x < 0)
        {
            playerInfo = Physics2D.Raycast(shootPos.position, Vector2.left, playerDistance, playerLayer);
        }


        if (safeInfo.collider)
        {
            groundTimer = 0.3f;
            safe = true;
            changeDirEnabled = true;
        }

        if (wasSafe && !safe)
        {
            ChangeDir();
        }

        if (wallInfo.collider && changeDirEnabled && grounded)
        {
            ChangeDir();
            changeDirEnabled = false;
        }
        if (baseInfo.collider)
        {
            based = true;
        }


        if (groundTimer <= 0)
        {
            changeDirEnabled = false;
            if (transform.position.x > player.transform.position.x)
            {
                accel = -constAccel;
            }
            Invoke("EnableChangeDir", 0.4f);
            groundTimer = 0.3f;
        }

        float rnd = Random.Range(-1f, 1f);
        if((rnd > 0 || based) && !jump)
        {
            jump = true;
            Invoke("JumpTimer", Random.Range(0.3f, 3.2f));
        }
        else if(!jump)
        {
            jump = true;
            Invoke("DeJumpTimer", Random.Range(0.3f, 4f));
        }

            if(rnd > 0.6f && changeDir)
            {
                changeDir = false;
                Invoke("ChangeDir", Random.Range(0.3f,5f));
            }
            velX = accel;

            if (Mathf.Abs(velX) < 10f)
            {
                velX = 0;
            }

            rb.velocity += new Vector2(velX, velY) * Time.deltaTime;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);

    }

    void Jump(float jumpVelF)
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(feetPos2.position, Vector2.down, distanceS, ground);
        if (!groundInfo.collider)
        {
            return;
        }
        rb.velocity += new Vector2(0, jumpVelF);
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("ColTimer", 0.25f);
    }

    void DeJump()
    {
        if (based || !grounded)
        {
            return;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("ColTimer", 0.25f);
    }

    private void JumpTimer()
    {
        CancelInvoke("JumpTimer");
        jump = false;
        if (safe)
        {
            Jump(jumpVel);
        }
    }

    public void EnableChangeDir()
    {
        changeDirEnabled = true;
    }

    public void ChangeDir()
    {
        if (changeDirDis)
        {
            return;
        }
        StartCoroutine(ChangeDirDisTimer());
        CancelInvoke("ChangeDir");
        Invoke("ChangeDirTrue", 0.5f);
        accel *= -1;
    }

    private IEnumerator ChangeDirDisTimer()
    {
        changeDirDis = true;
        yield return new WaitForSeconds(0.2f);
        changeDirDis = false;
    }

    private void ChangeDirTrue()
    {
        changeDir = true;
    }

    private void DeJumpTimer()
    {
        CancelInvoke("DeJumpTimer");
        jump = false;
        if (safe)
        {
            DeJump();
        }
    }

    private void ColTimer()
    {
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void TakeDamage(int damage)
    {
        lives -= damage;
        if (lives <= 0)
        {
            dead = true;
            CancelInvoke();
            Destroy(damageCol);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(GetComponentInChildren<EnPistolTrans>().gameObject);
            sprite.enabled = false;
            gunSprite.enabled = false;
            Destroy(damageCol.gameObject);
            Camera.main.transform.DOShakePosition(.2f, .8f, 18, 90, false, true);
            GetComponentInChildren<ParticleSystem>().Play();
            Invoke("DestroyDelay", 2f);
        }
        else
        {
            animator.Play("Hit");
            animator2.Play("Hit");
        }
    }

    void DestroyDelay()
    {
        Destroy(gameObject);
    }
}