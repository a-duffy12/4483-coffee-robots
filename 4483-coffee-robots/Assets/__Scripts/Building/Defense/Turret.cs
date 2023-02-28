using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class Turret : MonoBehaviour, IBuilding, IDefense
{
    [Header("GameObjects")]
    [SerializeField] private Transform effectsPoint;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private ParticleSystem muzzleFlash;
    public Image healthBar;
    public GameObject buildingCanvas;
    
    //[Header("Audio")]
    //public AudioClip attackAudio;

    [HideInInspector] public float hp { get { return currentHp; } }

    private float currentHp;
    private BuildStatus buildStatus;

    AudioSource source;
    GameObject player;
    TMP_Text promptText;

    private float lastAttackTime;
    private float retargetStartTime;
    private GameObject target;
    private bool quickInteract;
    
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        source = GetComponent<AudioSource>();
        promptText = GameObject.FindGameObjectWithTag("PromptText").GetComponent<TMP_Text>();
    }

    void Start()
    {
        source.playOnAwake = false;
        source.spatialBlend = 1f;
        source.volume = 0.7f;
        source.priority = 150;

        currentHp = Config.turretMaxHp;

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.turretMaxHp, 0, Config.turretMaxHp);
        buildingCanvas.transform.rotation = Quaternion.Euler(45, 0, 0);
        buildStatus = BuildStatus.Built;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.buildingCanvasDistance && currentHp < Config.turretMaxHp)
        {
            buildingCanvas.gameObject.SetActive(true);
        }
        else
        {
            buildingCanvas.gameObject.SetActive(false);
        }

        if (buildStatus == BuildStatus.Built)
        {
            if (target == null && Time.time > retargetStartTime + Config.buildingRetargetDelay) // not currently targetting any enemy
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                float distanceToEnemy = Config.rangeTurret;
                foreach (GameObject enemy in enemies)
                {
                    float tempDistance = Vector3.Distance(enemy.transform.position, transform.position);
                    if (tempDistance < Config.rangeTurret && tempDistance < distanceToEnemy)
                    {
                        distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                        target = enemy;
                    }
                }

                retargetStartTime = Time.time;

                Attack();
            }
            else if (target != null) // already have a target
            {
                retargetStartTime = Time.time;
                
                Attack();
            }

        }
        else if (buildStatus == BuildStatus.Damaged)
        {
            if (distanceToPlayer <= Config.buildingInteractDistance)
            {
                promptText.text = $"Fix Turret {Config.buildingFixScrapCost} <sprite=2> [E]";

                if (quickInteract)
                {
                    FixBuilding();
                }
            }
            else if (promptText.text.StartsWith("Fix Turret"))
            {
                promptText.text = ""; // only sets to empty if player just left text radius
            }
        }
        else if (promptText.text.StartsWith("Fix Turret"))
        {
            promptText.text = ""; // only sets to empty if player just left text radius
        }
    }

    void Attack()
    {
        float distanceToEnemy = Vector3.Distance(target.transform.position, transform.position);
        transform.LookAt(target.transform.position);
        
        if (Time.time > lastAttackTime + (1/Config.attackRateTurret) && distanceToEnemy <= Config.rangeTurret)
        {
            if (Physics.Raycast(effectsPoint.position, target.transform.position - effectsPoint.position, out RaycastHit hit, Config.rangeTurret, hitMask))
            {
                IEnemy enemy = hit.collider.gameObject.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    enemy.DamageEnemy(Config.damageTurret, "turret");
                }
            }
            
            lastAttackTime = Time.time;

            //audioSource.clip = attackAudio;
            //audioSource.Play();

            if (muzzleFlash.isPlaying)
            {
                muzzleFlash.Stop();
            }
            muzzleFlash.Play();
        }
    }

    public void DamageBuilding(float damage, string source = "")
    {
        damage = (float)Mathf.FloorToInt(damage);

        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.turretMaxHp, 0, Config.turretMaxHp);

        if (currentHp <= 0)
        {
            //buildStatus = BuildStatus.Damaged; // fix as need to actively add input function to PlayerInput when defense is spawned
            target = null;
        }
    }

    public void FixBuilding()
    {
        if (PlayerInventory.scrap >= Config.buildingFixScrapCost)
        {
            buildStatus = BuildStatus.Built;
            PlayerInventory.scrap -= Config.buildingFixScrapCost;
        }
    }

    #region input functions

    public void QuickInteract(InputAction.CallbackContext con)
    {
        if (con.performed)
        {
            quickInteract = true;
        }
    }

    #endregion input functions
}
