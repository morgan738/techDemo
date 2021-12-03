using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////PLAYER HEALTH////////////////////////////////////////////////
/*

Script that controls player health. Health damage calculations are done here

*/

public class PlayerHealthManager : MonoBehaviour
{
    public static int currentPlayerHealth;
    private int playerMaxHealth = 100;
    public static bool playerDead;
    Animator anim;
    new private Rigidbody2D rigidbody2D;
    [SerializeField] private float invulDuration;
    [SerializeField] private float invulDeltaTime;
    private bool isInvul = false;
    [HideInInspector] public bool playerHit = false;
    [HideInInspector] public int playerHitID;

    private PlayerMovement playerMovement;




    // Start is called before the first frame update
    void Start()
    {
        currentPlayerHealth = playerMaxHealth;
        anim = GetComponent<Animator>();
        rigidbody2D = transform.GetComponent<Rigidbody2D>();
        playerDead = false;
        playerMovement = GetComponent<PlayerMovement>();

    }

    // Update is called once per frame
    void Update()
    {
        //check to see if player is dead. if true, mark as dead and play death animation
        if (isDead() && gameObject != null)
        {
            playerDead = true;
            anim.SetBool("isDead", true);
            rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        }

        //if playerHit flag is set to true, player loses health
        //so far its a fixed -25 but will change to enemy damage values
        if (playerHit && !isInvul)
        {
            LoseHealth(25);
        }



    }

    //checks to see if player health is <= 0. if true, player is dead
    private bool isDead()
    {
        return currentPlayerHealth <= 0;
    }

    //invul frame coroutine
    private IEnumerator HitIFrame()
    {
        isInvul = true;


        for (float i = 0; i < invulDuration; i += invulDeltaTime)
        {


            /* if (isInvul)
            {
                hitVisual();
                //pushBackTween();

            } */
            hitVisual();

            yield return new WaitForSeconds(invulDeltaTime);
        }


        isInvul = false;
        anim.SetBool("isHurt", false);
        playerHit = false;
        //hitVisual(Vector3.one);
        Debug.Log("notInvul");
    }

    /* void TriggerIFrame()
    {
        if (!isInvul)
        {
            StartCoroutine(HitIFrame());
        }
    }
 */
    //subtracts damage from currenthealth then starts the invul coroutine
    public void LoseHealth(int damage)
    {
        if (isInvul)
        {
            return;
        }
        currentPlayerHealth -= damage;
        Debug.Log("damage taken");
        Debug.Log(currentPlayerHealth);


        StartCoroutine(HitIFrame());

    }

    //plays the hurt animation and intends to adds force for recoil
    //doesnt really work as intended, needs some work
    public void hitVisual()
    {
        anim.SetLayerWeight(1, 0f);
        anim.SetLayerWeight(2, 0f);
        anim.SetBool("isHurt", true);

    }



}
