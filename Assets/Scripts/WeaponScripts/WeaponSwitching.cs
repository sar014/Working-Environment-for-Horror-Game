using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;
    public int gunIndex;

    // Store ammo counts for each weapon
    public int[] ammoCounts;

    void Start()
    {
        //Searching for guns in the Area
        int sizeOfArray = SearchGunTags();

        //Initializing array ammoCount so that the ammo remains separate for both guns
        ammoCounts = new int[sizeOfArray];
        for (int i = 0; i < ammoCounts.Length; i++)
        {
            ammoCounts[i] = 6;
        }   
        // SelectWeapon();
    }

    int SearchGunTags()
    {
        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");
        return guns.Length;
    }

    void Update()
    {
        //When you click 1
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
            SelectWeapon();
        }

        //When you click 2
        if(Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount>=1)
        {
            selectedWeapon = 1;
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            SimpleShoot simpleShoot = weapon.GetComponentInChildren<SimpleShoot>();
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                if (simpleShoot != null)
                {
                    // if(simpleShoot.GetAmmo()==ammoCounts[i])
                    simpleShoot.SetAmmo(ammoCounts[i]);
                }
                else
                {
                    //Must be a melee weapon
                }
            }
            else
            {
                if(weapon.tag=="Gun")
                {
                    if (simpleShoot != null)
                    {
                        ammoCounts[i] = simpleShoot.GetAmmo();
                    }
                    else
                    {
                        Debug.Log("Not Working");
                    }
                }
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
