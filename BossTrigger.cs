using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayableDirector director;
    private bool isTriggered = false;
    public GameObject menu;

    private void OnTriggerEnter(Collider other)
    {
        if(!isTriggered)
        {
            director.Play();
            menu.SetActive(true);
            isTriggered = true;
        }
    }
}
