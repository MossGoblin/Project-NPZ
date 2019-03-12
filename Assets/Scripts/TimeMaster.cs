using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMaster : MonoBehaviour
{
    // References
    [SerializeField] private Conductor conductor;

    // HeroState
    private int heroStatus;

    // UI Elements
    public Image activeClock;
    public Image[] cooldownClocks;
    public Color disabledClock;

    // Hero UI Clock Reference
    public Image heroClock;

    // Timers
    public float activeTimer;
    public float[] cooldownTimers;
    public float swapThreshold;

    // Tokens
    public float timeFlow;
    public float cooldownFactor;
    public float activeToken;
    public float[] cooldownTokens;

    // Input variables
    public float horizontal;


    // Start is called before the first frame update
    void Start()
    {
        //// Hero UI Clock access point
        //heroClock = conductor.heroMaster.GetComponentInChildren<Image>();

        // Init arrays
        cooldownTimers = new float[3];
        cooldownTokens = new float[3];

        UpdateState();

        InitTimers();
        UpdateClocks();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateState();
        UpdateTimers();
        UpdateClocks();
        CheckTimeOut();
    }

    private void UpdateState()
    {
        heroStatus = conductor.heroStatus;
    }

    private void InitTimers()
    {
        for (int count = 0; count < 3; count++)
        {
            activeTimer = conductor.ghostMaster.defaultActiveTimer;
            if (count != heroStatus)
            {
                cooldownTimers[count] = conductor.ghostMaster.defaultCoolDownTimer;
            }
            else
            {
                cooldownTimers[count] = 0;
            }
        }
    }

    private void UpdateTimers()
    {
        // Reduce active timer for current state
        float newTime = activeTimer + timeFlow * Time.deltaTime + activeToken;
        activeTimer = Mathf.Clamp(newTime, 0, conductor.ghostMaster.defaultActiveTimer);

        // Increase cooldown timers for the next two states
        int nextState = NextState(heroStatus);
        newTime = cooldownTimers[nextState] + cooldownFactor * Time.deltaTime + cooldownTokens[nextState];
        cooldownTimers[nextState] = Mathf.Clamp(newTime, 0, conductor.ghostMaster.defaultCoolDownTimer);
        nextState = NextState(nextState);
        newTime = cooldownTimers[nextState] + cooldownFactor * Time.deltaTime + cooldownTokens[nextState];
        cooldownTimers[nextState] = Mathf.Clamp(newTime, 0, conductor.ghostMaster.defaultCoolDownTimer);
    }

    private void UpdateClocks()
    {
        // Update Clock Fill
        for (int count = 0; count < 3; count++)
        {
            float activeNormalized = Mathf.Clamp(activeTimer, 0, conductor.ghostMaster.defaultActiveTimer) / conductor.ghostMaster.defaultActiveTimer;
            activeClock.fillAmount = activeNormalized;
            float cooldownNormalized = Mathf.Clamp(cooldownTimers[count], 0, conductor.ghostMaster.defaultCoolDownTimer) / conductor.ghostMaster.defaultCoolDownTimer;
            cooldownClocks[count].fillAmount = cooldownNormalized;

            // Adjust HeroClock Position
            //RectTransform heroRectTransform = conductor.heroMaster.GetComponent<RectTransform>();
            //Transform heroRectTransform = conductor.heroMaster.GetComponent<Transform>();
            //Vector3 heroPosition = heroRectTransform.localPosition;
            //Vector3 heroClockPosition = new Vector3(heroPosition.x + 10, heroPosition.y + 70, heroPosition.z);
            //Transform heroClockRect = heroClock.GetComponent<Transform>();
            //heroClockRect.localPosition = heroClockPosition;

            // Update HeroClock Color and FillAmount
            heroClock.color = conductor.ghostMaster.colors[heroStatus];
            heroClock.fillAmount = activeNormalized;
        };

        // Update Active Color
        activeClock.color = conductor.ghostMaster.colors[heroStatus];

        //// Update CoolDown Colors
        //for (int count = 0; count < 3; count++)
        //{
        //    float alphaNormal = Mathf.Clamp(cooldownTimers[count], 0, conductor.ghostMaster.defaultCoolDownTimer) * 255 / 30;
        //    Color newColor = cooldownClocks[count].color;
        //    newColor.a = alphaNormal;
        //    cooldownClocks[count].color = newColor;
        //}

        // Update CoolDown Scale
        for (int count = 0; count < 3; count++)
        {
            float timeNormal = cooldownTimers[count];
            float alphaNormal = timeNormal / conductor.ghostMaster.defaultCoolDownTimer;
            alphaNormal *= 0.8f;
            Vector3 newScale = new Vector3(alphaNormal, alphaNormal, alphaNormal);
            cooldownClocks[count].transform.localScale = newScale;
        }
    }

    public void Swap(int state)
    {
        // Swap timers for current hero
        float tempTimer = activeTimer;
        activeTimer = cooldownTimers[heroStatus];
        cooldownTimers[heroStatus] = tempTimer;
        // Swap current hero
        heroStatus = state;
        // Swap timers for new hero
        tempTimer = activeTimer;
        activeTimer = cooldownTimers[heroStatus];
        cooldownTimers[heroStatus] = tempTimer;
    }

    private void CheckTimeOut()
    {
        // check if current time is out
        if (activeTimer <= 0)
        {
            // find if there is available state
            if ((cooldownTimers[NextState(heroStatus)] >= swapThreshold) || (cooldownTimers[NextState(NextState(heroStatus))] >= swapThreshold))
            {
                // if there is - get the next available state
                int nextState = GetNextState();
                // push state swap
                Swap(nextState);
                conductor.SwapByTimeOut(heroStatus);
                // TODO : here
            }
            else
                conductor.RestartLevel("Timed out");
        }
    }

    private int GetNextState()
    {
        int nextState = NextState(heroStatus);
        if (cooldownTimers[nextState] >= swapThreshold)
        {
            return nextState;
        }
        else
        {
            return NextState(nextState);
        }
    }

    private int NextState(int state)
    {
        if (state + 1 > 2)
        {
            return 0;
        }
        else
        {
            return state + 1;
        }
    }
}
