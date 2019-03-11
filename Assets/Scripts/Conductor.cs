using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    // References
    [SerializeField] public GhostMaster ghostMaster;
    [SerializeField] public HeroMaster heroMaster;
    [SerializeField] public TimeMaster timeMaster;

    // Hero Status
    [SerializeField] private int heroDefault;
    public int heroStatus;

    // Hero Movement Variables
    public float horizontal;

    // Start is called before the first frame update
    void Start()
    {
        heroDefault = 0;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
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
    private void HandleInput()
    {
        // Enter to swap heroes
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Debug.Log("CC: Return Pressed");
            heroStatus = NextState(heroStatus);
            SwapByInput();
        }

        // Enter to change to hero
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (heroStatus != 0)
            {
                heroStatus = 0;
                SwapByInput();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (heroStatus != 1)
            {
                heroStatus = 1;
                SwapByInput();
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (heroStatus != 2)
            {
                heroStatus = 2;
                SwapByInput();
            }
        }

        // Move sideways
        horizontal = Input.GetAxis("Horizontal");

        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            heroMaster.Jump();
        }
    }

    public void SwapByInput()
    {
        //Debug.Log("CC: SwapByInput Called");
        heroMaster.Swap(heroStatus);
        timeMaster.Swap(heroStatus);
    }

    public void SwapByTimeOut(int state)
    {
        heroStatus = state;
        heroMaster.Swap(state);
    }
}
