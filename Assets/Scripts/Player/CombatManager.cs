using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////PLAYER COMBAT MANAGER////////////////////////////////////////////////
/*

Script that controls player combat

*/

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    Animator anim;
    private float attackTime = 5;
    private bool isAttacking = false;
    int attackID;


    [HideInInspector] public bool canReceiveInput = true;
    [HideInInspector] public bool inputReceived = false;
    public GameObject attackArea;
    private BoxCollider2D attackCollider;
    public LayerMask enemy;
    private EnemyHealthManager enemyHealth;

    public static int swordDamage = 25;

    private PlayerMovement playerMovement;
    private Rigidbody2D rigidbody2D;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attackCollider = attackArea.GetComponent<BoxCollider2D>();
        attackCollider.enabled = false;
        playerMovement = GetComponent<PlayerMovement>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        //LayerMask enemy = LayerMask.GetMask("Enemy");

    }

    // Update is called once per frame
    void Update()
    {
        //constantly check if user is trying to attack
        if (anim.GetBool("isCrouching") == false && playerMovement.isGrounded())
        {

            Attack();
        }
        if (anim.GetBool("isCrouching") == false && !playerMovement.isGrounded())
        {

            AirAttack();
        }


        //if player is attacking enable collider, play animation
        if (isAttacking)
        {
            attackCollider.enabled = true;

            PlayerMovement.moveSpeed = .5f;
            if (anim.GetCurrentAnimatorStateInfo(1).IsName("CombatRun"))
            {
                anim.GetCurrentAnimatorStateInfo(1).IsName("CombatRun");
                PlayerMovement.moveSpeed = 10f;
            }

        }


        //collision check for attacks hitting anything in the enemy layer
        //if array length is > 0 enemy has been hit
        Bounds attackBounds = attackCollider.bounds;
        Vector2 center = new Vector2(attackBounds.center.x, attackBounds.center.y);
        Collider2D[] damage = Physics2D.OverlapBoxAll(attackBounds.center, attackBounds.size, 0f, enemy);

        //loops through array and set enemyhit flag to be true, damage calculation handled elsewhere
        for (int i = 0; i < damage.Length; i++)
        {

            //Destroy(damage[i].gameObject);
            //damage[i].EnemyHealthManager.isHit = true;
            //EnemyHealthManager.currentHealth -= swordDamage;

            enemyHealth = damage[i].gameObject.GetComponent<EnemyHealthManager>();
            if (enemyHealth.hitID != attackID)
            {
                enemyHealth.isHit = true;
                enemyHealth.hitID = attackID;
            }




        }

        //allows for transition between combat idle and regular idle
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;

        }
        else if (attackTime <= 0)
        {
            isAttacking = false;

        }
        if (!isAttacking)
        {
            attackCollider.enabled = false;
            PlayerMovement.moveSpeed = 10f;
            anim.SetLayerWeight(1, 0f);

        }

        if (!anim.GetBool("isAttacking"))
        {
            rigidbody2D.drag = 0;
            rigidbody2D.gravityScale = 5;
        }



    }


    //attack method to determine if the player is able to attack and plays animation if they can
    public void Attack()
    {

        if (Input.GetButton("Fire1"))
        {
            if (canReceiveInput)
            {

                anim.SetLayerWeight(1, 1f);
                anim.SetLayerWeight(2, 0f);
                isAttacking = true;
                attackTime = 5;
                attackID = Random.Range(0, int.MaxValue);



                inputReceived = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }

    public void AirAttack()
    {

        if (Input.GetButton("Fire1"))
        {
            if (canReceiveInput)
            {
                rigidbody2D.drag = 100;
                anim.SetLayerWeight(2, 1f);
                anim.SetLayerWeight(1, 0f);
                anim.SetBool("isAttacking", true);
                isAttacking = true;
                attackTime = 5;
                attackID = Random.Range(0, int.MaxValue);



                inputReceived = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }

    /*     public void AirAttackLoop()
        {
            if (!playerMovement.isGrounded())
            {
                anim.Play("AirAttack3", 0, .5f);
                Debug.Log("loop");
            }

        } */

    public void AirAttackLoop()
    {
        if (!playerMovement.isGrounded())
        {
            anim.SetBool("plunging", true);
            rigidbody2D.drag = 0;
            rigidbody2D.gravityScale = 50;
            Debug.Log(playerMovement.isGrounded());

        }

    }

    public void GroundReached()
    {
        if (playerMovement.isGrounded())
        {
            anim.SetBool("plunging", false);
            Debug.Log("grounded" + playerMovement.isGrounded());
        }
    }
    //allows for combo transitions
    public void InputManager()
    {
        if (!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }

}
