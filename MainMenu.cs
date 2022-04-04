using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Resolution[] res;
    [SerializeField] TMP_Dropdown drop;
    [SerializeField] TMP_Dropdown qual_drop;
    public void Start()
    {
        drop.ClearOptions();
        qual_drop.ClearOptions();

        res = Screen.resolutions;
        List<string> options = new List<string>();
        

        int curr = res.Length - 1;


        for (int i = 0; i < res.Length; i++)
        {
            string option = res[i].width + " x " + res[i].height;
            options.Add(option);
        }

        drop.AddOptions(options);
        drop.value = curr;
        drop.RefreshShownValue();



        //Quality
        options.Clear();

        string[] quals = QualitySettings.names;

        for (int i = 0; i < quals.Length; i++)
        {
            string option = quals[i];
            options.Add(option);
        }
        qual_drop.AddOptions(options);
        qual_drop.value = 0;
    }
    public void play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void quit()
    {
        Application.Quit();
    }

    public void full()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void SetResolution(int index)
    {
        try
        {
            Screen.SetResolution(res[index].width, res[index].height, Screen.fullScreen);
        }
        catch(System.NullReferenceException e)
        {

        }
    }

    public void setQuality(int index)
    {
        QualitySettings.SetQualityLevel(index, true);
    }
}
