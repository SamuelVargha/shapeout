using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExploCol : MonoBehaviour
{
    CircleCollider2D circleCol;
    List<Enemy> enemList = new List<Enemy>();
    List<GunEnemy> gunEnemList = new List<GunEnemy>();
    List<MGunEnemy> mgunEnemList = new List<MGunEnemy>();
    List<NadeEnemy> nadeEnemList = new List<NadeEnemy>();
    public int enemDamageInt = 5;
    public int playerDamageInt = 2;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    bool playerDamage = false;
    bool enemDamage = false;
    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        circleCol = GetComponent<CircleCollider2D>();
        circleCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (circleCol.IsTouchingLayers(playerLayer) && !playerDamage)
            {
                if (gameObject.transform.position.x > FindObjectOfType<PlayerSquare>().transform.position.x)
                {
                    FindObjectOfType<PlayerSquare>().TakeDamage(playerDamageInt, true);
                }
                else
                {
                    FindObjectOfType<PlayerSquare>().TakeDamage(playerDamageInt, false);
                }
                playerDamage = true;
            }

            foreach (Enemy e in FindObjectsOfType<Enemy>())
            {
                try
                {
                    if (e != null && !enemList.Contains(e) && circleCol.IsTouching(e.damageCol.GetComponent<BoxCollider2D>()))
                    {
                        e.TakeDamage(enemDamageInt);
                        enemList.Add(e);

                    }
                }
                catch
                {
                    
                }
                
            }

            foreach (GunEnemy e in FindObjectsOfType<GunEnemy>())
            {
                try
                {
                    if (e != null && !gunEnemList.Contains(e) && circleCol.IsTouching(e.damageCol.GetComponent<BoxCollider2D>()))
                    {
                        e.TakeDamage(enemDamageInt);
                        gunEnemList.Add(e);
                    }
                }
                catch
                {

                }
            }

            foreach (MGunEnemy e in FindObjectsOfType<MGunEnemy>())
            {
                try
                {
                    if (e != null && !mgunEnemList.Contains(e) && circleCol.IsTouching(e.damageCol.GetComponent<BoxCollider2D>()))
                    {
                        e.TakeDamage(enemDamageInt);
                        mgunEnemList.Add(e);
                    }
                }
                catch
                {

                }
                
            }
            foreach (NadeEnemy e in FindObjectsOfType<NadeEnemy>())
            {
                try
                {
                    if (e != null && !nadeEnemList.Contains(e) && circleCol.IsTouching(e.damageCol.GetComponent<BoxCollider2D>()))
                    {
                        e.TakeDamage(enemDamageInt);
                        nadeEnemList.Add(e);
                    }
                }
                catch
                {

                }
            }
        }
    }

    public void Activate()
    {
        enemList.Clear();
        gunEnemList.Clear();
        mgunEnemList.Clear();
        circleCol.enabled = true;
        active = true;
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(0.2f);
        active = false;
        circleCol.enabled = false;
        enemList.Clear();
        gunEnemList.Clear();
        mgunEnemList.Clear();
    }
}
