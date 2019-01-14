using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{
    // Hitpoints
    public int hp = 1;

    // Is it enemy
    public bool isEnemy = true;

    public Text pointsLabel;

    public RectTransform hp_UI;

    // Make damage and check if destroy
    public void Damage(int DamageCount)
    {
        hp -= DamageCount;

        if(hp_UI != null)
        {
            hp_UI.GetComponentsInChildren<RectTransform>()[hp].gameObject.SetActive(false);
        }

        if(hp<=0)
        {
            // Explosion
            SpecialEffectsHelper.Instance.Explosion(transform.position);

            SoundEffectsHelper.Instance.MakeExplosionSound(transform.position);
            if (pointsLabel != null)
            {
                pointsLabel.text = (System.Convert.ToInt32(pointsLabel.text) + 1).ToString();
            }

            // Death
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Is it a shot
        ShotScript shot = collision.gameObject.GetComponent<ShotScript>();
        if(shot!=null)
        {
            // Frendly fire check
            if(shot.isEnemyShot!=isEnemy)
            {
                Damage(shot.damage);

                // Destroy a shot
                Destroy(shot.gameObject);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
