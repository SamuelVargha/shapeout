using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnPistolTrans : MonoBehaviour
{
    SpriteRenderer gunSprite;
    Vector2 lastPos;
    PlayerSquare player;
    public float offset = 0;
    public Rigidbody2D rb;
    public bool isFacingRight = false;
    public Transform shootTrans;
    public GameObject bullet;
    public Animator gunAnim;
    bool shooting = false;


    private void Start()
    {
        gunSprite = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<PlayerSquare>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = player.transform.position;
        Vector3 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        if((angle < -90 && !isFacingRight) || (angle > 90 && !isFacingRight))
        {
            //print("flip");
            //Flip();
        }
        if (angle > -90 && angle < 90 && !isFacingRight)
        {
            Flip();
        }
        if ((angle < -90 && isFacingRight) || (angle > 90 && isFacingRight))
        {
            Flip();
        }
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));


        if (!shooting)
        {
            shooting = true;
            Invoke("Shoot", Random.Range(0.4f, 3f));
        }
    }

    private void Flip()
    {
        if (transform.localScale.y == 1)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
            isFacingRight = false;
        }
    }

    private void Shoot()
    {
        gunAnim.Play("Shoot", -1, 0f);
        GameObject s = Instantiate(bullet, shootTrans.position, shootTrans.rotation);
        s.transform.parent = gameObject.transform;
        shooting = false;

    }
}
