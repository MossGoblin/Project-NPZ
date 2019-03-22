using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] SpawnMaster spawnMaster;

    [SerializeField] public float timeLeft;
    [SerializeField] public float lifePonits;

    public int spawnPointIndex;
    public int selfType;

    // Start is called before the first frame update
    void Start()
    {
        // set time left    
        timeLeft = 60f;

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

    public void ReceiveDamage(float damage)
    {
        // TODO : ENEMY : Take Dagame
        Destroy(gameObject);
    }
}
