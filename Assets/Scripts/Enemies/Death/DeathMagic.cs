using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///////////////////////////////////DEATH MAGIC ATTACKS////////////////////////////////////////////////
/*

Script that controls death-specific attacks. this script is specifically for the arm that spawns during ranged attack

*/

public class DeathMagic : MonoBehaviour
{
    private BoxCollider2D magicArea;

    private Animator anim;

    private float windUpTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        magicArea = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        //check to see if attack collider has hit anything in the player layer.
        //if the array has an element, player has been hit, playhit flag set to true. damage calculation handled elsewhere
        //This affects RANGED ATTACKS ONLY
        /*         Bounds rangeBounds = rangeCollider.bounds;
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
                } */
    }

    private void windUp()
    {
        magicArea.enabled = false;

        if (windUpTime > 0)
        {
            anim.SetBool("windUp", true);
        }

    }
}
