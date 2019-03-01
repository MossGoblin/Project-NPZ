using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class heroController : MonoBehaviour
{
    // Dependencies and Components
    [SerializeField] Ghost ghost;
    private Rigidbody2D rgbd;
    private Transform trans;
    private SpriteRenderer rnd;
    [SerializeField] Transform[] playerBase;
    [SerializeField] private LayerMask groundDef;
    [SerializeField] private Sprite deadHero;
    [SerializeField] Transform shooter;
    [SerializeField] GameObject bullets;
    [SerializeField] GameObject gameMaster;
    private timeController master;
        
    // Clocks
    [SerializeField] private Image acOne;
    [SerializeField] private Image cdOne;
    [SerializeField] private Image acTwo;
    [SerializeField] private Image cdTwo;
    [SerializeField] private Image acThree;
    [SerializeField] private Image cdThree;

    // Current hero stats
    [SerializeField] private int state;
    private string hName;
    private Sprite sprite;
    public Color color;
    private float mvSpeed;
    private bool canDoubleJump;
    [SerializeField] private float JumpPower;

    // Timers
    [SerializeField] private float[] atTimers;
    [SerializeField] private float[] cdTimers;
    [SerializeField] private float atToken;
    [SerializeField] private float[] cdTokens;
    [SerializeField] private float dmgPool;

    // Magic Time Numbers
    [SerializeField] private float maxTimerTime;
    [SerializeField] private float coolDownFactor;
    [SerializeField] private float activationTH;

    // Magic Attack Numbers
    [SerializeField] private float bulletSpeed;

    // Spatial and Movement stats
    private float hrMove;
    [SerializeField] private bool didDoubleJump;
    [SerializeField] private bool onGround;
    private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
        // Dependencies
        rgbd = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        rnd = GetComponent<SpriteRenderer>();
        master = gameMaster.GetComponent<timeController>();

        // Init STARTING hero
        state = 0;
        SetHero();

        // Init Spatial
        didDoubleJump = false;
        onGround = IfOnGround();
        facingRight = true;

        // Set Timers
        atTimers = new float[3];
        cdTimers = new float[3];
        cdTokens = new float[3];
        for (int count = 0; count < 3; count++)
        {
            atTimers[count] = ghost.hActiveTimer[count];
            cdTimers[count] = ghost.hCoodownTimer[count];
            cdTokens[count] = ghost.hCooldownToken[count];
        }
        //atToken = -1;
        //maxTimerTime = 30.00f;
        //cdTokens[state] = 0;

        // Clock Action
        //acOne.color = ghost.hColor[0];
        //acTwo.color = ghost.hColor[1];
        //acThree.color = ghost.hColor[2];
        //ActionClocks();
    }

    // Update is called once per frame
    private void Update()
    {
        ManageInput();
        //ActionTimers();
        //ActionClocks();
        //UpdateTimeOuts();
        UpdateSpatialState();
    }

    void FixedUpdate()
    {
        //SetHero();
    }

    private void HeroFlip(float horizontal)
    {
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 newScale = trans.localScale;
            newScale.x *= -1;
            trans.localScale = newScale;
        }
    }
    private bool IfOnGround()
    {
        if (rgbd.velocity.y <= 0)
        {
            foreach (Transform groundPoint in playerBase)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(groundPoint.position.x, groundPoint.position.y), 0.2f, groundDef);
                for (int count = 0; count < colliders.Length; count++)
                {
                    if (colliders[count].gameObject != gameObject)
                    {
                        didDoubleJump = false;
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void SetHero()
    {
        // Identity
        hName = ghost.hNames[state];
        sprite = ghost.hSprites[state];
        rnd.sprite = sprite;
        color = ghost.hColor[state];
        rnd.color = color;

        // Movement stats
        mvSpeed = ghost.hMoveSpeed[state];
        canDoubleJump = ghost.hDoubleJump[state];
        JumpPower = ghost.hJumpPower[state];
    }

    private void UpdateSpatialState()
    {
        onGround = IfOnGround();
        HeroFlip(hrMove);
        //atToken = -1;
    }

    //private void UpdateTimeOuts()
    //{
    //    if (atTimers[state] == 0)
    //    {
    //        int newState = DoFindSuitableHero();
    //        DoSwapHero(state, newState);
    //    }
    //}

    private void ManageInput()
    {
        // Hero State Changes
        // Cycle
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // ask GM for next viable state
            int nextHero = master.FindNextState(state);
            if (nextHero != state)
            {
                DoSwapHero(state, nextHero);
            }
        }

        // Precision State Change
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // TODO : hero : find if the selected state has time from GameMaster
            if (state != 0 && master.IfGotTime(0))
            {
                DoSwapHero(state, 0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (state != 1 && master.IfGotTime(1))
            {
                DoSwapHero(state, 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (state != 2 && master.IfGotTime(2))
            {
                DoSwapHero(state, 2);
            }
        }

        // Movement Queue
        hrMove = Input.GetAxis("Horizontal");
        HeroMove();

        // Jumping Queue
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HeroJump();
        }

        // Attack Queue
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DoShootABullet();
        }
    }

    private int DoFindSuitableHero()
    {
        int heroCheck = NextState(state);
        if (cdTimers[heroCheck] < activationTH)
        {
            heroCheck = NextState(heroCheck);
            if (cdTimers[heroCheck] < activationTH)
            {
                Debug.Log("Time Out!");
                rnd.sprite = deadHero;
                // TODO : HERE
                //throw new Exception("TimedOut");
                throw new Exception("TimeOut");
                return state;
            }
        }
        return heroCheck;
    }

    // MOVED TO TimeMaster
    //private bool IfGotTime(int heroCheck)
    //{
    //    return (cdTimers[heroCheck] >= activationTH);
    //}

    private void DoSwapHero(int oldState, int newState)
    {
        //DoSwapTimers(oldState, false);
        SwapLocalState(newState);
        // TODO : HERE : hero : Send Message to TimeMaster to Swap Clocks
        master.UpdateState(state);
        //DoSwapTimers(state, true);
    }

    public void SwapLocalState(int newState)
    {
        state = newState;
        SetHero();
    }

    public void HeroJump()
    {
        if (onGround)
        {
            rgbd.AddForce(new Vector2(0, JumpPower));
            onGround = IfOnGround();
        }
        else if (!onGround && canDoubleJump && !didDoubleJump)
        {
            rgbd.velocity = new Vector2(rgbd.velocity.x, 0);
            didDoubleJump = true;
            rgbd.AddForce(new Vector2(0, JumpPower * 0.75f));
            onGround = IfOnGround();
        }
    }

    public void HeroMove()
    {
        // VELOCITY OPTION
        if (hrMove != 0)
        {
            rgbd.velocity = new Vector2(hrMove * mvSpeed, rgbd.velocity.y);
        }
        if (hrMove < 0)
        {
            
        }

        // ADD FORCE OPTION
        //if (hrMove != 0)
        //{
        //    rgbd.AddForce(new Vector2(Mathf.Sign(hrMove) * mvSpeed, rgbd.velocity.y));
        //}
        // TRANSLATE OPTION
        //if (hrMove != 0)
        //{
        //    trans.Translate(Mathf.Sign(hrMove) * mvSpeed * Time.deltaTime, 0, 0);
        //}
    }

    private int NextState(int oldState)
    {
        oldState ++;
        if (oldState > 2)
        {
            return 0;
        }
        else
        {
            return oldState;
        }
    }

    private float NormalTime(float timer, float maxTime)
    {
        return timer / maxTime;
    }

    //private void DoSwapTimers(int hero, bool newHero)
    //{
    //    if (newHero) // Activation
    //    {
    //        cdTokens[hero] = 0;
    //    }
    //    else
    //    {
    //        cdTokens[hero] = ghost.hCooldownToken[hero];
    //    }
    //    float tempTimer;
    //    tempTimer = atTimers[hero];
    //    atTimers[hero] = cdTimers[hero];
    //    cdTimers[hero] = tempTimer;
    //    //cdTokens[hero] = 0;
    //}

    // MOVED TO TimeMaster
    //private void ActionTimers()
    //{
    //    // Update Active Timer by ActiveTimeToken
    //    atTimers[state] = Mathf.Clamp((atTimers[state] - Time.deltaTime - dmgPool), 0, maxTimerTime);
    //    //atTimers[state] = Mathf.Clamp((atTimers[state] + atToken * Time.deltaTime - dmgPool), 0, maxTimerTime);

    //    // Update CoolDown Timers by CoolDownTokens
    //    for (int count = 0; count < 3; count++)
    //    {
    //        cdTimers[count] = Mathf.Clamp(cdTimers[count] + cdTokens[count] * Time.deltaTime * coolDownFactor, 0, maxTimerTime);
    //    }
    //    //atToken = 0;
    //    dmgPool = 0;
    //    atToken = -1;
    //}

    // MOVED TO TimeMaster
    //private void ActionClocks()
    //{

        //acOne.fillAmount = NormalTime(atTimers[0], maxTimerTime);
        //cdOne.fillAmount = NormalTime(cdTimers[0], maxTimerTime);
        //acTwo.fillAmount = NormalTime(atTimers[1], maxTimerTime);
        //cdTwo.fillAmount = NormalTime(cdTimers[1], maxTimerTime);
        //acThree.fillAmount = NormalTime(atTimers[2], maxTimerTime);
        //cdThree.fillAmount = NormalTime(cdTimers[2], maxTimerTime);
    //}

    public void DoReceiveTimeMod(int dmgState, bool active, float dmgAmount)
    {
        master.ReceiveTimeMod(dmgState, active, dmgAmount);
    }

    public void DoReceiveTimeMod(bool active, float dmgAmount)
    {
        master.ReceiveTimeMod(state, active, dmgAmount);
    }

    private void DoShootABullet()
    {
        //Debug.Log("Shot a bullet");

        Vector2 position;
        Vector2 direction = shooter.position - transform.position;
        direction.Normalize();
        Quaternion rotation;
        position = shooter.position;
        rotation = shooter.rotation;
        GameObject newBullet = Instantiate(bullets, position, rotation);
        newBullet.GetComponent<Rigidbody2D>().velocity = ghost.bulletSpeed[state] * direction;
        newBullet.GetComponent<SpriteRenderer>().color = color;
        heroBulletAI bulletAI = newBullet.GetComponent<heroBulletAI>();
        bulletAI.lifespan = ghost.bulletLifespan[state];
        bulletAI.damage = ghost.bulletDamage[state];
    }

}
