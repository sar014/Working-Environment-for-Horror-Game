using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    //Note: This script is part of installed package.
    
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public int counter;//No. of bullets

    [Header("Location Refrences")]
    public Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;


    [Tooltip("UI of bullets present")] public List<Image> bullets;

    [Tooltip("Accessing weapon holder game object")] [SerializeField] public GameObject weaponholder;

    void Start()
    {
        //Disabling shooting animation.Would be enabled via PropToPickUp script
        gunAnimator.enabled = false;

        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        // Find the bullet images dynamically
        FindBulletImages();

        // Debugging: Ensure the bullets list is populated
        if (bullets == null || bullets.Count == 0)
        {
            Debug.LogError("Bullets list is not assigned or empty!");
        }
        weaponholder = GameObject.Find("WeaponHolder");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && counter!=0 && gunAnimator.enabled==true)
        {
            //Calls animation on the gun that has the relevant animation events that will fire
            gunAnimator.SetTrigger("Fire");
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        counter-=1;
        updateBulletUI();
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

    }

    //This function creates a casing at the ejection slot 
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

    //Runs everytime after fire method runs
    void updateBulletUI()
    {
        // Debug.Log("Counter"+counter);
        for(int i=0;i<bullets.Count;i++)
        {
            if(i<counter)
            {
                bullets[i].enabled = true;
            }
            else
            {
                bullets[i].enabled = false;
            }
        }
    }

    // Method to find bullet images dynamically
    void FindBulletImages()
    {
        GameObject canvas = GameObject.Find("GunCanvas"); 
        if (canvas != null)
        {
            Image[] images = canvas.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.name.StartsWith("Bullet")) 
                {
                    bullets.Add(img);
                }
            }
        }
        else
        {
            Debug.LogError("Canvas not found!");
        }
    }


    public void SetAmmo(int ammo)
    {
        counter = ammo;
        updateBulletUI();
    }

    public int GetAmmo()
    {
        return counter;
    }

}
