using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class TimelinePlayer : MonoBehaviour
{
    public List<PlayableDirector> director;
    private int index = 0;
    
    public void Play()
    {
        director[index++].Play();
    }
}
