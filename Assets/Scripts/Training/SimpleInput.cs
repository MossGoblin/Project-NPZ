using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInput : MonoBehaviour
{
    // refs

    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject unitModel;

    private bool hasUnit;

    // Start is called before the first frame update
    void Start()
    {
        hasUnit = false;
        GameObject unit = Instantiate(unitModel, spawnPoint.position, Quaternion.identity, spawnPoint);
        hasUnit = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

    }
}
