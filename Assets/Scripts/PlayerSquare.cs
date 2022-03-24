using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Controller2D))]
public class PlayerSquare : MonoBehaviour
{
    Animator animator;
    public Animator animator2;
    Stats stats;
    private SpriteRenderer sprite;
    public SpriteRenderer sprite2;
    public ParticleSystem jumpParticle;
    public ParticleSystem deathParticle;
    public float moveSpeed = 6f;
    public float fallSpeed = 6f;
    public float immuneTime = 1f;
    public Vector2 damageJump;
    public GameObject damageCol;
    public Transform feetPos;
    public LayerMask baseLayer;

    public int lives = 3;
    public bool dead = false;
    public bool doubleJump;
    public bool grounded;
    public bool deJump = false;
    bool doubleJumpEn = false;
    bool wGrounded;
    public bool isJumping;
    bool slowmo = false;
    bool dirBool = false;
    bool based;
    bool immune;

    private float jumpCounter;
    public float jumpTime;
    int jumpDir;

    public float gravity = -15;
    public float accelerationTimeAir = 0.3f;
    public float accelerationTimeGround = 0.1f;
    float constAccelTimeGround;
    public float wallBoost = 0.2f;
    public float groundedTimerValue = 0.2f;
    float groundedTimer = 0f;
    public float jumpedTimerValue = 0.2f;
    float jumpedTimer = 0f;
    public float doubleJumpTimerValue = 1f;
    float doubleJumpTimer = 0f;

    public float jumpVelocity = 10;
    float constJumpVelocity;
    public Vector3 velocity;
    Vector3 oldVel;
    float velocityXSmoothing;

