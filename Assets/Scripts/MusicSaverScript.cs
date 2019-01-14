using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSaverScript : MonoBehaviour
{
    [HideInInspector]
    static int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(count == 0)
        {
            DontDestroyOnLoad(this);
            count++;
        }
        else
        {
            transform.GetComponent<AudioSource>().enabled = false;
        }

    }
}
