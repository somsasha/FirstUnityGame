using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffectsHelper : MonoBehaviour
{
    // Singletone
    public static SpecialEffectsHelper Instance;

    public ParticleSystem smokeEffect;
    public ParticleSystem fireEffect;

    private void Awake()
    {
        // Singletone register
        if(Instance!=null)
        {
            Debug.LogError("More than one exemple of SpecialEffectsHelper.");
        }

        Instance = this;
    }

    // Make an explosion in point
    public void Explosion(Vector3 position)
    {
        // Smoke
        instantiate(smokeEffect, position);

        // Fire
        instantiate(fireEffect, position);
    }

    // Make an example of particle system from prefab
    private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
    {
        ParticleSystem newParticleSystem = Instantiate(prefab, position, Quaternion.identity) as ParticleSystem;

        Destroy(newParticleSystem.gameObject, newParticleSystem.startLifetime);

        return newParticleSystem;
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
