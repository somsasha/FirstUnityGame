using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // speed
    public Vector2 speed = new Vector2(50, 50);

    // movment direction
    private Vector2 movement;

    // Joystic for phones
    public Joystick joy;

    public GameObject restartDialog;

    // Start is called before the first frame update
    void Start()
    {
        restartDialog.SetActive(false);
        if(joy != null)
        {
            joy.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //axis direction
        float inputX = 0.0f;
        float inputY = 0.0f;

        if (joy != null)
        {
            inputX = joy.Horizontal > 0.1f || joy.Horizontal < -0.1f ? joy.Horizontal : 0.0f;
            inputY = joy.Vertical > 0.1f || joy.Vertical < -0.1f ? joy.Vertical : 0.0f;

        }
        if(Input.GetAxis("Horizontal") != 0.0f)
        {
            inputX = Input.GetAxis("Horizontal");
        }
        if (Input.GetAxis("Vertical") != 0.0f)
        {
            inputY = Input.GetAxis("Vertical");
        }

        movement = new Vector2(
            speed.x * inputX,
            speed.y * inputY);

        // Shooting
        bool shoot = Input.GetButtonDown("Fire1");
        shoot |= Input.GetButtonDown("Fire2");

        if(shoot)
        {
            WeaponScript weapon = GetComponent<WeaponScript>();
            if(weapon != null)
            {
                // false, because of player not enemy
                weapon.Attack(false);

                SoundEffectsHelper.Instance.MakePlayerShotSound(transform.position);
            }
        }

        // Out of camera bounds check
        var dist = (transform.position - Camera.main.transform.position).z;

        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;

        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;

        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;

        var bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(transform.position.y, topBorder, bottomBorder),
            transform.position.z);
    }

    private void FixedUpdate()
    {
        //object movment
        GetComponent<Rigidbody2D>().velocity = movement;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool damagePlayer = false;

        // Collision with enemy
        EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript>();

        if (enemy != null)
        {
            // Enemy death
            HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
            if (enemyHealth != null)
            {
                enemyHealth.Damage(enemyHealth.hp);
            }

            damagePlayer = true;
        }

        // Damage to player
        if (damagePlayer)
        {
            HealthScript playerHealth = this.GetComponent<HealthScript>();
            if (playerHealth != null)
            {
                playerHealth.Damage(1);
            }
        }
    }

    private void OnDestroy()
    {
        if (joy != null)
        {
            joy.gameObject.SetActive(false);
        }
        if(restartDialog != null)
        {
            restartDialog.SetActive(true);
        }
    }
}