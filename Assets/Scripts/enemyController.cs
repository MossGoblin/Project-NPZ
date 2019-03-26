using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] SpawnMaster spawnMaster;

    [SerializeField] public float timeLeft;
    [SerializeField] public float lifePonits;
    [SerializeField] public float speed;

    private Rigidbody2D rigidBody;

    public int spawnPointIndex;
    public int selfType;
    private int currentMovingTarget;
    private Vector2 facingTarget;
    private Vector2 movingTarget;
    private float direction;

    // position boundaries
    private SpawnPointController spawnPoint;
    private Vector2 boundOne;
    private Vector2 boundTwo;


    // Start is called before the first frame update
    void Start()
    {
        // set time left    
        timeLeft = 60f;

        // get conductor
        spawnMaster = GameObject.FindObjectOfType<SpawnMaster>();

        // self
        rigidBody = GetComponent<Rigidbody2D>();

        // get borders
        spawnPoint = spawnMaster.spawnPoints[spawnPointIndex].GetComponent<SpawnPointController>();
        boundOne = spawnPoint.borders[0].position;
        boundTwo = spawnPoint.borders[1].position;

        // orient
        facingTarget = boundOne;
        movingTarget = boundOne;
        currentMovingTarget = 0;
        GetDirection();
    }

    private void GetDirection()
    {
        if (Math.Abs(movingTarget.x - transform.position.x) <= 0.25)
        {
            ChangeDitection();
        }

        if (movingTarget.x >= transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    private void ChangeDitection()
    {
        currentMovingTarget = 1 - currentMovingTarget;
        facingTarget = spawnPoint.borders[currentMovingTarget].position;
        movingTarget = spawnPoint.borders[currentMovingTarget].position;
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

        MoveAround();
        GetDirection();
    }

    private void MoveAround()
    {
        rigidBody.velocity = new Vector2(speed * direction * Time.deltaTime, rigidBody.velocity.y);
    }

    public void ReceiveDamage(float damage)
    {
        // TODO : ENEMY : Take Dagame
        Destroy(gameObject);
    }
}
