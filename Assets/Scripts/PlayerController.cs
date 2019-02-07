using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Movement states
    private bool midAir;

    // Ghost Connections Variables
    public GhostProps ghostProps;

    // Variables
    [SerializeField] private string heroName;
    public float heroJumpMag;

    // ghost data
    public string[] ghostNames = new string[3] { "Ninja", "Pirate", "Zombie" };
    public Sprite[] ghostSprites = new Sprite[3];
    public float[] ghostJumpMags = new float[3];
    public float[] ghostClocks;

    private SpriteRenderer spriteRenderer;
    private int crrHero;
    private Rigidbody2D heroRigidBody;

    // Methods
    void Start()
    {
        // Ghost Connections Init
        ghostProps = GetComponent<GhostProps>();

        // Init states
        midAir = false;

        // Array Attempts
        ghostJumpMags[0] = 15f;
        ghostJumpMags[1] = 10f;
        ghostJumpMags[2] = 5f;

        // Init character
        heroRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        crrHero = 0;
        SwitchTo(0);

    }

    void FixedUpdate()
    {
        HandleInput();
        UpdateStates();
    }

    private void UpdateStates()
    {
        if (heroRigidBody.velocity.y != 0)
        {
            midAir = true;
        }
    }

    private void HandleInput()
    {
        // Hero Switch Controls
        if (Input.GetKeyDown("1"))
        {
            SwitchTo(0);
        }
        if (Input.GetKeyDown("2"))
        {
            SwitchTo(1);
        }
        if (Input.GetKeyDown("3"))
        {
            SwitchTo(2);
        }
        if (Input.GetKeyDown("q"))
        {
            crrHero = CycleState(crrHero);
        }

        // Movement Controls
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.Log("Up / " + midAir);
            HeroJump();
        }

        // Test Input
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log(ghostProps.PingMe());
        }
    }

    private void HeroJump()
    {
        heroRigidBody.velocity = Vector2.up * heroJumpMag;
    }

    private int CycleState(int crrHero)
    {
        if (crrHero + 1 > 2)
        {
            crrHero = 0;
        }
        else
        {
            crrHero++;
        }
        SwitchTo(crrHero);
        return crrHero;
    }

    private void SwitchTo(int newState)
    {
        this.heroName = ghostNames[newState];
        this.heroJumpMag = ghostJumpMags[newState];
        spriteRenderer.sprite = ghostSprites[newState];
    }
}
