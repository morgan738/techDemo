using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////DEATH MAGIC ATTACKS////////////////////////////////////////////////
/*

Script that controls death-specific attacks. this script is specifically for the arm that spawns during ranged attack

*/

public class DeathMagic : MonoBehaviour
{
    public GameObject magicArea;
    private BoxCollider2D rangeCollider;
    public LayerMask playerLayer;
    [HideInInspector] public PlayerHealthManager playerHealth;

    // Start is called before the first frame update
    void Start()
    {
        rangeCollider = magicArea.GetComponent<BoxCollider2D>();
        //magicArea.GetComponent<BoxCollider2D>().enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        //check to see if attack collider has hit anything in the player layer.
        //if the array has an element, player has been hit, playhit flag set to true. damage calculation handled elsewhere
        //This affects RANGED ATTACKS ONLY
        Bounds rangeBounds = rangeCollider.bounds;
        Vector2 center = new Vector2(rangeBounds.center.x, rangeBounds.center.y);
        if(rangeCollider.size.y >= .4){
            rangeCollider.enabled = true;
        }else{
            rangeCollider.enabled = false;
        }
        Collider2D[] damage = Physics2D.OverlapBoxAll(rangeBounds.center, rangeBounds.size, 0f, playerLayer);
        for(int i = 0; i < damage.Length; i++){
            playerHealth = damage[i].gameObject.GetComponent<PlayerHealthManager>();
            playerHealth.playerHit = true;
        }
    }
}
