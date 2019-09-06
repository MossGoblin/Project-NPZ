using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // references
    EnemyController controller;
    private Transform player;

    // bounds
    private Transform boundOne;
    private Transform boundTwo;
    private Transform[] bounds;
    
    // spatial
    Rigidbody2D rigidBody;
    private float direction;

    // game stats
    public float health;
    public float healthMax;
    public float speed;
    public float[] speeds;
    [SerializeField] private int operationMode;


    // targets
    private Vector2 moveTarget;
    public Vector2 attackTarget;
    private Vector2 faceTarget;
    private int moveTargetIndex;
    public bool heroInRange;
    public bool calmState;

    // Start is called before the first frame update
    void Awake()
    {
        // references
        rigidBody = GetComponent<Rigidbody2D>();
        controller = GetComponentInParent<EnemyController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        // init spatial
        // get parent point
        Transform parentTransform = GetComponentInParent<Transform>();
        SpawnPointController spController = GetComponentInParent<SpawnPointController>();

        // get bound transforms
        bounds = new Transform[2];
        bounds[0] = spController.borders[0];
        bounds[1] = spController.borders[1];

        // spatial
        moveTargetIndex = 0;
        moveTarget = bounds[moveTargetIndex].position;
        faceTarget = moveTarget;
        direction = 1;
        speeds = new float[] { 15, 18, 20, 25 };

        // game stats
        healthMax = 100;
        health = healthMax;
        operationMode = 0;
        //speed = speeds[0];
        speed = 20;
        heroInRange = false;
        calmState = true;

        Debug.Log("Initiated");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            ModifyHealth(-20f);
        }

        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            ModifyHealth(+20f);
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            //Cycle();
        }

        ControlMovement();
    }

    private void ControlMovement()
    {
        // check unit status
        UpdateStatus();

        // update direction
        UpdateDirection();

        // apply movement
        Move();
    }

    private void UpdateDirection()
    {
        if ((direction == 1 && transform.position.x > moveTarget.x)
            ||
            (direction == -1 && transform.position.x < moveTarget.x))
        {
            // Change MoveTarget
            moveTargetIndex = 1 - moveTargetIndex;
            moveTarget = bounds[moveTargetIndex].position;
            //Debug.Log($"Change Target to {moveTargetIndex}");
        }

        // update facing target
        if (heroInRange)
        {
            faceTarget = attackTarget;
        }
        else
        {
            faceTarget = moveTarget;
        }

        if (
            (faceTarget.x > transform.position.x) && (direction == -1)
            ||
            (faceTarget.x < transform.position.x) && (direction == 1)
            )
        {
            // flip unit
            direction *= -1;
            UpdateModelFacing();
        }
    }

    private void UpdateModelFacing()
    {
        float newScaleX = transform.localScale.x * -1;
        transform.localScale = new Vector3(newScaleX, transform.localScale.y, transform.localScale.z);
        //Debug.Log("New facing");
    }

    private void Move()
    {
        if (heroInRange == false)
        {
            transform.position = new Vector3(transform.position.x + (speed / 10 * direction) * Time.deltaTime, transform.position.y);
        }
    }

    public void ModifyHealth(float healthModifier)
    {
        health = Mathf.Clamp(health + healthModifier, 0, healthMax);
        Debug.Log($"Health => {this.health}/{this.healthMax}");
    }

    private void UpdateStatus()
    {
        operationMode = 0;
        if (health <= healthMax / 3)
        {
            operationMode += 2;
        }
        else if (health <= healthMax / 2)
        {
            operationMode += 1;
        }

        if (!calmState)
        {
            operationMode += 1;
        }

        speed = speeds[operationMode];
    }

    public void Cycle()
    {
        if (operationMode == 3)
        {
            operationMode = 0;
        }
        else
        {
            operationMode = operationMode + 1;
        }
        speed = speeds[operationMode];
        Debug.Log($"Mode => {operationMode.ToString()}");
        Debug.Log($"Speed => {this.speed}");
    }
}
