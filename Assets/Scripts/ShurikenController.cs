using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenController : MonoBehaviour
{
    // base reference
    ProjectileDepo prDepo;
    SpriteRenderer spriteRend;

    // self reference

    // self variables
    [SerializeField] public int selfType;
    [SerializeField] public float speed;
    [SerializeField] private float damage;
    [SerializeField] private float lifeSpan;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D rgbd = gameObject.GetComponent<Rigidbody2D>();
        //rgbd.velocity = new Vector2(speed, 0);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();
            enemy.ReceiveDamage(damage); // TODO : SMTHG WRONG
            Destroy(gameObject);
        }
        else if (other.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan <= 0)
        {
            Destroy(gameObject);
        }
    }
}
