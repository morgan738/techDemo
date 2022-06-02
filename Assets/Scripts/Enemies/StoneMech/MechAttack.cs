using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechAttack : MonoBehaviour
{

    new private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    Animator anim;
    public LayerMask playerLayer;
    [HideInInspector] public PlayerHealthManager playerHealth;
    private EnemyFloatMovement enemyFloatMovement;
    private EnemyHealthManager enemyHealth;
    private Vector3 enemyDir;
    public GameObject laser;
    float rangeAttackTime = 2f;
    private GameObject player;
    private bool crRunning;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        enemyFloatMovement = GetComponent<EnemyFloatMovement>();
        enemyHealth = GetComponent<EnemyHealthManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        crRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyFloatMovement.alerted && !enemyHealth.isHit){
            
            StartCoroutine(LaserAttack());
        }
        if(!crRunning || enemyHealth.isHit){
            anim.SetLayerWeight(1, 0f);
            anim.SetLayerWeight(2, 0f);
        }
    }




    private IEnumerator LaserAttack(){

        crRunning = true;
        
        anim.Play("rangedAttack");
        anim.SetLayerWeight(2, 0f);
        anim.SetLayerWeight(1, 1f);

        yield return new WaitForSeconds(rangeAttackTime);
        crRunning = false;

        /* anim.SetLayerWeight(2, 0f);
        anim.SetLayerWeight(1, 0f); */
    }

    private void spawnLaser(){
        Vector3 currentPosition = this.transform.position;
        
        if(!GameObject.FindGameObjectWithTag("MechLaser")){
            var laserClone = Instantiate(laser, currentPosition, Quaternion.identity);
            Destroy(laserClone, rangeAttackTime);
        }
    }
}
