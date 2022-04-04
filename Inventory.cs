using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] Rig rig;
    public bool isOn = false;
    [SerializeField] private LayerMask layer;
    [SerializeField] private float range;
    [SerializeField] Transform camPos;
    [Header("Ammos")]
    public static int rifleAmmo, pistolAmmo, rifleC, pistolC;
    [Header("Menus")]
    public TMP_Text rifleText;
    public TMP_Text pistolText;
    public GameObject middleMenu;
    [SerializeField] Mission mission;
    private Rifle rifle;
    private Pistol pistol;
    [Header("Item Image UI")]
    public Image pistolImg, rifleImg;
    [Header("Does Have Boolean")]
    public bool DHPistol, DHRifle;
    [Header("Item Rig")]
    [SerializeField] GameObject pistolRig;
    [SerializeField] GameObject rifleRig;

    private float on = 1;

    void Start()
    {
        DHPistol = DHRifle = false;
        rifleAmmo = 0;
        pistolAmmo = 0;
        rifle = GameObject.Find("Character Cam").GetComponent<Rifle>();
        pistol = GameObject.Find("Character Cam").GetComponent<Pistol>();
        rig.weight = 0;
    }

    void Update()
    {
        rig.weight = Mathf.Lerp(rig.weight, on, 10f * Time.deltaTime);
        if (!pistol.isEquipped && !rifle.isEquipped)
            on = 0;
        else
            on = 1;

        if (on == 0)
            rig.weight = 0;

        if (middleMenu.activeInHierarchy)
        {
            middleMenu.SetActive(false);
            isOn = false;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1) && DHRifle)
            equipRifle();
        if (Input.GetKeyDown(KeyCode.Alpha2) && DHPistol)
            equipPistol();


        Vector2 screenCentre = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(screenCentre);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, range, layer))
        {
            middleMenu.GetComponentInChildren<TMP_Text>().SetText("Press F to take " + hitInfo.collider.gameObject.name);
            if (hitInfo.collider.gameObject.name == "Rifle")
            {
                if (Input.GetKeyDown(KeyCode.F) && mission.index == 3)
                {
                    DHRifle = true;
                    rifleC = 30;
                    rifleText.SetText(rifleC + " / " + rifleAmmo);
                    Destroy(hitInfo.transform.gameObject);
                    equipRifle();
                }
            }
            else if (hitInfo.collider.gameObject.name == "Pistol")
            {
                if (Input.GetKeyDown(KeyCode.F) && mission.index == 1)
                {
                    DHPistol = true;
                    pistolC = 7;
                    pistolText.SetText(pistolC + " / " + pistolAmmo);

                    mission.nextM();
                    Destroy(hitInfo.transform.gameObject);
                    equipPistol();
                }
            }
            else if (hitInfo.collider.gameObject.name.Contains("BSGAmmoBoxB"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    addPistolAmmo();
                    Destroy(hitInfo.transform.gameObject);
                }
                middleMenu.GetComponentInChildren<TMP_Text>().SetText("Press F to take Pistol Ammo");
            }
            else if (hitInfo.collider.gameObject.name.Contains("BSGAmmoBoxA"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    addRifleAmmo();
                    Destroy(hitInfo.transform.gameObject);
                }
                middleMenu.GetComponentInChildren<TMP_Text>().SetText("Press F to take Rifle Ammo");
                
            }

            middleMenu.SetActive(true);
            isOn = true;

        }
        //else if(Physics.SphereCast(ray, out RaycastHit hitSphere, 1, layer))
        //{ 
        //    middleMenu.GetComponentInChildren<TMP_Text>().SetText("Press F to take " + hitSphere.collider.gameObject.name);
        //}
    }

    void equipRifle()
    {
        rifle.isEquipped = (rifle.isEquipped == true) ? false : true;
        if (rifle.isEquipped)
        {
            rifleRig.SetActive(true);
            pistolRig.SetActive(false);
        }
        else
        {
            rifleRig.SetActive(false);
        }
            
        pistol.isEquipped = false;

        rifleImg.color = (rifle.isEquipped == true) ? new Color32(255, 255, 255, 255) :
            new Color32(255, 255, 255, 120);
        pistolImg.color = (pistol.isEquipped == true) ? new Color32(255, 255, 255, 255) :
            new Color32(255, 255, 255, 120);
    }

    void equipPistol()
    {
        pistol.isEquipped = (pistol.isEquipped == true) ? false : true;
        if(pistol.isEquipped)
        {
            pistolRig.SetActive(true);
            rifleRig.SetActive(false);
        }
        else
            pistolRig.SetActive(false);

        rifle.isEquipped = false;

        rifleImg.color = (rifle.isEquipped == true) ? new Color32(255, 255, 255, 255) :
            new Color32(255, 255, 255, 120);
        pistolImg.color = (pistol.isEquipped == true) ? new Color32(255, 255, 255, 255) :
            new Color32(255, 255, 255, 120);
    }

    public void addPistolAmmo()
    {
        pistolAmmo += 7;
        pistolText.SetText(pistolC + " / " + pistolAmmo);
    }

    public void addRifleAmmo()
    {
        rifleAmmo += 30;
        rifleText.SetText(rifleC + " / " + rifleAmmo);
    }

}

