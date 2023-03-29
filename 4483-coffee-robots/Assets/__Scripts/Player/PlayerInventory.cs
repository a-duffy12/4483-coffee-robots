using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public static int scrap;
    public static int electronics;
    public static int tech;

    [Header("Gameobjects")]
    public Transform firePoint;
    [SerializeField] private GameObject arPrefab;
    [SerializeField] private GameObject machetePrefab;
    [SerializeField] private GameObject shotgunPrefab;
    [SerializeField] private Image arBar;
    [SerializeField] private TMP_Text arText;
    [SerializeField] private GameObject arBarObj;
    [SerializeField] private GameObject macheteBarObj;
    [SerializeField] private Image shotgunBar;
    [SerializeField] private TMP_Text shotgunText;
    [SerializeField] private GameObject shotgunBarObj;
    [SerializeField] private TMP_Text scrapText;
    [SerializeField] private TMP_Text electronicsText;
    [SerializeField] private TMP_Text techText;

    [Header("Audio")]
    public AudioClip switchAudio;

    AudioSource inventorySource;
    [HideInInspector] public AssaultRifle ar;
    [HideInInspector] public Machete machete;
    [HideInInspector] public Shotgun shotgun;
    [HideInInspector] public List<GameObject> defenses;

    [HideInInspector] public int currentWeaponInt;
    
    private float shootAutoWeapon;
    private bool interact = false;
    private bool arActivated = false;
    private bool macheteActivated = false;
    private bool shotgunActivated = false;

    void Awake()
    {
        inventorySource = GameObject.FindGameObjectWithTag("Inventory").GetComponent<AudioSource>();
        ar = arPrefab.GetComponent<AssaultRifle>();
        machete = machetePrefab.GetComponent<Machete>();
        shotgun = shotgunPrefab.GetComponent<Shotgun>();

        scrap = 0;
        electronics = 0;
        tech = 0;
    }

    void Start()
    {
        inventorySource.playOnAwake = false;
        inventorySource.spatialBlend = 1f;
        inventorySource.volume = 0.3f;
        inventorySource.priority = 128;

        SwitchWeapons(ar.weaponInt);
        
        arBar.fillAmount = Mathf.Clamp((float)ar.currentAmmo/Config.maxAmmoAR, 0, Config.maxAmmoAR);
        shotgunBar.fillAmount = Mathf.Clamp((float)shotgun.currentAmmo/Config.maxAmmoShot, 0, Config.maxAmmoShot);
        arText.text = ar.currentAmmo.ToString("F0");
        shotgunText.text = shotgun.currentAmmo.ToString("F0");

        scrapText.text = scrap.ToString("F0");
        electronicsText.text = electronics.ToString("F0");
        techText.text = tech.ToString("F0");
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

        if (interact)
        {
            // react to nearby stuff
            interact = false;
        }

        if (currentWeaponInt == 0)
        {
            arBar.fillAmount = Mathf.Clamp((float)ar.currentAmmo/Config.maxAmmoAR, 0, Config.maxAmmoAR);
            arText.text = ar.currentAmmo.ToString("F0");
        }
        else if (currentWeaponInt == 2)
        {
            shotgunBar.fillAmount = Mathf.Clamp((float)shotgun.currentAmmo/Config.maxAmmoShot, 0, Config.maxAmmoShot);
            shotgunText.text = shotgun.currentAmmo.ToString("F0");
        }

        scrapText.text = scrap.ToString("F0");
        electronicsText.text = electronics.ToString("F0");
        techText.text = tech.ToString("F0");
    }

    void SwitchWeapons(int weaponInt)
    {
        if (weaponInt != currentWeaponInt)
        {
            currentWeaponInt = weaponInt;
            inventorySource.clip = switchAudio;
            inventorySource.Play();
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

            arBarObj.SetActive(true);
            macheteBarObj.SetActive(false);
            shotgunBarObj.SetActive(false);

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

            arBarObj.SetActive(false);
            macheteBarObj.SetActive(true);
            shotgunBarObj.SetActive(false);

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

            arBarObj.SetActive(false);
            macheteBarObj.SetActive(false);
            shotgunBarObj.SetActive(true);

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

    public void Interact(InputAction.CallbackContext con)
    {
        if (con.performed)
        {
            interact = true;
        }
    }

    #endregion input functions
}
