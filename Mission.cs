using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Mission : MonoBehaviour
{
    private Transform player;
    private float distance;
    public bool next;
    private List<string> missionList,dialogList;
    public int index = 0;

    [SerializeField] float close;
    public TimelinePlayer time;
    public TMP_Text missionText;
    public int curr;
    private Dialog dialog;
    
    
    void Start()
    {
        dialog = GetComponent<Dialog>();
        curr = 0;
        missionList = new List<string>
        {
            "Pick Up The Pistol !",
            "Shoot 10 Rounds at the shooting target!(0/10)",
            "Shoot 50 Bullets with the rifle! (0/50)",
            "Eliminate the soldiers that are attacking the village! (0/16)",
            "Head to the secret teleport room and defeat the boss"
        };

        dialogList = new List<string>
        {
            "Go pick up a pistol and start shooting!",
            "Hmm, seems like you're not familliar with it. Come back when you have mastered the art of shooting",
            "Good Job, now get the rifle and try shooting 50 rounds",
            "Seems like you're ready for war. Now go through the passage and eliminate the enemies",
            "Great Job! no we only need to eliminate the boss! Get into the secret teleport room to fight the boss!",
        };

        player = GameObject.FindGameObjectWithTag("Player").transform;
        next = true;
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        if (distance < close)
        {
            if (Input.GetKeyDown(KeyCode.F) && next)
            {
                dialog.setDialog(dialogList[index]);
                missionText.SetText(missionList[index++]);
                missionText.color = Color.yellow;
                //next mission
                next = false;
                //Heal
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Heal();
                //Add ammo after every mission
                GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().addPistolAmmo();
                GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().addRifleAmmo();
            }
            // Dialog Masih Active atau TimeLine Active
            else if(Input.GetKeyDown(KeyCode.F) && (dialog.dialog.activeInHierarchy || FreezeInput.getFreeze()))
            {
                return;
            }
            else if(Input.GetKeyDown(KeyCode.F) && !next)
            {
                dialog.cont();
            }
        }
        
    }

    public void nextM()
    {
        missionText.color = Color.green;
        next = true;
        curr = 0;
    }
}
