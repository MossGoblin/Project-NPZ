using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBulletAI : MonoBehaviour
{
    public float lifespan;
    [SerializeField] private GameObject player;
    [SerializeField] public float dmg;
    private heroController playerController;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<heroController>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifespan)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            // hit actvive state, active timer, damage amount
            playerController.DoReceiveTimeMod(true, dmg);
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
