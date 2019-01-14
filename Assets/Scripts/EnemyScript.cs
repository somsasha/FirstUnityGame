using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public Text pointsLabel;

    private bool hasSpawn;
    private MoveScript moveScript;
    private WeaponScript[] weapons;

    private new Collider2D collider2D;
    private new Renderer renderer;

    private void Awake()
    {
        // Give move script
        moveScript = GetComponent<MoveScript>();

        // Give Colider2D
        collider2D = GetComponent<Collider2D>();

        // Give renderer
        renderer = GetComponent<Renderer>();

        GetComponent<HealthScript>().pointsLabel = pointsLabel;
    }

    // Start is called before the first frame update
    void Start()
    {
        hasSpawn = false;

        // Disable collider2D, moveScript and weapons
        collider2D.enabled = false;

        moveScript.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn check
        if(hasSpawn == false)
        {
            if(renderer.IsVisibleFrom(Camera.main))
            {
                Spawn();
            }
        }
        else
        {
            foreach (WeaponScript weapon in weapons)
            {
                if (weapon != null && weapon.enabled && weapon.CanAttack)
                {
                    weapon.Attack(true);

                    SoundEffectsHelper.Instance.MakeEnemyShotSound(transform.position);
                }
            }

            // Out from bound of camera
            if (renderer.IsVisibleFrom(Camera.main) == false)
            {
                Destroy(gameObject);
            }
        }
    }

    // Spawn
    private void Spawn()
    {
        hasSpawn = true;

        // Give weapon only onece
        weapons = GetComponentsInChildren<WeaponScript>();

        // Enable collider2D, moveScript and weapons
        collider2D.enabled = true;

        moveScript.enabled = true;
    }
}
