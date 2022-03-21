using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////PLAYER MOVEMENT////////////////////////////////////////////////
/*

Script that controls player movement

*/

public class PlayerMovement : MonoBehaviour
{
    new private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private LayerMask layerMask;
    public static float moveSpeed = 10f;
    private bool facingRight = true;
    private float moveDirection;
    Animator anim;

    public float recoilDistance;
    public float recoilTime;
    public float recoilCoolDown;
    public bool recoilRight;

    private float slideCoolDown;
    private bool canSlide;
    private float slideInvul;




    private PlayerHealthManager playerHealthManager;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        slideCoolDown = 0f;
        canSlide = true;
    }

    void Awake()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        playerHealthManager = GetComponent<PlayerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Gets input (left, right; a or d. becuase of Horizontal key word)
        moveDirection = Input.GetAxis("Horizontal");




        //checks to see if character is on the gorund. if they are, they have the ability to jump
        //Input.GetKeyDown(KeyCode.Space)
        if ((isGrounded() && Input.GetKeyDown(KeyCode.W)) || (isGrounded() && Input.GetKeyDown(KeyCode.UpArrow)))
        {
            float jumpVelocity = 18f;
            rigidbody2D.velocity = Vector2.up * jumpVelocity;
            //anim.SetBool("isRunning", false);
            //anim.SetBool("isJumping", true);
            //Debug.Log(isGrounded());
            //anim.SetTrigger("attack");

        }
        /* else if (isGrounded())
        {
            anim.SetBool("isJumping", false);
        } */
        if (!isGrounded())
        {
            anim.SetBool("isJumping", true);
        }
        else if (isGrounded())
        {
            anim.SetBool("isJumping", false);
        }

        //Enables crouching if player is grounded
        if ((isGrounded() && Input.GetKey(KeyCode.S)) || (isGrounded() && Input.GetKey(KeyCode.DownArrow)))
        {
            anim.SetLayerWeight(1, 0f);
            anim.SetBool("isCrouching", true);
            anim.SetBool("isRunning", false);
            rigidbody2D.velocity = Vector2.zero;

        }

        if ((isGrounded() && Input.GetKeyUp(KeyCode.S)) || (isGrounded() && Input.GetKeyUp(KeyCode.DownArrow)))
        {
            anim.SetBool("isCrouching", false);


        }

        if (anim.GetBool("isCrouching") == true && Input.GetKeyDown("space"))
        {
            CombatManager.instance.canReceiveInput = false;

            if (canSlide)
            {

                slideCoolDown = .4f;

                anim.SetBool("isSliding", true);
            }

            //transform.position += transform.forward * Time.deltaTime * moveSpeed;

        }
        if (slideCoolDown > 0)
        {

            slideCoolDown -= Time.deltaTime;
        }
        else if (slideCoolDown <= 0)
        {
            canSlide = true;
        }

        if (slideInvul > 0)
        {
            Debug.Log("SLIDEINVUL" + slideInvul);
            Physics2D.IgnoreLayerCollision(9, 10, true);
            Physics2D.IgnoreLayerCollision(9, 11, true);
            slideInvul -= Time.deltaTime;
        }
        else if (slideInvul <= 0)
        {
            Physics2D.IgnoreLayerCollision(9, 10, false);
            Physics2D.IgnoreLayerCollision(9, 11, false);

        }


        if (Input.GetKeyUp("space"))
        {
            anim.SetBool("isSliding", false);
            //anim.SetBool("isCrouching", false);
            Physics2D.IgnoreLayerCollision(9, 10, false);
            Physics2D.IgnoreLayerCollision(9, 11, false);
            CombatManager.instance.canReceiveInput = true;

        }





        //flips the character to face the direction they are moving
        if (moveDirection > 0 && !facingRight)
        {
            flipCharacter();
            //anim.SetBool("isRunning", true);
        }
        else if (moveDirection < 0 && facingRight)
        {
            flipCharacter();
            //anim.SetBool("isRunning", true);
        }

        //moves the character based on speed and velocity
        if (anim.GetBool("isRunning") == true && recoilCoolDown <= 0 && anim.GetBool("isHurt") == false)
        {

            rigidbody2D.velocity = new Vector2(moveDirection * moveSpeed, rigidbody2D.velocity.y);
        }
        else if (recoilCoolDown > 0)
        {
            if (!recoilRight)
            {
                rigidbody2D.velocity = new Vector2(-recoilDistance, recoilDistance);
            }
            if (recoilRight)
            {
                rigidbody2D.velocity = new Vector2(recoilDistance, recoilDistance);
            }
            recoilCoolDown -= Time.deltaTime;
        }

        if (!isGrounded())
        {
            moveSpeed = 0f;
            Debug.Log(moveSpeed);
        }
        if (moveDirection != 0)
        {
            anim.SetBool("isRunning", true);
            if ((isGrounded() && Input.GetKey(KeyCode.S)) || (isGrounded() && Input.GetKey(KeyCode.DownArrow)))
            {
                anim.SetLayerWeight(1, 0f);
                anim.SetBool("isCrouching", true);
                anim.SetBool("isRunning", false);
                rigidbody2D.velocity = Vector2.zero;

            }
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

    }

    void FixedUpdate()
    {

        if (anim.GetBool("isSliding") == true && isGrounded())
        {

            float slideVelocity = 10f;

            if (facingRight)
            {
                anim.SetBool("isCrouching", false);
                rigidbody2D.velocity = Vector2.right * slideVelocity;
                Physics2D.IgnoreLayerCollision(9, 10, true);
                Physics2D.IgnoreLayerCollision(9, 11, true);

            }
            else if (!facingRight)
            {
                anim.SetBool("isCrouching", false);
                rigidbody2D.velocity = Vector2.left * slideVelocity;
                Physics2D.IgnoreLayerCollision(9, 10, true);
                Physics2D.IgnoreLayerCollision(9, 11, true);


            }
            //slideCoolDown -= Time.deltaTime;



            //This method can teleport 
            //rigidbody2D.MovePosition(transform.forward * moveSpeed);

        }


    }

    //return true if player is touching the ground
    public bool isGrounded()
    {


        RaycastHit2D raycastHit2D = Physics2D.BoxCast(boxCollider2D.bounds.center, boxCollider2D.bounds.size, 0f, Vector2.down, .1f, layerMask);



        return raycastHit2D.collider != null;

    }

    //method to turn the character or flip the sprite
    private void flipCharacter()
    {
        if (!PlayerHealthManager.playerDead)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public void FallLoop()
    {
        if (rigidbody2D.velocity.y < 0)
        {
            anim.SetBool("isFalling", true);
        }
    }

    public void Landed()
    {
        if (isGrounded())
        {
            anim.SetBool("isFalling", false);
        }
    }


    public void slidePauseEnd()
    {
        Debug.Log("outside");
        if (canSlide)
        {
            Debug.Log("inside");
            canSlide = false;
            anim.Rebind();

        }


    }

    public void slidePause()
    {
        Debug.Log("slide");
        canSlide = false;
        slideInvul = .4f;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.layer == 10)
        {
            //Debug.Log("hit enemy");
            playerHealthManager.LoseHealth(25);
            recoilCoolDown = recoilTime;
            if (col.transform.position.x < transform.position.x)
            {
                recoilRight = true;

            }
            else if (col.transform.position.x > transform.position.x)
            {
                recoilRight = false;
            }

        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11)
        {
            Debug.Log("TOUCHING");
            playerHealthManager.LoseHealth(25);
            recoilCoolDown = recoilTime;
            if (col.transform.position.x < transform.position.x)
            {
                recoilRight = true;

            }
            else if (col.transform.position.x > transform.position.x)
            {
                recoilRight = false;
            }
        }
    }

}
