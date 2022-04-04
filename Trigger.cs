using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    private bool active;
    public Timer timer;
    public Image image;
    public bool isTriggered = false;
    public float disableTimer;
    private bool readyTp;
    public TMP_Text text;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(readyTp)
        {
            if(disableTimer > 0)
                disableTimer -= Time.deltaTime;
            else
                timer.menu.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(!active && !isTriggered)
        {
            timer.isActive = true;
            timer.menu.SetActive(true);
            image.gameObject.SetActive(true);
            active = true;
        }
    }

    public void Disable()
    {
        timer.isActive = false;
        timer.menu.SetActive(true);
        timer.menu.GetComponentInChildren<TMP_Text>();
        text.SetText("Use the orb to teleport to the boss room !");

        readyTp = true;
        disableTimer = 1;
        image.gameObject.SetActive(false);
    }
}
