using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMaster : MonoBehaviour
{
    // References
    [SerializeField] private Conductor conductor;
    private int currentState;

    // HeroState
    private int heroStatus;

    // UI Elements
    public Image activeClock;
    public Image[] cooldownClocks;
    public Color disabledClock;

    // Timers
    public float activeTimer;
    public float[] cooldownTimers;

    // Tokens
    public float timeFlow;
    public float cooldownFactor;
    public float activeToken;
    public float[] cooldownTokens;


    // Start is called before the first frame update
    void Start()
    {
        // Init arrays
        cooldownTimers = new float[3];
        cooldownTokens = new float[3];

        currentState = conductor.heroStatus;

        InitTimers();
        UpdateClocks();
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
        UpdateTimers();
        UpdateClocks();
    }

    private void CheckState()
    {
        heroStatus = conductor.heroStatus;
    }

    private void InitTimers()
    {
        for (int count = 0; count < 3; count++)
        {
            activeTimer = conductor.ghostMaster.defaultActiveTimer;
            if (count != currentState)
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
