using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{

    [HideInInspector] public bool aerialHit;
    public static DamageManager instance;

    // Start is called before the first frame update
    void Start()
    {
        aerialHit = false;
    }

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 10)
        {

            EnemyHealthManager enemyHealthManager = col.gameObject.GetComponent<EnemyHealthManager>();
            enemyHealthManager.isHit = true;
            aerialHit = true;
        }
    }
}
