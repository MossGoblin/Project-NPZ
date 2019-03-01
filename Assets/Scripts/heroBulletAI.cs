using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heroBulletAI : MonoBehaviour
{
    [SerializeField] public float lifespan;
    private float timer;
    [SerializeField] public int damage;
    private GameObject enemy;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
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
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            // hit actvive state, active timer, damage amount
            other.gameObject.GetComponent<enemyController>().TakeDamage(damage);
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
