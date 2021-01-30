using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sProjectile : MonoBehaviour
{
    public Rigidbody rb;
    private Animator enemyAnimator;
    public float damage;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        enemyAnimator = gameObject.GetComponent<Animator>();
        //myCollider = gameObject.GetComponent<BoxCollider>();
    }

    /*private void Update()
    {
        print(transform.eulerAngles);
        float moveSpeed = 10f;
        rb.velocity = new Vector3 (0, 0, 0);
    }*/

    // Update is called once per frame
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<sPlayer>())
        {
            other.gameObject.GetComponent<sPlayer>().TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
