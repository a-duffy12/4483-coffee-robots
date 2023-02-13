using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Gameobjects")]
    public Transform firePoint;
    [SerializeField] private GameObject arPrefab;
    [SerializeField] private GameObject machetePrefab;
    [SerializeField] private GameObject shotgunPrefab;
    // ui fields

    [Header("Audio")]

    AudioSource inventorySource;
    AssaultRifle ar;
    Machete machete;
    Shotgun shotgun;

    [HideInInspector] public int currentWeaponInt;
    
    private float shootAutoWeapon;
    private bool arActivated = false;
    private bool macheteActivated = false;
    private bool shotgunActivated = false;

    void Awake()
    {
        inventorySource = GameObject.FindGameObjectWithTag("Inventory").GetComponent<AudioSource>();
        ar = arPrefab.GetComponent<AssaultRifle>();
        machete = machetePrefab.GetComponent<Machete>();
        shotgun = shotgunPrefab.GetComponent<Shotgun>();
    }

    void Start()
    {
        inventorySource.playOnAwake = false;
        inventorySource.spatialBlend = 1f;
        inventorySource.volume = 1f;
        inventorySource.priority = 128;

        SwitchWeapons(ar.weaponInt);
        // show inventory ui
    }

    void Update()
    {
        if (shootAutoWeapon == 1) // trigger is held down
        {
            if (currentWeaponInt == 0)
            {
                ar.Fire(firePoint);
            }
        }

        // update ar ammo ui
        if (Config.shotgunUnlocked)
        {
            // update shotgun ammo ui
        }
    }

    void SwitchWeapons(int weaponInt)
    {
        if (weaponInt != currentWeaponInt)
        {
            currentWeaponInt = weaponInt;
            // play swap audio
        }
        else
        {
            return;
        }

        if (currentWeaponInt == 0)
        {
            arPrefab.SetActive(true);
            machetePrefab.SetActive(false);
            shotgunPrefab.SetActive(false);

            ar.OverrideLastFireTime(); // can shoot after swapping

            if (!arActivated)
            {
                arActivated = true;
            }
        }
        if (currentWeaponInt == 1)
        {
            arPrefab.SetActive(false);
            machetePrefab.SetActive(true);
            shotgunPrefab.SetActive(false);

            machete.OverrideLastFireTime(); // can shoot after swapping

            if (!macheteActivated)
            {
                macheteActivated = true;
            }
        }
        if (currentWeaponInt == 2)
        {
            arPrefab.SetActive(false);
            machetePrefab.SetActive(false);
            shotgunPrefab.SetActive(true);

            shotgun.OverrideLastFireTime(); // can shoot after swapping

            if (!shotgunActivated)
            {
                shotgunActivated = true;
            }
        }
    }

    // inventory ui update function

    #region input functions

    public void Shoot(InputAction.CallbackContext con)
    {
        if (con.performed) // key press for semi auto
        {
            if (currentWeaponInt == 1)
            {
                machete.Fire(firePoint);
            }
            else if (currentWeaponInt == 2)
            {
                shotgun.Fire(firePoint);
            }
        }

        shootAutoWeapon = con.ReadValue<float>(); // key hold for auto weapon
    }

    public void AlternateShoot(InputAction.CallbackContext con)
    {
        if (con.performed) // all alternate fire are semi auto
        {
            if (Config.alternateARUnlocked && currentWeaponInt == 0)
            {
                ar.AlternateFire(firePoint);
            }
            else if (Config.alternateMacheteUnlocked && currentWeaponInt == 1)
            {
                machete.AlternateFire(firePoint);
            }
            else if (Config.alternateShotgunUnlocked && currentWeaponInt == 2)
            {
                shotgun.AlternateFire(firePoint);
            }
        }
    }

    public void Item1(InputAction.CallbackContext con)
    {
        if (con.performed)
        {
            SwitchWeapons(ar.weaponInt);
        }
    }

    public void Item2(InputAction.CallbackContext con)
    {
        if (con.performed)
        {
            SwitchWeapons(machete.weaponInt);
        }
    }

    public void Item3(InputAction.CallbackContext con)
    {
        if (Config.shotgunUnlocked && con.performed)
        {
            SwitchWeapons(shotgun.weaponInt);
        }
    }

    #endregion input functions
}
