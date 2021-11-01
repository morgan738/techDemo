using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////DEATH ATTACKS////////////////////////////////////////////////
/*

Script that controls death-specific attacks. includes melee and ranged moves

*/

public class DeathAttacks : MonoBehaviour
{
    new private Rigidbody2D rigidbody2D;
    public GameObject attackArea;
    private BoxCollider2D attackCollider;
    Animator anim;
    public bool deathAttacking;
    private bool crRunning;
    private GameObject player;
    private float minAttackDistance = 7f;
    private float rangedAttackDistance = 10f;
    float distanceToPlayer; 
    float attackTime = 5f;
    float rangeAttackTime = 2f;
    public static int deathMeleeDamage = 30;
    int deathMeleeID;
    public LayerMask playerLayer;
    [HideInInspector] public PlayerHealthManager playerHealth;
    private EnemyAIMovement enemyAIMovement;
    private EnemyHealthManager enemyHealth;
    private Vector3 enemyDir;
    public GameObject magicArm;
    [HideInInspector]public float attackDelay;


    // Start is called before the first frame update
    void Start()
    {
       anim = GetComponent<Animator>();
       attackCollider = attackArea.GetComponent<BoxCollider2D>();
       attackCollider.enabled = false;
       deathAttacking = false;
       crRunning = false;
       player = GameObject.FindGameObjectWithTag ("Player");
       enemyAIMovement = GetComponent<EnemyAIMovement>();
       enemyHealth = GetComponent<EnemyHealthManager>();

       //as attacks are being spammed if player is in range, want to implement some pause between attacks
       //not yet implemented though
       attackDelay = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        //if player exists get direction and distance of enemy relative to player
        if(player != null){
            enemyDir = (player.transform.position - transform.position).normalized;
            distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        }

        //////////////ATTACK CONDITION CHECKS////////////////////
        //first check melee condition. if player is within melee distance start melee coroutine
        //if the player is behind enemy has no LOS. will not attack. I want to change this eventually but for now its this
        if(distanceToPlayer < minAttackDistance && player != null){
            
            if(!isBehind() && !crRunning){


                StartCoroutine(MeleeAttack());

               
            }else if(isBehind() && !crRunning){
                anim.SetLayerWeight(1, 0f);
                anim.SetLayerWeight(2, 0f);
                deathAttacking = false;
            }

        //check ranged condition next. if player is within ranged distnace start ranged coroutine
        //if the player is behind enemy has no LOS. will not attack. I want to change this eventually but for now its this
        } else if(distanceToPlayer < rangedAttackDistance && distanceToPlayer > minAttackDistance && player != null){
            if(!isBehind() && !crRunning){
                StartCoroutine(RangeAttack());
                
            }else if (isBehind() && !crRunning){
                anim.SetLayerWeight(1, 0f);
                anim.SetLayerWeight(2, 0f);
                deathAttacking = false;
            }

        //if no condition are met, stop all attacks
        }else{
            anim.SetLayerWeight(1, 0f);
            anim.SetLayerWeight(2, 0f);
            deathAttacking = false;
        }
        
            
        //check to see if attack collider has hit anything in the player layer.
        //if the array has an element, player has been hit, playhit flag set to true. damage calculation handled elsewhere
        //This affects MELEE ATTACKS ONLY
        Bounds attackBounds = attackCollider.bounds;
        Vector2 center = new Vector2(attackBounds.center.x, attackBounds.center.y);
        Collider2D[] damage = Physics2D.OverlapBoxAll(attackBounds.center, attackBounds.size, 0f, playerLayer);
        for(int i = 0; i <damage.Length; i++){
            playerHealth = damage[i].gameObject.GetComponent<PlayerHealthManager>();
            if(playerHealth.playerHitID != deathMeleeID){
                playerHealth.playerHit = true;
                playerHealth.playerHitID = deathMeleeID;
            }
        }

        
 
        
    }

    //Melee coroutine method
    //simply starts melee animation
    private IEnumerator MeleeAttack(){

        deathAttacking = true;
        crRunning = true;
        deathMeleeID = Random.Range(0,int.MaxValue);
        anim.SetBool("isWalking", false);
        anim.SetLayerWeight(1, 0f);
        anim.SetLayerWeight(2, 1f);
        /* anim.SetBool("meleeAttack", true);
        anim.SetBool("isAttacking", true); */
        attackCollider.enabled = true;
        
        yield return new WaitForSeconds(attackTime);



        //PlayerHealthManager.currentPlayerHealth -=100;
        //deathAttacking = false;
        attackCollider.enabled = false;
        crRunning = false;
        //anim.SetBool("meleeAttack", false);
        //anim.SetLayerWeight(2, 0f);
        //anim.SetBool("isWalking", true);
        //anim.SetBool("isAttacking", false);

    }

    //Ranged coroutine method
    //gets the y value above player position, starts the ranged animation
    private IEnumerator RangeAttack(){
        deathAttacking = true;
        crRunning = true;
        deathMeleeID = Random.Range(0,int.MaxValue);
        Vector3 playerPos = player.transform.position;
        playerPos.y += 1.2f;
        anim.SetBool("isWalking", false);
        anim.SetLayerWeight(2, 0f);
        anim.SetLayerWeight(1, 1f);
        /* anim.SetBool("rangedAttack", true);
        anim.SetBool("isAttacking", true); */
        
        //spawns the magic arm if there isnt one and destroy it once attack finishes
        if(!GameObject.FindGameObjectWithTag("MagicArm")){

           var magicArmClone = Instantiate(magicArm, playerPos, Quaternion.identity);
           Destroy(magicArmClone, rangeAttackTime);
        }


        yield return new WaitForSeconds(rangeAttackTime);
        crRunning = false;
        //anim.SetBool("rangedAttack", false);
        //anim.SetLayerWeight(1, 0f);
        //anim.SetBool("isWalking", true);
        //anim.SetBool("isAttacking", false);
        
    }

    //Checks to see player is behind death. return true if player is behind, false otherwise
    private bool isBehind(){
        var projection = Vector3.Dot(enemyDir, transform.right);
        if(projection < 0 && enemyAIMovement.facingRight){
            Debug.Log("Left");
            return true;
            
        }
        if(projection > 0 && !enemyAIMovement.facingRight){
            Debug.Log("right");
            return true;
        }

        return false;
    }
}
