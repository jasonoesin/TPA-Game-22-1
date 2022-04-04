using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Rifle : MonoBehaviour
{
    [Header("Equipped")]
    public bool isEquipped;
    [Header("Settings")]
    public float cooldownSpeed;
    public float fireRate;
    public float recoilCooldown;
    private float accuracy;
    public float maxSpreadAngle;
    public float timeTillMaxSpread;
    [Header("GameObjects")]
    public GameObject bullet;
    public GameObject shootPoint;
    public AudioSource singleShot;
    public TMP_Text rifleText;

    // Start is called before the first frame update
    void Start()
    {
        singleShot = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldownSpeed += Time.deltaTime * 60f;

        if (Input.GetKeyDown(KeyCode.R) && isEquipped && Inventory.rifleC != 30 && Inventory.rifleAmmo > 0)
        {
            int dif = 30 - Inventory.rifleC;

            if (Inventory.rifleAmmo - dif >= 0)
            {
                Inventory.rifleC = 30;
                Inventory.rifleAmmo -= dif;
            }
            else
            {
                Inventory.rifleC += Inventory.rifleAmmo;
                Inventory.rifleAmmo = 0;
            }
            rifleText.SetText(Inventory.rifleC + " / " + Inventory.rifleAmmo);
        }

        if (Input.GetButton("Fire1") && isEquipped && Inventory.rifleC > 0)
        {

            accuracy += Time.deltaTime * 4f;
            if (cooldownSpeed >= fireRate)
            {
                Inventory.rifleC--;
                rifleText.SetText(Inventory.rifleC + " / " + Inventory.rifleAmmo);
                Shoot();
                singleShot.Play();
                cooldownSpeed = 0;
                recoilCooldown = 1;
            }
        }
        else
        {
            recoilCooldown -= Time.deltaTime;
            if (recoilCooldown <= 1)
            {
                accuracy = 0.0f;
            }
        }
    }

    void Shoot()
    {
        RaycastHit hit;
        Quaternion fireRotation = Quaternion.LookRotation(transform.forward);
        float currentSpread = Mathf.Lerp(0.0f, maxSpreadAngle, accuracy / timeTillMaxSpread);
        fireRotation = Quaternion.RotateTowards(fireRotation, Random.rotation, Random.Range(0.0f, currentSpread));
        if (Physics.Raycast(transform.position, fireRotation * Vector3.forward, out hit, Mathf.Infinity))
        {
            GameObject tempBullet = Instantiate(bullet, shootPoint.transform.position, fireRotation);
            tempBullet.GetComponent<RifleBullet>().Push();
        }
    }
}