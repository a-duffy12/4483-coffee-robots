using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class CoffeePlant : MonoBehaviour, IBuilding
{
    [HideInInspector] public float hp { get { return currentHp; } }
    private float currentHp;

    [Header("GameObjects")]
    [SerializeField] private GameObject stage2Prefab;
    [SerializeField] private GameObject stage3Prefab;
    [SerializeField] private GameObject stage4Prefab;
    [SerializeField] private GameObject lowHpOverlay;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private Image healthBar;
    [SerializeField] private Canvas buildingCanvas;
    [SerializeField] private Image progressBar;
    [SerializeField] private GameObject progressBarObj;

    [HideInInspector] public GameObject player;
    private bool startStage;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    void Start()
    {
        currentHp = Config.coffeePlantMaxHp;

        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
        lowHpOverlay.SetActive(false);

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.coffeePlantMaxHp, 0, Config.coffeePlantMaxHp);
        progressBarObj.SetActive(false);
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= Config.buildingCanvasDistance && currentHp < Config.coffeePlantMaxHp)
        {
            buildingCanvas.gameObject.SetActive(true);
        }
        else
        {
            buildingCanvas.gameObject.SetActive(false);
        }

        StageProgress(); // update game progress bar

        if (Config.gameStage == 4 && Time.time > Config.stage4Time + Config.stage4Duration)
        {
            StartCoroutine(WinGame());
        }

        if (distanceToPlayer <= Config.buildingInteractDistance)
        {
            if (Config.gameStage == 0)
            {
                promptText.text = "Plant Coffee Seed";

                if (startStage)
                {
                    Stage1Start();
                }
            }
            else if (Config.gameStage == 1 && Time.time > Config.stage1Time + Config.stage1Duration)
            {
                promptText.text = "Start Stage 2";

                if (startStage)
                {
                    Stage2Start();
                }
            }
            else if (Config.gameStage == 2 && Time.time > Config.stage2Time + Config.stage2Duration)
            {
                promptText.text = "Start Stage 3";

                if (startStage)
                {
                    Stage3Start();
                }
            }
            else if (Config.gameStage == 3 && Time.time > Config.stage3Time + Config.stage3Duration)
            {
                promptText.text = "Start Stage 4";

                if (startStage)
                {
                    Stage4Start();
                }
            }
            else
            {
                if (promptText.text == "Plant Coffee Seed" || promptText.text == "Start Stage 2" || promptText.text == "Start Stage 3" || promptText.text == "Start Stage 4")
                {
                    promptText.text = "";
                } 
            }
        }
        else
        {
            if (promptText.text == "Plant Coffee Seed" || promptText.text == "Start Stage 2" || promptText.text == "Start Stage 3" || promptText.text == "Start Stage 4")
            {
                promptText.text = "";
            }
        }
    }

    void Stage1Start()
    {
        Config.gameStage = 1;
        Config.stage1Time = Time.time;
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
        startStage = false;
    }

    void Stage2Start()
    {
        Config.gameStage = 2;
        Config.stage2Time = Time.time;
        stage2Prefab.SetActive(true);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
        startStage = false;
        buildingCanvas.transform.position += new Vector3(0, 0.5f, 0);
    }

    void Stage3Start()
    {
        Config.gameStage = 3;
        Config.stage3Time = Time.time;
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(true);
        stage4Prefab.SetActive(false);
        startStage = false;
        buildingCanvas.transform.position += new Vector3(0, 1.25f, 0);
    }

    void Stage4Start()
    {
        Config.gameStage = 4;
        Config.stage4Time = Time.time;
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(true);
        startStage = false;
        buildingCanvas.transform.position += new Vector3(0, 1.25f, 0);

        // will also need to confirm that the coffee machine has been built
    }

    void StageProgress()
    {
        if (Config.gameStage == 1)
        {
            if (!progressBarObj.activeSelf)
            {
                progressBarObj.SetActive(true);
            }

            progressBar.fillAmount = Mathf.Clamp((Time.time - Config.stage1Time)/Config.stage1Duration, 0, Config.stage1Duration);
        }
        else if (Config.gameStage == 2)
        {
            if (!progressBarObj.activeSelf)
            {
                progressBarObj.SetActive(true);
            }

            progressBar.fillAmount = Mathf.Clamp((Time.time - Config.stage2Time)/Config.stage2Duration, 0, Config.stage2Duration);
        }
        else if (Config.gameStage == 3)
        {
            if (!progressBarObj.activeSelf)
            {
                progressBarObj.SetActive(true);
            }

            progressBar.fillAmount = Mathf.Clamp((Time.time - Config.stage3Time)/Config.stage3Duration, 0, Config.stage3Duration);
        }
        else if (Config.gameStage == 4)
        {
            if (!progressBarObj.activeSelf)
            {
                progressBarObj.SetActive(true);
            }

            progressBar.fillAmount = Mathf.Clamp((Time.time - Config.stage4Time)/Config.stage4Duration, 0, Config.stage4Duration);
        }
        else if (Config.gameStage == 5)
        {
            if (progressBarObj.activeSelf)
            {
                progressBarObj.SetActive(false);
            }
        }
    }

    public void DamageBuilding(float damage, string source = "")
    {
        currentHp -= damage;
        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.coffeePlantMaxHp, 0, Config.coffeePlantMaxHp);

        if (currentHp > Config.coffeePlantMaxHp)
        {
            currentHp = Config.coffeePlantMaxHp;
        }
        
        if (currentHp <= Config.coffeePlantMaxHp * 0.2f)
        {
            lowHpOverlay.SetActive(true);
        }
        else
        {
            lowHpOverlay.SetActive(false);
        }

        if (currentHp <= 0)
        {
            LoseGame();
        }
    }

    IEnumerator WinGame()
    {
        Config.gameStage = 5;

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
        if (con.performed)
        {
            startStage = true;
        }
    }

    #endregion input functions
}
