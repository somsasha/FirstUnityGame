using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartDialogScript : MonoBehaviour
{
    public void RestartGame()
    {
        //ObjectSaver.Instance.SaveBackgroundObjects();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitToMenu()
    {
        //ObjectSaver.Instance.SaveBackgroundObjects();
        SceneManager.LoadScene(0);
    }
}
