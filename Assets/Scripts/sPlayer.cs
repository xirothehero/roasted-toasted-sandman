using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class sPlayer : MonoBehaviour
{
    public float health = 100;
    public float speed = 8;
    public int sand = 0;
    public Text sandText;
    public Transform theCamera;

    //Temp vars like in sEnemy
    public float attackCooldownTime = 0.15f;
    public sWeapon weapon;
    //Quaternion orgWeaponRot;

    //public List<sItempickup> itemsCollected = new List<sItempickup>();
    public List<string> itemsCollected = new List<string>();

    // Temporary variable until actual damage indication is implemented
    public float damageIndicatorTime = 0.5f;

    public Transform attackPoint;
    public float attackRange = 2f;
    public LayerMask enemyLayers;

    public bool allowGoingBackward = false;

    public Slider healthSlider;

    // Jumping
    public float jumpForce = 2;
    bool isGrounded = true;


    [HideInInspector] public bool allowInput = true; 

    bool atkCooldown = false;
    Animator playerAnimator;
    Rigidbody rb;
    //float heading = 0;
    float keep = 1;
    private BoxCollider myCollider;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //myCollider = gameObject.GetComponent<BoxCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            Rotate(xInput, zInput);
        else
        {
            //Debug.Log(theCamera.localRotation.y);
            //transform.localRotation = Quaternion.Euler(0, theCamera.localRotation.y, 0);
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, theCamera.localEulerAngles.y, transform.localEulerAngles.z);
            //transform.rotation = Quaternion.LookRotation(theCamera.forward);
            //transform.Rotate(0,Input.GetAxis("Mouse X") * theCamera.GetComponent<sCameraFollow>().inputSensitivity/80,0);
        }
    }

    void FixedUpdate()
    {
        if (allowInput)
        {
            float zInput = Input.GetAxis("Vertical");
            float xInput = Input.GetAxis("Horizontal");

            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, theCamera.localEulerAngles.y, transform.localEulerAngles.z);
            //Debug.Log(transform.localEulerAngles.y);

            float actualSpeed = speed;
            if (zInput != 0 || xInput != 0)
            {
                // Uncomment this as well to rotate behind.
                if (allowGoingBackward)
                {
                    Rotate(xInput, zInput);
                    keep = zInput;
                    //transform.position += transform.forward * Time.deltaTime * speed;
                }
                //else
                //{
                //    if (zInput == 1 || zInput == -1)
                //        actualSpeed /= 1.5f;
                //    transform.position += (transform.forward + transform.right * xInput) * Time.deltaTime * actualSpeed;
                //}

                if (zInput == 1 || zInput == -1)
                    actualSpeed /= 1.5f;
                transform.position += (transform.forward * zInput + transform.right * xInput) * Time.deltaTime * actualSpeed;



            }

            if (!atkCooldown && Input.GetMouseButton(0))
            {
                //Rotate(xInput, zInput);
                Attack();
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && rb.velocity.y <= 0)
            {
                Debug.Log("Jumping");
                Jump();
            }
        }
    }


    public void EquipWeapon(sWeapon _wep)
    {
        weapon = _wep;
    }

    void Jump()
    {
        isGrounded = false;
        rb.AddForce(new Vector3(0, 2, 0) * jumpForce, ForceMode.Impulse);
        //rb.velocity += (Vector3.up * 3) * Physics.gravity.y * jumpForce * Time.deltaTime;
    }

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

        if (keep > 0)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, theCamera.localEulerAngles.y, transform.localEulerAngles.z);
            Debug.Log(theCamera.localEulerAngles.y);
            //Debug.Log(theCamera.eulerAngles.y);
        }
        else
        {
            //Debug.Log("Rotating towards camera.");
            Vector3 lookPos = theCamera.position - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = rotation;
        }
    }

    void Attack()
    {
        keep = 1;
        if (weapon.swordAnimator)
            weapon.swordAnimator.SetTrigger("SwingSword");
        //playerAnimator.SetTrigger("Attack");
        else
        {
            Debug.LogWarning("There is currently no animator attached to the playerAnimator, thus no annimation played.");
            if (weapon)
            {
                //weapon.RotateSword(true);
            }
            else
            {
                Debug.LogError("No weapon transform was attached to the player's weapon variable");
            }
        }
        //myCollider.enabled = true;
        Collider[] enemiesHit = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider enemy in enemiesHit)
        {
            Debug.Log("Enemy was Hit");
            sEnemy enemyScript = enemy.GetComponent<sEnemy>();
            if (enemyScript)
                enemyScript.TakeDamage(weapon.damage);
        }
        StartCoroutine(AttackCooldown());
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
    IEnumerator AttackCooldown()
    {
        atkCooldown = true;
        while (atkCooldown)
        {
            yield return new WaitForSeconds(attackCooldownTime);
            atkCooldown = false;
            //weapon.RotateSword(false);
        }
    }

    public void TakeDamage(float _dmg)
    {
        health -= _dmg;
        if (health > 0)
        {
            if (healthSlider)
                healthSlider.value = health;
            else
                Debug.LogError("No slider attached to the healthSlider variable on the player.");
            StartCoroutine(IndicateDamage());
        }
        else
        {
            Time.timeScale = 0;
            Gamemanager.instance.gameOverCanvas.SetActive(true);
        }

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
        //myCollider.enabled = false;
        gameObject.GetComponent<Renderer>().material.color = orgColor;
    }
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
