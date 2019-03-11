using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timeController : MonoBehaviour
{
    // References
    [SerializeField] private GameObject player;
    [SerializeField] private Ghost ghost;
    private heroController heroController;
    [SerializeField] private Image[] activeClockFace;
    [SerializeField] private Image[] cooldownClockFace;

    // Parameters
    [SerializeField] private float maxTime;
    [SerializeField] private float activeFlow;
    [SerializeField] private float cooldownFlow;

    // Clocks
    [SerializeField] private float[] acTimer;
    [SerializeField] private float[] cdTimer;

    // Modifiers
    [SerializeField] private float activeMod;
    [SerializeField] private float[] cooldownMod;
    [SerializeField] private float activationTH;

    [SerializeField] private int state;

    // Start is called before the first frame update
    void Start()
    {
        heroController = player.GetComponent<heroController>();
        acTimer = new float[3] { maxTime , 0, 0};
        cdTimer = new float[3] { 0, maxTime, maxTime };
        cooldownMod = new float[3] { 0, 0, 0 };
        // TODO : OLD : master : assign clockface colors
        //activeClockFace = new Image[3];
        //for (int count = 0; count < 3; count++)
        //{
        //    activeClockFace[count].color = ghost.hColor[count];
        //}

        activeFlow = -1;
        cooldownFlow = 0.25f;
        activeMod = 0;

        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimers();
        UpdateClocks();
        CheckTimeOuts();
        ClearMods();
    }

    private void CheckTimeOuts()
    {
        if (acTimer[state] <= 0)
        {
            int newState = FindNextState(state);
            UpdateState(newState);
            // TODO : OLD : master : fix dependenciesCall the hero controller to change states
            heroController.SwapLocalState(state);
        }
        
        //    if (atTimers[state] == 0)
        //    {
        //        int newState = DoFindSuitableHero();
        //        DoSwapHero(state, newState);
        //    }
    }

    private void ClearMods()
    {
        activeMod = 0;
        for (int count = 0; count < 3; count++)
        {
            cooldownMod[count] = 0;
        }
    }

    private void UpdateTimers()
    {
        // Update active timer
        float rawValue = acTimer[state] + activeFlow * Time.deltaTime + activeMod;
        acTimer[state] = Mathf.Clamp(rawValue, 0, maxTime);

        // Update cooldown timers
        for (int count = 0; count < 3; count++)
        {
            rawValue = cdTimer[count] + cooldownFlow * Time.deltaTime + cooldownMod[count];
            cdTimer[count] = Mathf.Clamp(rawValue, 0, maxTime);
        }
    }

    private void UpdateClocks()
    {
        for (int count = 0; count < 3; count++)
        {
            activeClockFace[count].fillAmount = NormalTime(acTimer[count], maxTime);
            cooldownClockFace[count].fillAmount = NormalTime(cdTimer[count], maxTime);
        }
    }

    private float NormalTime(float timer, float maxTime)
    {
        return timer / maxTime;
    }

    public bool IfGotTime(int heroCheck)
    {
        return (cdTimer[heroCheck] >= activationTH);
    }
    public void UpdateState(int incomingState)
    {
        //int newState = FindNextState(incomingState);
        SwapTimers(state, incomingState);
        state = incomingState;
    }

    private void SwapTimers(int oldState, int newState)
    {
            
        float tempTime = acTimer[oldState];
        acTimer[oldState] = cdTimer[oldState];
        cdTimer[oldState] = tempTime;

        tempTime = acTimer[newState];
        acTimer[newState] = cdTimer[newState];
        cdTimer[newState] = tempTime;

    }

    public int FindNextState(int oldState)
    {
        int heroCheck = NextState(oldState);
        if (cdTimer[heroCheck] < activationTH)
        {
            heroCheck = NextState(heroCheck);
            if (cdTimer[heroCheck] < activationTH)
            {
                throw new Exception("TimeOut");
                return oldState;
            }
        }
        return heroCheck;
    }

    private int NextState(int oldState)
    {
        oldState++;
        if (oldState > 2)
        {
            return 0;
        }
        else
        {
            return oldState;
        }
    }

    private int PrevState(int oldState)
    {
        oldState--;
        if (oldState < 2)
        {
            return 2;
        }
        else
        {
            return oldState;
        }
    }

    public void ReceiveTimeMod(int affState, bool active, float amount)
    {
        if (active)
        {
            Debug.Log("active dmg: " + amount);
            activeMod += amount;
        }
        else
        {
            Debug.Log("cooldown " + affState + ": " + amount);
            cooldownMod[affState] += amount;
        }

    }

    public void ReceiveTimeBonus(int sourceId, float bonusAmount)
    {
        int isEasyFor = sourceId;
        int isHardFor = NextState(sourceId);
        if (isHardFor == state)
        {
            activeMod += bonusAmount;
            cooldownMod[isEasyFor] += bonusAmount;
        }
        if (isEasyFor != state)
        {
            cooldownMod[isEasyFor] += bonusAmount;
        }
    }
}
