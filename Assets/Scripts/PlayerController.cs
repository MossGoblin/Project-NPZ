using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Dependencies
    // Ghost Connection
    private GhostProps ghosts;

    // Components
    SpriteRenderer heroRenderer;
    Transform heroTransform;
    [SerializeField] Transform[] playerBase;
    Rigidbody2D heroRigidBody;
    [SerializeField] Text timerDisplay;


    // Hero Stats
    [SerializeField] private string heroName;
    [SerializeField] private float heroJumpFactor;
    [SerializeField] private float heroMoveSpeed;
    [SerializeField] private bool heroDoubleJump;
    [SerializeField] private Sprite heroSprite;
    [SerializeField] private int heroState;
    [SerializeField] private float[] ActiveTimers;
    [SerializeField] private float[] CoolDownTimers;

    [SerializeField] private float tmpJumpFactor;

    // Hero Position Checks
    [SerializeField] private bool onGround; // Is the hero touching the ground
    [SerializeField] private bool didDoubleJump; // Did the player double jump already

    // Hero Status Checks
    [SerializeField] private bool shouldJump; // Trigger showing that the hero should jump
    [SerializeField] private float activeTimePickUp;
    [SerializeField] private float[] cooldownTimePickUp;

    // Ground Checks
    [SerializeField] private float playerBaseGroundRadius;
    [SerializeField] private LayerMask groundDef;

    // Variables
    private float hrMovement;

    // Temp Variables and Magic Numbers
    [SerializeField] private float timeDilationFactor;
    [SerializeField] private float activeCoolDown;

    // Start is called before the first frame update
    void Start()
    {
        // Establish ghost connection
        ghosts = FindObjectOfType<GhostProps>();
        Debug.Log(ghosts.PingMe());

        // Hero Components
        heroRenderer = GetComponent<SpriteRenderer>();
        heroTransform = GetComponent<Transform>();
        heroRigidBody = GetComponent<Rigidbody2D>();

        // SetTimers
        ActiveTimers = new float[3];
        CoolDownTimers = new float[3];
        for (int count = 0; count < 3; count++)
        {
            if (count == heroState)
            {
                ActiveTimers[count] = ghosts.hrMaxTimers[count];
                CoolDownTimers[count] = 0;
            }
            else
            {
                ActiveTimers[count] = 0;
                CoolDownTimers[count] = ghosts.hrMaxTimers[count];
            }
        }
        // Set default hero state
        heroState = 0;

        // Init from ghost 0
        UpdateHeroState();

        // Update Position Checks
        UpdatePositionChecks();
    }

    // Update is called once per frame

    void FixedUpdate()
    {
        Time.timeScale = timeDilationFactor;
        ResetValues();
        HandleInput();
        HandleMovement();
        UpdateTimers();
        CheckForTimeOuts();
    }

    private void CheckForTimeOuts()
    {
        if (ActiveTimers[heroState] == 0)
        {
            int newState = NextState();
            if (CoolDownTimers[newState] >= 1)
            {
                heroState = newState;
                UpdateHeroState();
                SwapTimers();
            }
            else
            {
                newState = NextState();
                if (CoolDownTimers[newState] >= 1)
                {
                    heroState = newState;
                    UpdateHeroState();
                    SwapTimers();
                }
                else
                {
                    // TODO Pause game
                    Debug.Log(heroName + " IZ DED!");
                    Time.timeScale = 0.0000001f;
                }
            }
        }
    }

    private int NextState()
    {
        int newState;
        if (heroState + 1 > 2)
        {
            newState = 0;
        }
        else
        {
            newState = heroState + 1;
        }
        return newState;
    }

    private void UpdateTimers()
    {
        timerDisplay.text = "";
        string heroTimerMarker = "";

        for (int count = 0; count < 3; count++)
        {
            if (count == heroState)
            {
                heroTimerMarker = "*";
                ActiveTimers[count] = Math.Max((ActiveTimers[count] - (ghosts.hrActiveTempo[count] * Time.deltaTime) + activeTimePickUp), 0);
                CoolDownTimers[count] = Math.Min((CoolDownTimers[count] + (ghosts.hrCoolDownTempo[count] * Time.deltaTime * activeCoolDown)), ghosts.hrMaxTimers[count]);
            }
            else
            {
                // TODO : WARNING MAGIC NUMBERS
                heroTimerMarker = ".";
                CoolDownTimers[count] = Math.Min((CoolDownTimers[count] + (ghosts.hrCoolDownTempo[count] * Time.deltaTime)), ghosts.hrMaxTimers[count]);
            }

            // Update Display
            timerDisplay.text += heroTimerMarker + " "  + ghosts.hrNames[count] + ": " + (int)ActiveTimers[count] + " (" + (int)CoolDownTimers[count] + ")\n";
        }
    }

    private void ResetValues()
    {
        //Debug.Log("Absorbed " + activeTimePickUp);
        activeTimePickUp = 0f;
        UpdatePositionChecks();
    }

    private bool IsOnGround()
    {
        if (heroRigidBody.velocity.y == 0)
        {
            foreach (Transform groundPoint in playerBase)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(groundPoint.position, playerBaseGroundRadius, groundDef);
                for (int count = 0; count < colliders.Length; count++)
                {
                    if (colliders[count].gameObject != gameObject);
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void UpdatePositionChecks()
    {
        onGround = IsOnGround();
        if (onGround)
        {
            didDoubleJump = false;
        }
        shouldJump = false;
    }

    private void UpdateHeroState()
    {
        heroName = ghosts.hrNames[heroState];
        heroJumpFactor = ghosts.hrJumpFactor[heroState];
        heroDoubleJump = ghosts.hrDoubleJump[heroState];
        heroMoveSpeed = ghosts.hrMoveSpeed[heroState];
        heroSprite = ghosts.hrSprites[heroState];
        heroRenderer.sprite = heroSprite;
        //Debug.Log("Hero changed to: " + heroName);
    }

    private void HandleInput()
    {
        HandleHeroStateChange();

        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Debug.Log("UP");
            shouldJump = true;
        }

        // Move Horizontal
        hrMovement = Input.GetAxis("Horizontal");
    }

    private void HandleMovement()
    {
        // Perform Jump
        if ((onGround && shouldJump) || (heroDoubleJump && shouldJump && !didDoubleJump)) 
        {
            //Debug.Log("Jumping");
            if (!onGround)
            {
                didDoubleJump = true;
            }
            onGround = false;
            heroRigidBody.AddForce(new Vector2(0, heroJumpFactor * tmpJumpFactor));
            shouldJump = false;
        }

        //// Horizontal movement only on ground
        //if (onGround)
        //{
        //    if (hrMovement > 0)
        //    {
        //        Debug.Log("Right!");
        //    }
        //    else if (hrMovement < 0)
        //    {
        //        Debug.Log("Left!");
        //    }
        //    heroRigidBody.velocity = new Vector2(hrMovement * heroMoveSpeed, heroRigidBody.velocity.y);
        //}

        // Horizontal movement with air control
        if (hrMovement > 0)
        {
            //Debug.Log("Right!");
        }
        else if (hrMovement < 0)
        {
            //Debug.Log("Left!");
        }
        heroRigidBody.velocity = new Vector2(hrMovement * heroMoveSpeed, heroRigidBody.velocity.y);
    }

    private void HandleHeroStateChange()
    {
        // Cycle Hero
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SwapTimers();
            heroState = NextState();
            UpdateHeroState();
            SwapTimers();
        }

        // Change Hero Directly
        if (heroState != 0 && (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)))
        {
            SwapTimers();
            heroState = 0;
            UpdateHeroState();
            SwapTimers();
        }
        if (heroState != 1 && (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)))
        {
            SwapTimers();
            heroState = 1;
            UpdateHeroState();
            SwapTimers();
        }
        if (heroState != 2 && (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)))
        {
            SwapTimers();
            heroState = 2;
            UpdateHeroState();
            SwapTimers();
        }
    }

    private void SwapTimers()
    {
        // Activation or Deactivation
        // Swap Active and CoolDown Timers for CURRENT hero

        float newActive = CoolDownTimers[heroState];
        float newCoolDown = ActiveTimers[heroState];
        //Debug.Log("New Active: " + newActive + " / New CoolDown: " + newCoolDown);
        ActiveTimers[heroState] = newActive;
        CoolDownTimers[heroState] = newCoolDown;
    }

    // Colliding with the Interactable
    void OnTriggerStay2D(Collider2D other)
    {

        // Meet the healer
        if (other.tag == "healer")
        {
            // TODO HERE
            Debug.Log("Heal");
            activeTimePickUp += 1f;
        }

        // Meet the enemy
        if (other.tag == "enemy")
        {
            Debug.Log("Damage");
            activeTimePickUp -= 0.5f;
        }
    }
}
