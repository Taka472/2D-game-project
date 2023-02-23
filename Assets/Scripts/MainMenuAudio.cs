using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuAudio : MonoBehaviour
{
    private void Awake()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("BGM");
        if (temp.Length > 1)
        {
            Destroy(gameObject);
        }
        else if (SceneManager.GetActiveScene().name != "Assignment")
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
