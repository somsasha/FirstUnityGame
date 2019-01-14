using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundEffectsHelper : MonoBehaviour
{
    // Singletone
    public static SoundEffectsHelper Instance;

    public AudioClip explosionSound;
    public AudioClip playerShotSound;
    public AudioClip enemyShotSound;

    private void Awake()
    {
        // Register singletone
        if (Instance != null)
        {
            Debug.LogError("More than one exemple of SoundEffectsHelper.");
        }

        Instance = this;
    }

    public void MakeExplosionSound(Vector3 position)
    {
        MakeSound(explosionSound, position);
    }

    public void MakePlayerShotSound(Vector3 position)
    {
        MakeSound(playerShotSound, position);
    }

    public void MakeEnemyShotSound(Vector3 position)
    {
        MakeSound(enemyShotSound, position);
    }

    private void MakeSound(AudioClip originalClip, Vector3 position)
    {
        //AudioSource.PlayClipAtPoint(originalClip, position);
        //AudioSource audioSource = new AudioSource();
        //audioSource.outputAudioMixerGroup = audioMixerGroup;
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.transform.position = position;
        audioSource.clip = originalClip;
        audioSource.Play();
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
