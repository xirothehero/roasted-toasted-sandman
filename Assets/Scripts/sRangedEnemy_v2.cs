using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class sRangedEnemy_v2 : MonoBehaviour
{

    public AudioSource takeDamage, throwSpear;
    
    public float health = 100;
    public GameObject projectile;
    public GameObject itemPickupPrefab;
    public Transform attackPoint;
    public Transform weaponRotater;
    public float damageIndicatorTime = 0.5f;
    //public float decreaseAccuracyRate = 0.15f;
    //public float increaseAccuracyRate = 0.15f;
    public float startAttackingAtRange = 5f;
    public float rotationSpeed = 1.5f;
    public float attackCooldownTime = 0.15f;

    [Header("Sprites")]
    public Sprite attackSprite;
    public Sprite walkingSprite;
    public Sprite idleSprite;
    public SpriteRenderer damageIndicator;
    public SpriteRenderer mySpriteRender;
    public Animator enemyAnimator;

    //Vector3 relativeSamePos = new Vector3(0,0,0);
    //float accuracy;
    Transform thePlayer;
    NavMeshAgent myAgent;
    bool atkCooldown = false;

    private void Start()
    {
        myAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        if (thePlayer)
        {
            // Keep moving until we're close enough
            myAgent.destination = thePlayer.position;
            if (myAgent.remainingDistance > startAttackingAtRange)
            {
                enemyAnimator.SetBool("IsMoving", true);
                myAgent.isStopped = false;
                Debug.Log("Continuing to folow the player");
            }
            else
            {
                enemyAnimator.SetBool("IsMoving", false);
                myAgent.isStopped = true;
                Vector3 relativePos = thePlayer.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(relativePos, Vector3.up);
                // Rotate towards the player 
                    Debug.Log("Rotating towards player");
                transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed);
                // Rotate weapon rowards the player
                //else
                //{
                //    // Keep track of the players relative position
                //    if (Vector3.Distance(relativeSamePos, thePlayer.position) > 1)
                //    {
                //        accuracy -= decreaseAccuracyRate;
                //        relativeSamePos = thePlayer.position;
                //    }
                //    else
                //    {
                //        if (accuracy < 1)
                //            accuracy += increaseAccuracyRate;
                //    }

                //    if (accuracy < 1)
                //    {
                //        weaponRotater.localEulerAngles = Quaternion.RotateTowards(weaponRotater.rotation, 0, rotationSpeed);
                //        Debug.Log("Rotating weapon towards player");
                //    }
                //    else
                //    {
                //        Debug.Log("Having weapon look at the player");
                //        weaponRotater.localRotation = lookRotation;
                //    }
                //}
                if (!atkCooldown)
                {
                    Attack();
                    enemyAnimator.SetBool("IsAttacking", true);
                }
            }
        }
    }

    public void TakeDamage(float _dmg)
    {
        takeDamage.Play();
        health -= _dmg;
        if (health > 0)
        {
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

    IEnumerator AttackCooldown()
    {
        atkCooldown = true;
        while (atkCooldown)
        {
            yield return new WaitForSeconds(attackCooldownTime);
            atkCooldown = false;
        }
    }

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

    public void Attack()
    {
        // Fire attack here
        throwSpear.Play();
        GameObject pew = Instantiate(projectile, attackPoint.position, gameObject.transform.rotation);
        pew.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
        Destroy(pew, 2f);
        
        // If it exists
        if (attackPoint)
        {
            StartCoroutine("AttackCooldown");
        }
        //myCollider.enabled = true;
        enemyAnimator.SetBool("IsAttacking", false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            thePlayer = other.transform;
        }
    }

}