    Controller2D controller;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        dead = false;
        stats = GetComponent<Stats>();
        constAccelTimeGround = accelerationTimeGround;
        sprite = GetComponent<SpriteRenderer>();
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        constJumpVelocity = jumpVelocity;
    }

    private void Update()
    {
        if (dead)
        {
            return;
        }
        //debug
        if (Input.GetButtonDown("SlowMoDebug"))
        {
            if (slowmo)
            {
                Time.timeScale = 1;
                slowmo = false;
            }
            else
            {
                Time.timeScale = 0.2f;
                slowmo = true;
            }
        }
        //debug


        oldVel = velocity;
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        int wallDirX = (controller.collisions.left) ? -1 : 1;
        bool jump = false;
        wGrounded = grounded;
        grounded = false;

        if (Mathf.Abs(input.x) < 0.8f)
        {
            input.x = 0;
        }

        groundedTimer -= Time.deltaTime;
        if (controller.collisions.below)
        {
            groundedTimer = groundedTimerValue;
            grounded = true;
            doubleJumpTimer = 0;
            doubleJumpEn = true;
        }

        jumpedTimer -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            jumpedTimer = jumpedTimerValue;
        }
        if(Input.GetButtonDown("DeJump"))
        {
            RaycastHit2D baseInfo = Physics2D.Raycast(feetPos.position, Vector2.down, 0.3f, baseLayer);
            if (!baseInfo.collider && grounded)
            {
                deJump = true;
                StartCoroutine(DeJumpDisable());
            }
        }

        if ((controller.collisions.left || controller.collisions.right) && !grounded)
        {
            if ((controller.collisions.left && input.x > 0) || (controller.collisions.right && input.x < 0))
            {
                input.x = 0;
            }
        }

        //Jump
        if (jumpedTimer > 0 && groundedTimer > 0)
        {
            jumpedTimer = 0;
            if (input.x > 0)
            {
                jumpDir = 6;
            }

            if (input.x < 0)
            {
                jumpDir = 6;
            }
            jump = true;
            groundedTimer = 0;
            velocity.y = jumpVelocity + 5;
            if (input.x > 0)
            {
                velocity.x = 6;
            }
            if (input.x < 0)
            {
                velocity.x = -6;
            }
            jumpCounter = jumpTime;
            isJumping = true;
            //jumpParticle.Play();
            if (doubleJump)
            {
                doubleJumpTimer = doubleJumpTimerValue;
            }
        }

        //Double Jump
        if (jumpedTimer > 0 && doubleJumpTimer > 0 && doubleJumpEn && !grounded)
        {
            jumpedTimer = 0;
            jumpDir = 6;
            jump = true;
            groundedTimer = 0;
            velocity.y = jumpVelocity + 5;
            jumpCounter = jumpTime - 0.3f;
            isJumping = true;
            //jumpParticle.Play();
            doubleJumpEn = false;
        }

        //Aircontrolls
        if (Input.GetButton("Jump"))
        {

            if (isJumping)
            {
                if (jumpCounter > 0)
                {
                    velocity.y = velocity.y + jumpVelocity / 5;
                    if(jumpVelocity > 0.5f)
                    {
                        jumpVelocity -= 0.5f;
                    }
                    jumpCounter -= Time.deltaTime;
                }
                else
                {
                    jumpVelocity = constJumpVelocity;
                    isJumping = false;
                }
            }
        }
        else
        {
            isJumping = false;
            jumpVelocity = constJumpVelocity;
            if (jumpCounter > 0)
            {
                    velocity.y = velocity.y - jumpVelocity;

                jumpCounter = 0;
            }
        }

        if (!grounded)
        {
            doubleJumpTimer -= Time.deltaTime;
            if ((velocity.x > -6 && input.x < -0.2) || (velocity.x < 6 && input.x > 0.2))
            {
                jumpDir = 0;
            }
            else
            {
                switch (jumpDir)
                {
                    case 1:
                        velocity.x = 8;
                        break;
                    case 2:
                        velocity.x = -8;
                        break;
                    case 4:
                        velocity.x = 10;
                        break;
                    case 5:
                        velocity.x = -10;
                        break;
                    case 6:
                        if(velocity.x > 0 && dirBool)
                        {
                            velocity.x += 1;
                            dirBool = false;
                        }
                        else if(dirBool)
                        {
                            velocity.x -= 1;
                            dirBool = false;
                        }
                        break;
                }
            }
        }
        else if (!isJumping)
        {
            jumpDir = 0;
        }

        float targetVelocityX;
        if (input.x == 0)
        {
            targetVelocityX = 0;
        }
        else
        {
            targetVelocityX = Mathf.Sign(input.x) * moveSpeed;
        }

        if (velocity.y >= 0)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        else if (velocity.y < 0)
        {
            velocity.y += (gravity - 20) * Time.deltaTime;
        }
        if (Mathf.Abs(input.x) <= 0 && !isJumping && Mathf.Abs(velocity.x) < 4)
        {
            jumpDir = 0;
            velocity.x = 0;
        }
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGround : accelerationTimeAir);
        velocity.y = Mathf.Clamp(velocity.y, -fallSpeed, fallSpeed + 5);
        controller.Move(velocity * Time.deltaTime, input);

        if (controller.collisions.above)
        {
            if(isJumping && velocity.y > 0.5f && !grounded)
            {
                //nothing
            }
            else
            {
                velocity.y = 0;
            }
            
        }
        if (controller.collisions.below)
        {
            velocity.y = 0;
        }
    }

    public void Disable()
    {
        deathParticle.Play();
        dead = true;
        sprite2.enabled = false;
        foreach(SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            s.enabled = false;
        }
        foreach (BoxCollider2D b in GetComponents<BoxCollider2D>())
        {
            b.enabled = false;
        }
        foreach (BoxCollider2D b in damageCol.GetComponents<BoxCollider2D>())
        {
            b.enabled = false;
        }
        foreach (CapsuleCollider2D c in damageCol.GetComponents<CapsuleCollider2D>())
        {
            c.enabled = false;
        }
    }

    public void TakeDamage(int damage, bool right)
    {
        if (immune)
        {
            return;
        }
        lives -= damage;
        if(lives <= 0)
        {
            Disable();
        }
        else
        {
            animator.Play("Hit");
            animator2.Play("Hit");
            if (right)
            {
                velocity.x = -14;
                velocity.y = 7;
            }
            else
            {
                velocity.x = 14;
                velocity.y = 7;
            }
        }
        immune = true;
        Invoke("DisableImmune", immuneTime);
    }

    void DisableImmune()
    {
        immune = false;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void DamageJump()
    {
        velocity.x += damageJump.x;
        velocity.y += damageJump.y;
        controller.Move(velocity * Time.deltaTime, false);
    }

    private IEnumerator AccelChange()
    {
        accelerationTimeGround *= 1.5f;
        yield return new WaitForSeconds(0.3f);
        accelerationTimeGround = constAccelTimeGround;
    }

    private IEnumerator DeJumpDisable ()
    {
        yield return new WaitForSeconds(0.15f);
        deJump = false;
    }
}
