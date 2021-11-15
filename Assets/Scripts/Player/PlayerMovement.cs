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
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Awake()
    {
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
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
            anim.SetBool("isRunning", false);
            anim.SetBool("isJumping", true);
            //Debug.Log(isGrounded());
            //anim.SetTrigger("attack");

        }
        else if (isGrounded())
        {
            anim.SetBool("isJumping", false);
        }

        //Enables crouching if player is grounded
        if ((isGrounded() && Input.GetKeyDown(KeyCode.S)) || (isGrounded() && Input.GetKeyDown(KeyCode.DownArrow)))
        {
            anim.SetBool("isCrouching", true);

        }

        if ((isGrounded() && Input.GetKeyUp(KeyCode.S)) || (isGrounded() && Input.GetKeyUp(KeyCode.DownArrow)))
        {
            anim.SetBool("isCrouching", false);
        }

        if (anim.GetBool("isCrouching") == true && Input.GetKeyDown("space"))
        {

            Debug.Log("slide");
            anim.SetBool("isSliding", true);
            //transform.position += transform.forward * Time.deltaTime * moveSpeed;

        }

        if (Input.GetKeyUp("space"))
        {
            anim.SetBool("isSliding", false);
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
        rigidbody2D.velocity = new Vector2(moveDirection * moveSpeed, rigidbody2D.velocity.y);
        if (!isGrounded())
        {
            moveSpeed = 0f;
            Debug.Log(moveSpeed);
        }
        if (moveDirection != 0)
        {
            anim.SetBool("isRunning", true);
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

            float slideVelocity = 8f;
            if (facingRight)
            {

                rigidbody2D.velocity = Vector2.right * slideVelocity;

            }
            else if (!facingRight)
            {
                rigidbody2D.velocity = Vector2.left * slideVelocity;

            }


            //This method can teleport 
            //rigidbody2D.MovePosition(transform.forward * moveSpeed);

        }
        //boxCollider2D.enabled = true;

    }

    //return true if player is touching the ground
    private bool isGrounded()
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
}
