using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public GameObject dialog;
    public TMP_Text text;
    public FreezeInput inp;
    public TimelinePlayer time;
    public bool flag;
    // Start is called before the first frame update
    void Start()
    {
        inp = GameObject.FindGameObjectWithTag("Player").GetComponent<FreezeInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.activeInHierarchy && Input.GetKeyDown(KeyCode.Space) && flag)
        {
            time.Play();
            dialog.SetActive(false);
            flag = false;
        }
        else if(dialog.activeInHierarchy && Input.GetKeyDown(KeyCode.Space))
        {
            dialog.SetActive(false);
            inp.Release();
        }
    }
    
    public void cont()
    {
        text.SetText("You haven't finished your task yet. Come back when you have finished it. [Space to continue]");
        dialog.SetActive(true);
        flag = false;
        // DISABLE CHARACTER MOVEMENT DLL
        // ...
        inp.Freeze();
    }

    public void setDialog(string temp)
    {
        text.SetText(temp + " [Space to continue]");
        flag = true;
        dialog.SetActive(true);
        inp.Freeze();
    }
}
