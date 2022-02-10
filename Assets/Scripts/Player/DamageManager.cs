using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{

    [HideInInspector] public bool aerialHit;
    public static DamageManager instance;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        aerialHit = false;
        anim = GetComponentInParent<Animator>();
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
            EnemyAIMovement enemyAIMovement = col.gameObject.GetComponent<EnemyAIMovement>();

            if (anim.GetCurrentAnimatorStateInfo(1).IsName("Attack3"))
            {
                Rigidbody2D rigidbody2D = col.gameObject.GetComponent<Rigidbody2D>();
                if (!enemyAIMovement.facingRight)
                {
                    rigidbody2D.AddForce(new Vector3(15, 2, 0), ForceMode2D.Impulse);
                }
                if (enemyAIMovement.facingRight)
                {
                    rigidbody2D.AddForce(new Vector3(-15, 2, 0), ForceMode2D.Impulse);
                }
            }

            enemyHealthManager.isHit = true;
            aerialHit = true;
        }
    }

}
