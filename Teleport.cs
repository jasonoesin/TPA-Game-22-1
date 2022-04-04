using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    private Transform player;
    private float distance;
    [SerializeField] float close;
    public GameObject target;
    public GameObject menu;
    public Inventory inv;
    public Mission mission;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if(distance < close)
        {
            menu.SetActive(true);
            if(Input.GetKeyDown(KeyCode.F) && mission.next)
            {
                player.position = target.transform.position;
                player.GetComponent<PlayerController>().Heal();
            }
        }
        else
        {
                menu.SetActive(false);
        }
    }
}
