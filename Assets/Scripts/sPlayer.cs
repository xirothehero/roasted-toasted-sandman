using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class sPlayer : MonoBehaviour
{
    // Health
    // Movement
    // Audio
    // Attack
    public float health = 100;
    public float speed = 8;
    public int sand = 0;
    //public Text sandText;
    public Transform theCamera;

    // Audio things
    public AudioSource walk, takeDamage, swingSword;

    //Temp vars like in sEnemy
    public float attackCooldownTime = 0.15f;
    public sWeapon weapon;

    public HitFX hitFX;
    //Quaternion orgWeaponRot;

    //public List<sItempickup> itemsCollected = new List<sItempickup>();
    public List<string> itemsCollected = new List<string>();

    // Temporary variable until actual damage indication is implemented
    public float damageIndicatorTime = 0.5f;

    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask enemyLayers;

    public bool allowGoingBackward = false;

    public float rotationSpeed = 2f;

    public Slider healthSlider;

    // Jumping
    public float jumpForce = 2;
    bool isGrounded = true;


    [HideInInspector] public bool allowInput = true; 

    bool atkCooldown = false;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerRenderer;
    Rigidbody rb;
    //float heading = 0;
    float keep = 1;
    private BoxCollider myCollider;

    private bool isMoving;
    private bool isLookingBack;
    private bool isLookingRight;
    private bool isAttacking;

    private bool walkCooldown;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isMoving = false;
        isLookingBack = false;
        isLookingRight = false;
        isAttacking = false;

        if (!playerRenderer)
        {
            Debug.LogError("There isn't a Sprite Renderer attached to the player");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }


    private void Update()
    {
        float zInput = Input.GetAxis("Vertical");
        float xInput = Input.GetAxis("Horizontal");
        if (allowGoingBackward)
        {
            //if (isLookingBack)
                Rotate(xInput, zInput);
            if (zInput < 0) isLookingBack = true;
            else if (zInput > 0) isLookingBack = false;
            // else if zInput is 0, we just keep isLookingBack as is
        }
        else
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, theCamera.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }

    void FixedUpdate()
    {
        if (allowInput)
        {
            float zInput = Input.GetAxis("Vertical");
            float xInput = Input.GetAxis("Horizontal");

            isMoving = false;
            float actualSpeed = speed;
            if (zInput != 0 || xInput != 0)
            {
                // Uncomment this as well to rotate behind.
                if (allowGoingBackward)
                {
                    //if (isLookingBack)
                    //{
                        Rotate(xInput, zInput);
                        keep = zInput;
                    //}
                    //transform.position += transform.forward * Time.deltaTime * speed;


                    if (zInput < 0) isLookingBack = true;
                    else if (zInput >= 0) isLookingBack = false;
                    // else if zInput is 0, we just keep isLookingBack as is //haha jk
                }
                //else
                //{
                //    if (zInput == 1 || zInput == -1)
                //        actualSpeed /= 1.5f;
                //    transform.position += (transform.forward + transform.right * xInput) * Time.deltaTime * actualSpeed;
                //}

                if (zInput != 0 && xInput != 0)
                    actualSpeed /= 1.5f;
                if (keep < 0)
                {
                    zInput = 1;
                    if (zInput != 0)
                        xInput = -xInput;
                }
                isMoving = (actualSpeed != 0);
                if (!walkCooldown)
                {
                    walk.Play();
                    StartCoroutine("WalkCooldown");
                }

                transform.position += (transform.forward * zInput + transform.right * xInput) * Time.deltaTime * actualSpeed;

                if (xInput < 0) isLookingRight = false;
                else if (xInput > 0) isLookingRight = true;
                // else if xInput is 0, we just keep isLookingRight as is

            }

            if (!atkCooldown && Input.GetMouseButton(0))
            {
                //Rotate(xInput, zInput);
                swingSword.Play();
                Attack();
                isAttacking = true;
            }

            //if (Input.GetKeyDown(KeyCode.Space) && isGrounded && rb.velocity.y <= 0)
            //{
            //    Debug.Log("Jumping");
            //    //Jump();
            //}
        }

        if (playerRenderer)
        {
            if (isLookingBack)
                playerRenderer.flipX = isLookingRight;
            else
                playerRenderer.flipX = !isLookingRight;
            playerAnimator.SetBool("IsMoving", isMoving);
            playerAnimator.SetBool("IsFacingBack", isLookingBack);
            playerAnimator.SetBool("IsAttacking", isAttacking);
        }

    }


    public void EquipWeapon(sWeapon _wep)
    {
        weapon = _wep;
    }

    //void Jump()
    //{
    //    isGrounded = false;
    //    rb.AddForce(new Vector3(0, 2, 0) * jumpForce, ForceMode.Impulse);
    //    //rb.velocity += (Vector3.up * 3) * Physics.gravity.y * jumpForce * Time.deltaTime;
    //}

    void Rotate(float _xInput, float _zInput)
    {
        // Forward direction: camPos -> playerPos\\
        Vector3 cameraVector = new Vector3(transform.position.x - theCamera.position.x, 0.0f,
                                                   transform.position.z - theCamera.position.z);

        // Calculate the look direction of the player based on the input and the cameraVector
        //Vector3 playerLookDirection = Quaternion.LookRotation(cameraVector) * new Vector3(_xInput, 0.0f, _zInput);

        //if (playerLookDirection != Vector3.zero)
        //{
        //    Quaternion destRot = Quaternion.LookRotation(playerLookDirection);
        //    transform.rotation = destRot;
        //}
        //else
        //{
        //    if (keep > 0)
        //    {
        //        transform.rotation = Quaternion.Euler(0,theCamera.rotation.y, 0);
        //        //Debug.Log(theCamera.eulerAngles.y);
        //    }
        //    else
        //    {
        //        //Debug.Log("Rotating towards camera.");
        //        Vector3 lookPos = theCamera.position - transform.position;
        //        lookPos.y = 0;
        //        Quaternion rotation = Quaternion.LookRotation(lookPos);
        //        transform.rotation = rotation;
        //    }
        //}

        // if (keep >= 0)
        if (!isLookingBack)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, theCamera.localEulerAngles.y, transform.localEulerAngles.z);
            //Debug.Log(theCamera.eulerAngles.y);
        }
        else
        {
            //Debug.Log("Rotating towards camera.");
            Vector3 lookPos = theCamera.GetChild(0).position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
            // transform.LookAt(theCamera.GetChild(0));
        }
    }

    void Attack()
    {
        keep = 1;
//         if (weapon.swordAnimator)
//             weapon.swordAnimator.SetTrigger("SwingSword");
//         //playerAnimator.SetTrigger("Attack");
//         else
//         {
//             Debug.LogWarning("There is currently no animator attached to the playerAnimator, thus no annimation played.");
//             if (weapon)
//             {
//                 //weapon.RotateSword(true);
//             }
//             else
//             {
//                 Debug.LogError("No weapon transform was attached to the player's weapon variable");
//             }
//         }
        //myCollider.enabled = true;

        hitFX.PlayHitFX();

        Collider[] enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in enemiesHit)
        {
            Debug.Log("Enemy was Hit");
            sEnemy enemyScript = enemy.GetComponent<sEnemy>();
            sRangedEnemy_v2 otherEnemy = enemy.GetComponent<sRangedEnemy_v2>();
            if (enemyScript)
                enemyScript.TakeDamage(weapon.damage);
            else if (otherEnemy)
                otherEnemy.TakeDamage(weapon.damage);
        }
        StartCoroutine(AttackCooldown());
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        //Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
    IEnumerator AttackCooldown()
    {
        atkCooldown = true;
        while (atkCooldown)
        {
            yield return new WaitForSeconds(attackCooldownTime);
            atkCooldown = false;
            isAttacking = false;
            //weapon.RotateSword(false);
        }
    }

    private IEnumerator WalkCooldown()
    {
        walkCooldown = true;
        yield return new WaitForSeconds(walk.clip.length);
        walkCooldown = false;
    }

    public void TakeDamage(float _dmg)
    {
        takeDamage.Play();
        health -= _dmg;
        if (health > 0)
        {
            if (healthSlider)
                healthSlider.value = health;
            else
                Debug.LogError("No slider attached to the healthSlider variable on the player.");
            //StartCoroutine(IndicateDamage());
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
            Gamemanager.instance.gameOverCanvas.SetActive(true);
        }

    }

    // Temporary to show damage
    //IEnumerator IndicateDamage()
    //{
    //    Color orgColor = gameObject.GetComponent<Renderer>().material.color;
    //    gameObject.GetComponent<Renderer>().material.color = Color.red;
    //    bool wait = true;
    //    while (wait)
    //    {
    //        yield return new WaitForSeconds(damageIndicatorTime);
    //        wait = false;
    //    }
    //    //myCollider.enabled = false;
    //    gameObject.GetComponent<Renderer>().material.color = orgColor;
    //}
}   

//    private void OnTriggerEnter(Collider other)
//    {
//        //if (other.gameObject.CompareTag("Enemy"))
//        //{
//        //    if (other.gameObject.GetComponent<sEnemy>())
//        //        other.gameObject.GetComponent<sEnemy>().TakeDamage(weapon.damage);
//        //}
//    }
//}
