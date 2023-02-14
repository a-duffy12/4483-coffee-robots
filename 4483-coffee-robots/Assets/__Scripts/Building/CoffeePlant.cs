using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CoffeePlant : MonoBehaviour, IBuilding
{
    [HideInInspector] public float hp { get { return currentHp; } }
    private float currentHp;

    [Header("GameObjects")]
    [SerializeField] private GameObject stage1Prefab;
    [SerializeField] private GameObject stage2Prefab;
    [SerializeField] private GameObject stage3Prefab;
    [SerializeField] private GameObject stage4Prefab;

    [HideInInspector] public GameObject player;
    private float stage1Time;
    private float stage2Time;
    private float stage3Time;
    private float stage4Time;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Start()
    {
        currentHp = Config.coffeePlantMaxHp;

        stage1Prefab.SetActive(true);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);

        // set up healthbar
    }

    void Update()
    {
        // only show healthbar if not full

        StageProgress(); // update game progress bar

        if (Config.gameStage == 4 && Time.time > stage4Time + Config.stage4Duration)
        {
            StartCoroutine(WinGame());
        }
    }

    void Stage1Start()
    {
        Config.gameStage = 1;
        stage1Time = Time.time;
        stage1Prefab.SetActive(true);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
    }

    void Stage2Start()
    {
        Config.gameStage = 2;
        stage2Time = Time.time;
        stage1Prefab.SetActive(true);
        stage2Prefab.SetActive(true);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
    }

    void Stage3Start()
    {
        Config.gameStage = 3;
        stage3Time = Time.time;
        stage1Prefab.SetActive(false);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(true);
        stage4Prefab.SetActive(false);
    }

    void Stage4Start()
    {
        Config.gameStage = 4;
        stage4Time = Time.time;
        stage1Prefab.SetActive(false);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(true);
        stage4Prefab.SetActive(false);
    }

    void StageProgress()
    {
        if (Config.gameStage == 0)
        {
            // do nothing
        }
        else if (Config.gameStage == 1)
        {

        }
        else if (Config.gameStage == 2)
        {

        }
        else if (Config.gameStage == 3)
        {

        }
        else if (Config.gameStage == 4)
        {

        }
        else if (Config.gameStage == 5)
        {

        }
    }

    public void DamageBuilding(float damage, string source = "")
    {
        currentHp -= damage;
        
        // update hp bar

        if (currentHp > Config.coffeePlantMaxHp)
        {
            currentHp = Config.coffeePlantMaxHp;
        }
        
        if (currentHp <= Config.coffeePlantMaxHp * 0.2f)
        {
            // low HP overlay
        }

        if (currentHp <= 0)
        {
            LoseGame();
        }
    }

    IEnumerator WinGame()
    {
         yield return new WaitForSeconds(0.0005f);

        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
    }

    void LoseGame()
    {
        PlayerSystem system = player.GetComponent<PlayerSystem>();
        system.DamagePlayer(Config.playerMaxHp * 2, "coffee_plant");
    }

    #region input functions

    public void ActivateStage(InputAction.CallbackContext con)
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (con.performed && distanceToPlayer <= Config.buildingInteractDistance)
        {
            if (Config.gameStage == 0)
            {
                Stage1Start();
            }
            else if (Config.gameStage == 1 && Time.time > stage1Time + Config.stage1Duration)
            {
                Stage2Start();
            }
            else if (Config.gameStage == 2 && Time.time > stage2Time + Config.stage2Duration)
            {
                Stage3Start();
            }
            else if (Config.gameStage == 3 && Time.time > stage3Time + Config.stage3Duration)
            {
                Stage4Start();
            }
        }
    }

    #endregion input functions
}
