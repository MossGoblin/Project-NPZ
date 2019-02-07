using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Dependencies
    // Ghost Connection
    private GhostProps ghosts;

    // Components
    SpriteRenderer heroRenderer;
    Transform heroTransform;
    [SerializeField] Transform playerBase;
    Rigidbody2D heroRigidBody;

    // Hero Stats
    [SerializeField] private string heroName;
    [SerializeField] private float heroJumpFactor;
    [SerializeField] private float heroMoveSpeed;
    [SerializeField] private bool heroDoubleJump;
    [SerializeField] private Sprite heroSprite;
    [SerializeField] private int heroState;

    // Hero Position Checks
    [SerializeField] private bool midAir;

    // Ground Checks
    [SerializeField] private float playerBaseGroundRadius;
    [SerializeField] LayerMask groundDef;



    // Start is called before the first frame update
    void Start()
    {
        // Establish ghost connection
        //ghosts = FindObjectOfType<GhostProps>().GetComponent<GhostProps>();
        ghosts = FindObjectOfType<GhostProps>();
        Debug.Log(ghosts.PingMe());

        // Hero Components
        heroRenderer = GetComponent<SpriteRenderer>();
        heroTransform = GetComponent<Transform>();
        heroRigidBody = GetComponent<Rigidbody2D>();

        // Set default hero state
        heroState = 0;

        // Init from ghost 0
        UpdateHeroState();

        // Update Position Checks
        UpdatePositionChecks();
    }

    // Update is called once per frame

    void Update()
    {
        HandleInput();

    }

    private void UpdatePositionChecks()
    {
        // TODO : Check here: https://www.youtube.com/watch?v=05TCTrpGB-4&index=9&list=PLX-uZVK_0K_6VXcSajfFbXDXndb6AdBLO
        midAir = true;
        Collider2D baseCollider = Physics2D.OverlapCircle(playerBase.position, playerBaseGroundRadius, groundDef);
        if (baseCollider.gameObject != gameObject)
        {
            Debug.Log("TouchDown!");
            midAir = false;
        }
    }

    private void UpdateHeroState()
    {
        heroName = ghosts.hrNames[heroState];
        heroJumpFactor = ghosts.hrJumpFactor[heroState];
        heroDoubleJump = ghosts.hrDoubleJump[heroState];
        heroMoveSpeed = ghosts.hrMoveSpeed[heroState];
        heroSprite = ghosts.hrSprites[heroState];
        heroRenderer.sprite = heroSprite;
        Debug.Log("Hero changed to: " + heroName);
    }

    private void HandleInput()
    {
        HandleHeroStateChange();
        HandleMovement();
    }

    private void HandleMovement()
    {
        //// TEMPORARY MOVEMENT MECHANIC
        //var heroHMv = Input.GetAxis("Horizontal");
        //var heroVMv = Input.GetAxis("Vertical");
        //if (heroHMv != 0 || heroVMv != 0)
        //{
        //    heroTransform.Translate(heroHMv * heroMoveSpeed * Time.deltaTime, heroVMv * heroMoveSpeed* Time.deltaTime, heroTransform.position.z);
        //}

        // TODO : Jump Attempt
    }

    private void HandleHeroStateChange()
    {
        // Cycle Hero
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (heroState + 1 > 2)
            {
                heroState = 0;
            }
            else
            {
                heroState++;
            }
            UpdateHeroState();
        }

        // Change Hero Directly
        if (heroState != 0 && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)))
        {
            heroState = 0;
            UpdateHeroState();
        }
        if (heroState != 1 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)))
        {
            heroState = 1;
            UpdateHeroState();
        }
        if (heroState != 2 && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)))
        {
            heroState = 2;
            UpdateHeroState();
        }
    }
}
