using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    // Prefab for shooting
    public Transform shotPrefab;

    // Time to cooldown in seconds
    public float shootingRate = 0.25f;

    //Cooldown
    private float shootCooldown;


    // Start is called before the first frame update
    void Start()
    {
        shootCooldown = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (shootCooldown > 0)
        {
            shootCooldown -= Time.deltaTime;
        }
    }

    // Shooting from another script

    // Create new shoot
    public void Attack(bool isEnemy)
    {
        if (CanAttack)
        {
            shootCooldown = shootingRate;

            // Create new shot
            var shotTransform = Instantiate(shotPrefab) as Transform;

            // Define position
            shotTransform.position = transform.position;

            // Enemy properties
            ShotScript shot = shotTransform.gameObject.GetComponent<ShotScript>();
            if (shot != null)
            {
                shot.isEnemyShot = isEnemy;
            }

            // Direction to enemy
            MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                move.direction = this.transform.right;
            }
        }
    }

    // Is it ready to attack
    public bool CanAttack
    {
        get
        {
            return shootCooldown <= 0f;
        }
    }


}
