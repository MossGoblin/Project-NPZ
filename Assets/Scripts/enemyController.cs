using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] SpawnMaster spawnMaster;

    [SerializeField] public float timeLeft;

    public int spawnPointIndex;
    public int selfType;

    // Start is called before the first frame update
    void Start()
    {
        // set time left
        timeLeft = 5f;

        // get conductor
        spawnMaster = GameObject.FindObjectOfType<SpawnMaster>();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            spawnMaster.RemoveEnemy(gameObject, selfType, spawnPointIndex);
            Destroy(gameObject);
        }
    }
}
