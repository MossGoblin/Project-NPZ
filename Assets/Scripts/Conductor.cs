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

    // Start is called before the first frame update
    void Start()
    {
        heroDefault = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
