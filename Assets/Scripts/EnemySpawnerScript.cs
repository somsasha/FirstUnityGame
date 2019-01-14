using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawnerScript : MonoBehaviour
{
    // Enemy prefab
    public Transform enemyPrefab;

    // Boss prefab
    public Transform bossPrefab;

    // Enemy weapon prefab
    public Transform enemyWeaponPrefab;

    // Spawn delay
    [Range(0.0f, 100.0f)]
    public float spawnTime = 1.0f;

    // Min count of weapons
    [Range(0, 13)]
    public int minCountOfWeapons = 1;

    // Max count of weapons
    [Range(0, 13)]
    public int maxCountOfWeapons = 4;

    // Hardpoints
    [Range(0.0f, 360.0f)]
    public List<float> hardpoints = new List<float>() { 0.0f, 30.0f, 60.0f, 90.0f, 120.0f, 150.0f, 180.0f, 210.0f, 240.0f, 270.0f, 300.0f, 330.0f, 360.0f };

    public Text pointsLabel;

    public int BossFightPoints = 25;

    private int BossWaitTime = 5;

    private int BossFightTime = 17;

    private bool BossFight = false;

    // Start is called before the first frame update
    void Start()
    {
        enemyPrefab.GetComponent<EnemyScript>().pointsLabel = pointsLabel;

        InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    private void Spawn()
    {
        // If the player has no health left...
        HealthScript playerHP = GetComponentInChildren<HealthScript>();
        if (playerHP != null)
        {
            if (GetComponentInChildren<HealthScript>().hp <= 0)
            {
                // ... exit the function.
                return;
            }
        }   else { return; }

        if (System.Convert.ToInt32(pointsLabel.text) >= BossFightPoints)
        {
            if (!BossFight && BossWaitTime <= 0)
            {
                BossFightTime = 20;
                BossFight = true;
                BossSpawn();
            }
            else if (!BossFight)
            {
                BossWaitTime--;
            }
            else if (BossFight && BossFightTime > 0)
            {
                BossFightTime--;
            }
            else if (BossFight && BossFightTime <= 0)
            {
                BossFight = false;
                BossFightTime = 20;
                BossWaitTime = 5;
                pointsLabel.text = (System.Convert.ToInt32(pointsLabel.text) + BossFightPoints).ToString();
                BossFightPoints *= 4;
            }
            return;
        }

        // Start position
        Vector3 position = new Vector3(Camera.main.transform.position.x + Camera.main.orthographicSize + Random.Range(5.0f, 15.0f), Random.Range(-9.0f, 9.0f), transform.position.z);
        // Spawn enemy
        Transform enemy = Instantiate(enemyPrefab, position, transform.rotation);

        enemy.parent = transform;

        List<float> availableHardpoints = new List<float>(hardpoints);

        // Add Weapons
        for (int i = 0; i < Random.Range(minCountOfWeapons, maxCountOfWeapons); i++)
        {
            int hardpointNumber = Random.Range(0, availableHardpoints.Count - 1);
            float hardpoint = availableHardpoints[hardpointNumber];
            availableHardpoints.RemoveAt(hardpointNumber);
            Transform weapon;
            weapon = Instantiate(enemyWeaponPrefab);
            weapon.position = position;
            weapon.Rotate(0, 0, hardpoint);
            weapon.parent = enemy;
        }
    }

    private void BossSpawn()
    {
        // Start position
        Vector3 position = new Vector3(Camera.main.transform.position.x + Camera.main.orthographicSize + Random.Range(5.0f, 15.0f), Random.Range(-9.0f, 9.0f), transform.position.z);
        // Spawn enemy
        Transform boss = Instantiate(bossPrefab, position, transform.rotation);
        boss.parent = transform;
    }
}
