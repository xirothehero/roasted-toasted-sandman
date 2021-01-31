using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFX : MonoBehaviour
{
    public GameObject ps;
    public void PlayHitFX() {
        ps.SetActive(false);
        ps.SetActive(true);
        
    }
}
