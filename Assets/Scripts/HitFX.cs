using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFX : MonoBehaviour
{
    public GameObject ps;
    void PlayHitFX() {
        ps.SetActive(false);
        ps.SetActive(true);
        
    }
}
