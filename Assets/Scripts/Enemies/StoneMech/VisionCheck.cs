using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCheck : MonoBehaviour
{

    private PolygonCollider2D vision;

    private EnemyFloatMovement enemyFloatMovement;

    private float alertCooldown;
    private bool seen;



    // Start is called before the first frame update
    void Start()
    {
        vision = GetComponent<PolygonCollider2D>();
        enemyFloatMovement = GetComponentInParent<EnemyFloatMovement>();
        alertCooldown = 5f;
        seen = false;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Alerted: " + enemyFloatMovement.alerted);
        Debug.Log("Seen " + seen);
        Debug.Log(alertCooldown);
        if(!seen && enemyFloatMovement.alerted){
            alertCooldown -= Time.deltaTime;
        }
        if(alertCooldown <= 0){
            enemyFloatMovement.alerted = false;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.layer);
        if (col.gameObject.layer == 9)
        {
            //Debug.Log(enemyFloatMovement.alerted);
            enemyFloatMovement.alerted = true;
            seen = true;
            alertCooldown = 5f;
        }

    }

    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.layer == 9){
            seen = false;
           /*  if(!seen && enemyFloatMovement.alerted){
                alertCooldown -= Time.deltaTime;
            } */
            
            /* if(alertCooldown <= 0){
                enemyFloatMovement.alerted = false;
            } */
        }

    }
}
