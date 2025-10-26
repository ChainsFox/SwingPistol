using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalRifleAmmo = 0;
    public int totalPistolAmmo = 0;

    [Header("Throwables")]
    public float throwForce = 10f;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit = 2f;

    [Header("Lethals")]
    public int maxLethals = 5;
    public int lethalsCount = 0;
    //public Throwable.ThrowableType equippedLethalType; //we only allow 1 throwable type that can be equipped
    public GameObject grenadePrefab;

    [Header("Tactials")]
    public int maxTacticals = 5;
    public int tacticalCount = 0;
    //public Throwable.ThrowableType equippedTacticalType; 
    public GameObject smokeGrenadePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    //private void Start()
    //{
    //    activeWeaponSlot = weaponSlots[0];

    //    equippedLethalType = Throwable.ThrowableType.None;
    //    equippedTacticalType = Throwable.ThrowableType.None;
    //}

    private void Update()
    {
        /*We are looping all over these weapon in weaponSlots(list), and if one of these slot is the
         * active slot, then we are going to make the gameobject of this slot active, else false.
         */
        foreach(GameObject weaponSlot in weaponSlots) 
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }

        if(Input.GetKeyDown(KeyCode.Alpha1)) //alpha1 = number 1 key
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }

        if (Input.GetKey(KeyCode.G) || Input.GetKey(KeyCode.T)) //hit and hold the g/t key -> forceMultiplier increase
        {
            forceMultiplier += Time.deltaTime;

            if(forceMultiplier > forceMultiplierLimit)
            {
                forceMultiplier = forceMultiplierLimit;
            }
        }

        //if (Input.GetKeyUp(KeyCode.G))
        //{
        //    if(lethalsCount > 0)
        //    {
        //        ThrowLethal();

        //    }
        //    forceMultiplier = 0f;
        //}

        //if (Input.GetKeyUp(KeyCode.T))
        //{
        //    if (tacticalCount > 0)
        //    {
        //        ThrowTactical();

        //    }
        //    forceMultiplier = 0f;
        //}

    }

    public void PickupWeapon(GameObject pickedupWeapon) //a dedicated method that is going to equip the weapon into the active weapon slot
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    public void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false); //take this weapon transform, and set its parent to be the active weapon slot, active weapon slot always get updated

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>(); //grab the Weapon script from this weapon, 

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z); //and then set the local position/rotation of this model to be according to the spawnPosition/Rotation 
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z); //that we have set in the editor

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    //internal void PickupAmmo(AmmoBox ammo)
    //{
    //    switch(ammo.ammoType)
    //    {
    //        case AmmoBox.AmmoType.PistolAmmo:
    //            totalPistolAmmo += ammo.ammoMount;
    //            break;

    //        case AmmoBox.AmmoType.RifleAmmo:
    //            totalRifleAmmo += ammo.ammoMount;
    //            break;



    //    }
    //}

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if(activeWeaponSlot.transform.childCount > 0) //check if the active weapon slot already has something inside of it
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject; //get the weapon that we are using and saving it in this variable

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false; //disable it from being the active weapon
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent); //set parent of the new weapon that we pick up to be the same parent of the old weapon 
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition; //also set the same position/rotation = to the old weapon
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation; //=> basically we are switching values between the 2
        }
    }

    public void SwitchActiveSlot(int slotNumber) //this function will receive a number to switch between weapon slot
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }

    }

    internal void DecreaseTotalAmmo(int bulletsToDecrease, Weapon.WeaponModel thisWeaponModel)
    {
        switch(thisWeaponModel)
        {
            case Weapon.WeaponModel.M4:
                totalRifleAmmo -= bulletsToDecrease;
                break;

            case Weapon.WeaponModel.M1911:
                totalPistolAmmo -= bulletsToDecrease;
                break;

        }


    }

    public int CheckAmmoLeftFor(Weapon.WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.M4:
                return totalRifleAmmo;

            case Weapon.WeaponModel.M1911:
                return totalPistolAmmo;

            default:
                return 0;

        }
    }

    #region || throwables ||
    //public void PickupThrowable(Throwable throwable)
    //{
    //    switch(throwable.throwableType)
    //    {
    //        case Throwable.ThrowableType.Grenade:
    //            PickupThrowableAsLethal(Throwable.ThrowableType.Grenade);
    //            break;

    //        case Throwable.ThrowableType.Smoke_Grenade:
    //            PickupThrowableAsTactial(Throwable.ThrowableType.Smoke_Grenade);
    //            break;

    //    }
    //}

    //private void PickupThrowableAsTactial(Throwable.ThrowableType tactical)
    //{
    //    if (equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None)
    //    {
    //        equippedTacticalType = tactical;

    //        if (tacticalCount < maxTacticals) //limit the amount of lethals(grenades)
    //        {
    //            tacticalCount += 1;
    //            Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
    //            HUBManager.Instance.UpdateThrowablesUI();


    //        }
    //        else
    //        {
    //            print("Tacticals limit reached");
    //        }

    //    }
    //    else
    //    {
    //        //cannot pickup different tactical
    //        //option to swap tactical
    //    }
    //}

    //private void PickupThrowableAsLethal(Throwable.ThrowableType lethal)
    //{
    //    if (equippedLethalType == lethal || equippedLethalType == Throwable.ThrowableType.None)
    //    {
    //        equippedLethalType = lethal;

    //        if (lethalsCount < maxLethals) //limit the amount of lethals(grenades)
    //        {
    //            lethalsCount += 1;
    //            Destroy(InteractionManager.Instance.hoveredThrowable.gameObject);
    //            HUBManager.Instance.UpdateThrowablesUI();


    //        }
    //        else
    //        {
    //            print("Lethals limit reached");
    //        }

    //    }
    //    else
    //    {
    //        //cannot pickup different lethal
    //        //option to swap lethals
    //    }
    //}

    ////private void PickupGrenade() - remove in p15
    ////{
    ////    grenades += 1;

    ////    HUBManager.Instance.UpdateThrowables(Throwable.ThrowableType.Grenade);
    ////}

    //private void ThrowLethal()
    //{
    //    GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType); //temporay solution

    //    GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
    //    Rigidbody rb = throwable.GetComponent<Rigidbody>();

    //    rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

    //    throwable.GetComponent<Throwable>().hasBeenThrown = true; //start countdown for the explosion

    //    lethalsCount -= 1;
    //    if(lethalsCount <= 0)
    //    {
    //        equippedLethalType = Throwable.ThrowableType.None;
    //    }

    //    HUBManager.Instance.UpdateThrowablesUI();
    //}

    //private void ThrowTactical()
    //{
    //    GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType); //temporary solution

    //    GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
    //    Rigidbody rb = throwable.GetComponent<Rigidbody>();

    //    rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);

    //    throwable.GetComponent<Throwable>().hasBeenThrown = true; 

    //    tacticalCount -= 1;
    //    if (tacticalCount <= 0)
    //    {
    //        equippedTacticalType = Throwable.ThrowableType.None;
    //    }

    //    HUBManager.Instance.UpdateThrowablesUI();
    //}

    //private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType)
    //{
    //    switch(throwableType)
    //    {
    //        case Throwable.ThrowableType.Grenade:
    //            return grenadePrefab;

    //        case Throwable.ThrowableType.Smoke_Grenade:
    //            return smokeGrenadePrefab;
    //    }

    //    return new(); //we have to return something so we put this in put this code will never run
    //}

    #endregion


}
