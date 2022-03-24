using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    public SpriteRenderer sprite;
    public Transform feetPos;
    public Transform feetPos2;
    public LayerMask ground;
    public LayerMask walls;
    public LayerMask baseLayer;
    public GameObject damageCol;
    public float changeDirTime = 0.6f;
    public float distance = 0.2f;
    public float distanceS = 0.2f;
    public float accel = 3;
    public float maxSpeed = 5;
    public float jumpVel = 4;
    public float lives = 2;
    public float jumpTime = 0;

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
    bool dead = false;
    bool changeDirDis = false;


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
        RaycastHit2D wallInfo = Physics2D.Raycast(feetPos.position, Vector2.right, distanceS, walls);
        RaycastHit2D baseInfo = Physics2D.Raycast(feetPos.position, Vector2.down, distanceS, baseLayer);

        if (groundInfo.collider)
        {
            grounded = true;
        }
        if (safeInfo.collider)
        {
            groundTimer = 0.3f;
            safe = true;
            changeDirEnabled = true;
        }


        if (!safe && wasSafe)
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

        if (transform.position.y + 1 < player.transform.position.y && !jump)
        {
            jump = true;
            float rnd = jumpTime + Random.Range(3.3f, 0.2f);
            Invoke("JumpTimer", rnd);
        }
        else if (transform.position.y - 1 > player.transform.position.y && !jump)
        {
            jump = true;
            float rnd = jumpTime + Random.Range(3.3f, 0.2f);
            Invoke("DeJumpTimer", rnd);
        }
        if (safe)
        {
            if (changeDir && transform.position.x > player.transform.position.x && rb.velocity.x > 2)
            {
                //print("go left");
                changeDir = false;
                float rnd = Random.Range(changeDirTime - 0.5f, changeDirTime + 3f);
                Invoke("ChangeDir", rnd);
            }
            else if (changeDir && transform.position.x < player.transform.position.x && rb.velocity.x < -2)
            {
                //print("go right");
                changeDir = false;
                float rnd = Random.Range(changeDirTime - 0.5f, changeDirTime + 3f);
                Invoke("ChangeDir", rnd);
            }
            velX += accel;
        }
        else
        {
            velX += accel;
        }

        rb.velocity += new Vector2(velX, velY) * Time.deltaTime;
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);

    }

    void Jump()
    {
        RaycastHit2D groundInfo = Physics2D.Raycast(feetPos2.position, Vector2.down, distance, ground);
        if (!groundInfo.collider)
        {
            return;
        }
        rb.velocity += new Vector2(0, jumpVel);
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("ColTimer", 0.2f);
    }

    void DeJump()
    {
        if (based || !grounded)
        {
            return;
        }
        GetComponent<BoxCollider2D>().enabled = false;
        Invoke("ColTimer", 0.2f);
    }

    private void JumpTimer()
    {
        jump = false;
        if (!(transform.position.y + 1 < player.transform.position.y))
        {
            return;
        }
        if (safe)
        {
            Jump();
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
        jump = false;
        if (!(transform.position.y - 1 > player.transform.position.y))
        {
            return;
        }
        if (safe)
        {
            DeJump();
        }
    }

    private void ColTimer()
    {
        CancelInvoke("ColTimer");
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
            Destroy(damageCol.gameObject);
            sprite.enabled = false;
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
