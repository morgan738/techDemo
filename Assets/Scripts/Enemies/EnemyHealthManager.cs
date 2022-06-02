using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////ENEMY HEALTH////////////////////////////////////////////////
/*

Script that controls enemy health. This is where damage calculations take place as well as invul frames and animation

*/

public class EnemyHealthManager : MonoBehaviour
{
    //declare private variables visible in editor
    [SerializeField] private float invulDuration;
    [SerializeField] private float invulDeltaTime;

    //declare other private+public variables
    public int maxHealth;
    private int currentHealth;
    public bool isHit = false;
    private bool isInvul = false;
    private BoxCollider2D hitBox;
    Animator anim;
    public int hitID;
    new private Rigidbody2D rigidbody2D;
    private GameObject player;
    private Vector3 enemyDir;
    private EnemyAIMovement enemyAIMovement;
    private EnemyFloatMovement enemyFloatMovement;



    // Start is called before the first frame update
    void Start()
    {
        //instantiate variables and components
        currentHealth = maxHealth;
        hitBox = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        enemyAIMovement = GetComponent<EnemyAIMovement>();
        enemyFloatMovement = GetComponent<EnemyFloatMovement>();

    }





    // Update is called once per frame
    void Update()
    {
        //if player exists gets the direction of the enemy relative to the player
        if (player != null)
        {

            enemyDir = (player.transform.position - transform.position).normalized;

        }

        //if enemy has been hit and they are not invul, take damage
        if (isHit && !isInvul)
        {
            Debug.Log("isHit");


            LoseHealth(CombatManager.swordDamage);
        }



    }

    //iframe calcualtions take place here
    private IEnumerator HitIFrame()
    {
        isInvul = true;


        for (float i = 0; i < invulDuration; i += invulDeltaTime)
        {

            if (isInvul)
            {
                hitVisual();


            }

            yield return new WaitForSeconds(invulDeltaTime);
        }

        //after the for loop executes, resets the invulframes
        isInvul = false;
        anim.SetBool("isHurt", false);
        isHit = false;

        Debug.Log("notInvul");
    }

    //Method to trigger Coroutine to invoke invul state
    void TriggerIFrame()
    {
        if (!isInvul)
        {
            StartCoroutine(HitIFrame());
        }
    }

    //damage calculations done here
    public void LoseHealth(int damage)
    {
        if (isInvul)
        {
            return;
        }
        currentHealth -= damage;


        //checks if enemy still has health
        //if no health they are dead
        if (currentHealth <= 0)
        {
            anim.SetBool("isDead", true);

            return;
        }

        //triggers iframes once they lose health
        StartCoroutine(HitIFrame());

    }


    ////////////Once enemy is hit://////////////////
    //play hit animation
    //turn to face player if needed
    //applies force to the enemy so they are pushed back a bit
    private void hitVisual()
    {
        anim.SetLayerWeight(1, 0f);
        anim.SetLayerWeight(2, 0f);

        anim.SetBool("isHurt", true);
        //rigidbody2D.AddForce(transform.forward * 200,ForceMode2D.Impulse);
        //StopMovement();
        anim.SetBool("isWalking", false);
        facePlayer();
        //enemyAIMovement.patrolPause = 0;

        ///////////////////////KNOCKBACK///////////////////////
        /*  if (!enemyAIMovement.facingRight)
         {
             rigidbody2D.AddForce(new Vector3(5, 2, 0), ForceMode2D.Impulse);
         }
         if (enemyAIMovement.facingRight)
         {
             rigidbody2D.AddForce(new Vector3(-5, 2, 0), ForceMode2D.Impulse);
         } */
        ///////////////////////KNOCKBACK///////////////////////

    }


    //Method to attempt to have enemy face the player
    public void facePlayer()
    {


        var projection = Vector3.Dot(enemyDir, transform.right);
        if (projection < 0 && (enemyAIMovement.facingRight || enemyFloatMovement.facingRight))
        {
            Debug.Log("Left");
            enemyAIMovement.Flip();
        }
        if (projection > 0 && (!enemyAIMovement.facingRight || !enemyFloatMovement.facingRight))
        {
            Debug.Log("right");
            enemyAIMovement.Flip();
        }


    }





}
