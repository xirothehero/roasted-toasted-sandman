using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sRangedEnemy : sEnemy
{

    public GameObject projectile;

    public override void Attack()
    {
        // Fire attack here
        GameObject pew = Instantiate(projectile, attackPoint.position, gameObject.transform.rotation);
        pew.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
        Destroy(pew, 2f);
        
        // If it exists
        if (attackPoint)
        {
            StartCoroutine("AttackCooldown");
        }
        //myCollider.enabled = true;

    }

    
}
