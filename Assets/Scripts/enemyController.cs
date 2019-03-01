using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public GameObject bullet;
    public Transform shootingPoint;
    public Transform target;
    [SerializeField] private timeController master;
    public float bulletInterval;
    public float bulletSpeed;
    public float bulletLifeSpan;
    public float damage;
    public int identity;
    public int totalLifePoints;
    public float timeToken;
    private int lifePoints;

    private float bulletTimer;

    // Start is called before the first frame update
    void Start()
    {
        lifePoints = totalLifePoints;
    }

// Update is called once per frame
void Update()
    {
        bulletTimer += Time.deltaTime;
        if (bulletTimer >= bulletInterval)
        {
            bulletTimer = 0;
            Shoot();
        }
        SwapFacing();
    }

    private void SwapFacing()
    {
        // get current scale
        Vector3 newScale = transform.localScale;
        // get direction
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        // if the current scale and the direction are with opposite signs
        if (FacingOpposite(direction.x, newScale.x))
        {
            // adjust local scale
            newScale.x *= -1;
            transform.localScale = newScale;
        }
    }

    private bool FacingOpposite(float valueOne, float valueTwo)
    {
        return !((valueOne >= 0 && valueTwo >= 0) || (valueOne < 0 && valueTwo < 0));
    }

    private void Shoot()
    {
        Vector2 direction = target.transform.position - transform.position;
        direction.Normalize();
        //Debug.Log("direction: " + direction);
        GameObject newBullet = Instantiate(bullet, shootingPoint.transform.position, shootingPoint.transform.rotation);
        //Debug.Log("bulletspawn");
        newBullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        newBullet.GetComponent<enemyBulletAI>().dmg = damage;
        newBullet.GetComponent<enemyBulletAI>().lifespan = bulletLifeSpan;

        //Debug.Log("bullet velocity: " + direction * bulletSpeed + " / " + newBullet.GetComponent<Rigidbody2D>().velocity);
    }

    public void TakeDamage(int dmgTaken)
    {
        Debug.Log("hit to " + lifePoints + " / " + (lifePoints - dmgTaken));
        lifePoints -= dmgTaken;
        if (lifePoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        master.ReceiveTimeBonus(identity, 3);
        Destroy(gameObject);
    }
}
