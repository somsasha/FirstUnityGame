using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void PlayGame()
    {
        ObjectSaver.Instance.SaveBackgroundObjects();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetSoundVolume(float volume)
    {
        audioMixer.SetFloat("SoundVolume", volume);
    }
    
    public void GetVolumes()
    {
        List<Slider> sliders = new List<Slider>(GetComponentsInChildren<Slider>());
        float value;
        audioMixer.GetFloat("MasterVolume", out value);
        sliders[0].value = value;
        audioMixer.GetFloat("MusicVolume", out value);
        sliders[1].value = value;
        audioMixer.GetFloat("SoundVolume", out value);
        sliders[2].value = value;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
