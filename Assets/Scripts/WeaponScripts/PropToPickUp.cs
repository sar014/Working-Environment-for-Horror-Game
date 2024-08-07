using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PropToPickUp : MonoBehaviour
{
    GameObject Parent;
    WeaponSwitching weaponSwitching;
    public bool inReach;
    public Animator animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        inReach = false;
        GameObject fpsGameObject = GameObject.Find("FirstPersonPlayer");
        Transform cameraTransform = fpsGameObject.transform.Find("Main Camera");
        Parent = cameraTransform.Find("WeaponHolder")?.gameObject;

        //Attaching the FadeOut Text animation to the animator component
        GameObject gunCanvas = GameObject.Find("GunCanvas");
        TextMeshProUGUI text = gunCanvas.GetComponentInChildren<TextMeshProUGUI>();
        animator = text.GetComponent<Animator>();
    }
    
    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag("Reach"))
        {   
            //Ensures only two weapons are added to inventory at a time
            if(Parent.transform.childCount>=2)
            {
                Debug.Log(this.gameObject.name);
                animator.SetTrigger("FadeOut");
                Debug.Log("Inventory Full");
            }
            else
            {
                AddingToInventory(this.gameObject);
                inReach = true;
            }
            
        }
    }

    void OnTriggerExit(Collider other) 
    {
        inReach =false;
    }

    void AddingToInventory(GameObject weapon)
    {
        //Accessing the simpleShoot script in order to access the gunAnimator animation component
        if(weapon.tag=="Gun")
        {
            SimpleShoot simpleShoot= weapon.GetComponentInChildren<SimpleShoot>();
            simpleShoot.gunAnimator.enabled = true;
        }

        //Setting the weapon a child of weapon holder and setting its transform values
        weapon.transform.SetParent(Parent.transform);
        // Destroy(weapon.GetComponent<BoxCollider>());//Preventing the weapon to interact with the reach component when it swings
        weapon.transform.SetLocalPositionAndRotation(new Vector3(0.0769999921f,-0.230000019f,0.439999998f), new Quaternion(0,0,0,0));
        weapon.transform.localScale = new Vector3(1,1,1);

        //Ensuring that the first weapon picked up is the one enabled even after picking the 2nd weapon     
        if(Parent.transform.childCount>1)
        {
            // Debug.Log(Parent.transform.GetChild(1).name);
            // Debug.Log("More than 1");
            weapon.gameObject.SetActive(false);
        }
    }

    void CheckAmmoCount()
    {
        int gunPos;
    }

    void Update()
    {
        if(Input.GetButtonDown("Submit"))
        {
            this.gameObject.transform.parent = null;
            this.gameObject.transform.localPosition = new Vector3(this.transform.localPosition.x+5,0,this.transform.localPosition.z); //Adding 5 units so that the weapon falls a few units away
            this.gameObject.transform.localRotation = Quaternion.Euler(0,0,90);
            this.AddComponent<BoxCollider>();
            // Destroy(this.gameObject);
        }
    }
}
