using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

public class Stats : MonoBehaviour
{
    PlayerSquare player;
    public ParticleSystem deathParticles;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private int health;
    bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = 1;
        dead = false;
        player = GetComponent<PlayerSquare>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 0)
        {
            print("die");
        }
        else if (!dead)
        {
            dead = true;
            deathParticles.Play();
            print("die");
            player.Disable();
            GameObject.FindGameObjectWithTag("MainCamera").transform.DOShakePosition(.3f, .6f, 15, 90, false, true);
            StartCoroutine(LevelLoadDelay());
        }
    }

    private IEnumerator LevelLoadDelay()
    {
        yield return new WaitForSeconds(0.7f);
    }
}
