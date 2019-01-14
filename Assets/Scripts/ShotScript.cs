using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    // Damage
    public int damage = 1;

    // Time to live
    public float timeToLive = 15;

    // Damage to player
    public bool isEnemyShot = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
