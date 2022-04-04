using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{

    public static bool Paused = false;
    public GameObject menu;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            func();
        }
    }

    public void func()
    {
        if (Paused)
        {
            menu.SetActive(false);
            Time.timeScale = 1f;
            Paused = false;
            CursorHide.Lock();
        }
        else
        {
            menu.SetActive(true);
            Time.timeScale = 0f;
            Paused = true;
            CursorHide.Lock();
        }
    }

    public void quit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }
}
