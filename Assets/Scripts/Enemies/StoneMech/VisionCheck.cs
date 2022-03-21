using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionCheck : MonoBehaviour
{

    private PolygonCollider2D vision;

    private EnemyFloatMovement enemyFloatMovement;



    // Start is called before the first frame update
    void Start()
    {
        vision = GetComponent<PolygonCollider2D>();
        enemyFloatMovement = GetComponentInParent<EnemyFloatMovement>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log(col.gameObject.layer);
        if (col.gameObject.layer == 9)
        {
            //Debug.Log(enemyFloatMovement.alerted);
            enemyFloatMovement.alerted = true;
        }

    }
}
