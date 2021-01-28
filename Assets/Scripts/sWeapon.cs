using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sWeapon : MonoBehaviour
{
    public float damage;
    public Animator swordAnimator;

    //public float angleOfWeapon = 15;
    //[HideInInspector] public sPlayer thePlayer;
    //[HideInInspector] public string enemyTag;
    //private Quaternion orgRot;

    //private void Start()
    //{
    //    orgRot = gameObject.transform.parent.transform.rotation;
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag(enemyTag))
    //    {
    //        if (!thePlayer)
    //            other.gameObject.GetComponent<sEnemy>().TakeDamage(damage);
    //        else
    //            thePlayer.TakeDamage(damage);
    //    }
    //}

    //public void RotateSword(bool _rotate)
    //{
    //    if (_rotate)
    //        gameObject.transform.parent.transform.Rotate(0, 0, angleOfWeapon, Space.Self);
    //    else
    //        gameObject.transform.parent.transform.rotation = orgRot;
    //}
}
