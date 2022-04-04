using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class Pistol : MonoBehaviour
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
    public TMP_Text pistolText;

    public Light muzzleFlashLight;
    public ParticleSystem muzzleFlashParticles;
    float lightIntensity;
    [SerializeField] float lightReturnSpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        singleShot = GetComponent<AudioSource>();
        lightIntensity = muzzleFlashLight.intensity;
        muzzleFlashLight.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        cooldownSpeed += Time.deltaTime * 60f;

        if (Input.GetKeyDown(KeyCode.R) && isEquipped && Inventory.pistolC != 7 && Inventory.pistolAmmo > 0)
        {
            int dif = 7 - Inventory.pistolC;

            if(Inventory.pistolAmmo - dif >= 0)
            {
                Inventory.pistolC = 7;
                Inventory.pistolAmmo -= dif;
            }
            else
            {
                Inventory.pistolC += Inventory.pistolAmmo;
                Inventory.pistolAmmo = 0;
            }

            pistolText.SetText(Inventory.pistolC + " / " + Inventory.pistolAmmo);
        }

        if (Input.GetButtonDown("Fire1") && isEquipped && Inventory.pistolC > 0)
        {
            if (cooldownSpeed >= fireRate)
            {
                Inventory.pistolC--;
                pistolText.SetText(Inventory.pistolC + " / " + Inventory.pistolAmmo);
                TriggerFlash();
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
        muzzleFlashLight.intensity = Mathf.Lerp(muzzleFlashLight.intensity, 0, lightReturnSpeed * Time.deltaTime);
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
            tempBullet.GetComponent<PistolBullet>().Push();
            
        }
    }

    void TriggerFlash()
    {
        muzzleFlashParticles.Play();
        muzzleFlashLight.intensity = lightIntensity;
    }
}