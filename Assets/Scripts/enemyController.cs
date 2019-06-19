using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] SpawnMaster spawnMaster;

    [SerializeField] public float timeLeft;

    private Rigidbody2D rigidBody;

    public int spawnPointIndex;
    private int currentMovingTarget;
    private Vector2 facingTarget;
    private Vector2 movingTarget;
    private float direction;
    private bool facingRight;
    public int selfType;

    // position boundaries
    public SpawnPointController spawnPoint;

    // AI factors
    [SerializeField] public EnemyAI enemyAI;

    // Start is called before the first frame update
    void Awake()
    {
        // get AI
        enemyAI = GetComponent<EnemyAI>(); // Why does this NULL out one me?

        // set time left    
        timeLeft = 60f;

        // get conductor
        spawnMaster = GameObject.FindObjectOfType<SpawnMaster>();

        // self
        rigidBody = GetComponent<Rigidbody2D>();

        // get borders
        spawnPoint = spawnMaster.spawnPoints[spawnPointIndex].GetComponent<SpawnPointController>();

        // Init AI
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            spawnMaster.RemoveEnemy(gameObject, selfType, spawnPointIndex);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Physics2D.GetIgnoreCollision(other.GetComponent<BoxCollider2D>(), gameObject.GetComponent<BoxCollider2D>());
        }
    }



    public void ReceiveDamage(float damage)
    {
        // TODO : ENEMY : Take Actual Damage
        Destroy(gameObject);
    }
}
