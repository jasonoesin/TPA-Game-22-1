using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public bool isActive;
    public TMP_Text text;
    float start = 60.0f;
    public GameObject menu;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isActive)
        {
            if(start > 0)
            {
                start -= Time.deltaTime;
            }
            else
            {
                start = 0;
            }
            double temp = System.Math.Round(start, 2);
            text.SetText(temp.ToString("0.00"));
        }

        if(start < 58.5)
        {
            menu.SetActive(false);
        }

        if(start <= 0)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().otherMenu.SetActive(false);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().dead.SetActive(true);
            CursorHide.Lock();
        }
    }
}
