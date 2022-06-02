using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloatMovement : MonoBehaviour
{

    //declare public variables that are hidden in editor
    [HideInInspector] public float patrolPause;
    [HideInInspector] public bool isPatrolling;
    [HideInInspector] public EnemyHealthManager enemyHealth;
    

    //declare public variables visible in editor + private variables
    public float walkSpeed;
    public LayerMask groundLayer;
    private bool mustFlip = false;
    public bool facingRight = false;
    new private Rigidbody2D rigidbody2D;
    public Transform groundCheckPos;
    private Animator anim;
    private Collider2D boxCollider2D;

    float distanceToPlayer;
    private GameObject player;
    private Vector3 enemyDir;

    [HideInInspector] public bool alerted;

    // Start is called before the first frame update
    void Start()
    {
        isPatrolling = true;
        patrolPause = 2f;
        anim = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        enemyHealth = GetComponent<EnemyHealthManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        alerted = false;



    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            enemyDir = (player.transform.position - transform.position).normalized;
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        }

        if (enemyHealth.isHit)
        {
            isPatrolling = false;
        }
        if (!enemyHealth.isHit)
        {
            isPatrolling = true;
        }

        if (isPatrolling)
        {
            Patrol();
        }

    }

    private void FixedUpdate()
    {
        /* if(isPatrolling){
            mustFlip = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        } */

        //logic for checking when the enemy turns, thus flipping the sprite
        if (isPatrolling && needsToTurn())
        {
            mustFlip = needsToTurn();
        }
        else if (isPatrolling)
        {
            mustFlip = !Physics2D.OverlapCircle(groundCheckPos.position, 0.1f, groundLayer);
        }
    }

    //Allows the enemy to patrol an area. if they reach a wall or end of a platform they pause and then turn
    void Patrol()
    {
        /* anim.SetLayerWeight(1, 0f);
        anim.SetLayerWeight(2, 0f); */

        if (alerted)
        {
            rigidbody2D.velocity = new Vector2(enemyDir.x * 4f, enemyDir.y * 4f);
        }
        if (!alerted)
        {

            rigidbody2D.velocity = new Vector2(walkSpeed * Time.fixedDeltaTime, rigidbody2D.velocity.y);
        }
        if (mustFlip)
        {
            if (patrolPause > 0)
            {
                //anim.SetBool("isWalking", false);
                patrolPause -= Time.deltaTime;
                return;
            }
            else if (patrolPause <= 0)
            {
                patrolPause = 2f;
            }
            Flip();
        }
        //anim.SetBool("isWalking", true);

    }

    //Allows enemy to turn
    public void Flip()
    {
        isPatrolling = false;

        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        facingRight = !facingRight;

        walkSpeed *= -1;
        //patrolPause = 5f;

        isPatrolling = true;
    }

    //if return true, enemy has hit a wall or end of platform, they need to turn
    private bool needsToTurn()
    {
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.left, .01f, groundLayer);
        RaycastHit2D raycastHit2DRight = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.right, .01f, groundLayer);


        return raycastHit2D.collider != null || raycastHit2DRight.collider != null;

    }
}
