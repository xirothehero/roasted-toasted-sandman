using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sEnemy : MonoBehaviour
{
    public float health = 100;
    public float attackCooldownTime = 0.15f;

    public sWeapon myWeapon;

    public float rotationSpeed = 3f;

    public float distanceToAttack = 2.5f;

    // Temp var until actual damage indication is impemented
    public float damageIndicatorTime = 0.5f;

    public float knockBackforce = 3;
    public GameObject itemPickupPrefab;

    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask playerLayer;

    //private BoxCollider myCollider;
   
    Transform thePlayer;
    bool atkCooldown = false;
    Quaternion orgWeaponRot;
    Animator enemyAnimator;
    NavMeshAgent myAgent;
    Rigidbody rb;

    void Start()
    {
        //thePlayer = Gamemanager.instance.thePlayer.transform;
        myAgent = gameObject.GetComponent<NavMeshAgent>();
        rb = gameObject.GetComponent<Rigidbody>();
        enemyAnimator = gameObject.GetComponent<Animator>();
        //myCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thePlayer != null)
        {
            //transform.LookAt(thePlayer.transform.position);
            Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(thePlayer.position), attackRange);
            myAgent.destination = thePlayer.position;
            if (myAgent.remainingDistance <= distanceToAttack)
            {
                myAgent.isStopped = true;
                if (!atkCooldown)
                    Attack();
            }
            else
            {
                myAgent.isStopped = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            thePlayer = other.transform;
        }
    }

    public virtual void Attack()
    {
        // If it exists

        if (myWeapon.swordAnimator)
            myWeapon.swordAnimator.SetTrigger("SwingSword");
        //if (enemyAnimator)
        //    enemyAnimator.SetTrigger("Attack");
        if (myWeapon != null)
        {
            StartCoroutine(AttackCooldown());
        }

        Collider[] playerHit = Physics.OverlapSphere(attackPoint.position, attackRange, playerLayer);

        if (playerHit.Length > 0)
        {
            if (playerHit[0].GetComponent<sPlayer>())
            {
                playerHit[0].GetComponent<sPlayer>().TakeDamage(myWeapon.damage);
            }
        }
        //myCollider.enabled = true;

    }

    IEnumerator AttackCooldown()
    {
        atkCooldown = true;
        while (atkCooldown)
        {
            yield return new WaitForSeconds(attackCooldownTime);
            atkCooldown = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
    public void TakeDamage(float _dmg)
    {
        health -= _dmg;
        if (health > 0)
        {
            KnockBack();
            StartCoroutine(IndicateDamage());
        }
        else
        {
            if (itemPickupPrefab)
            {
                GameObject item = Instantiate(itemPickupPrefab);
                item.transform.position = transform.position;
            }
            else
            {
                Debug.Log("Do note that you have no itemPickupPrefab on this NPC.");
            }
            Destroy(gameObject);
        }
        
    }

    void KnockBack()
    {
        rb.AddForce(new Vector3(-transform.forward.x,transform.position.y*3,-transform.forward.z) * knockBackforce, ForceMode.Impulse);
    }
    
    // Temporary to show damage
    IEnumerator IndicateDamage()
    {
        Color orgColor = gameObject.GetComponent<Renderer>().material.color;
        gameObject.GetComponent<Renderer>().material.color = Color.red;
        bool wait = true;
        while (wait)
        {
            yield return new WaitForSeconds(damageIndicatorTime);
            wait = false;
        }
        gameObject.GetComponent<Renderer>().material.color = orgColor;
    }
}
