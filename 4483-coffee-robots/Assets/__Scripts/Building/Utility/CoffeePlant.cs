using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [Header("Tutorials")]
    [SerializeField] private GameObject stage1Intro;
    [SerializeField] private GameObject stage2Intro;
    [SerializeField] private GameObject stage3Intro;
    [SerializeField] private GameObject stage4Intro;

    [Header("Audio")]
    public AudioClip succeedAudio;
    public AudioClip failAudio;
    AudioSource source;

    [HideInInspector] public GameObject player;
    PlayerInput input;
    [HideInInspector] public MachineShop machineShop;
    [HideInInspector] public Armory armory;
    [HideInInspector] public Fabricator fabricator;
    [HideInInspector] public CoffeeMachine coffeeMachine;
    private bool startStage;
    private bool activeStageDone;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        input = player.GetComponent<PlayerInput>();
        machineShop = GameObject.FindGameObjectWithTag("MachineShop").GetComponent<MachineShop>();
        armory = GameObject.FindGameObjectWithTag("Armory").GetComponent<Armory>();
        fabricator = GameObject.FindGameObjectWithTag("Fabricator").GetComponent<Fabricator>();
        coffeeMachine = GameObject.FindGameObjectWithTag("CoffeeMachine").GetComponent<CoffeeMachine>();
        source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }
    
    void Start()
    {
        currentHp = Config.coffeePlantMaxHp;

        stage1Intro.SetActive(false);
        stage2Intro.SetActive(false);
        stage3Intro.SetActive(false);
        stage4Intro.SetActive(false);

        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
        lowHpOverlay.SetActive(false);

        healthBar.fillAmount = Mathf.Clamp(currentHp/Config.coffeePlantMaxHp, 0, Config.coffeePlantMaxHp);
        progressBarObj.SetActive(false);

        Time.timeScale = 1f;
        input.SwitchCurrentActionMap("Player");
        Config.gameStage = 0;

        Config.ResetConfigValues(); // reset unlocks and upgrades
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

        if (distanceToPlayer <= Config.buildingInteractDistance)
        {
            if (Config.gameStage == 0)
            {
                promptText.text = "Plant Coffee Seed [E]";

                if (startStage)
                {
                    Stage1Intro();
                }
            }
            else if (Config.gameStage == 1 && Time.time > Config.stage1Time + Config.stage1Duration)
            {
                promptText.text = "Start Stage 2 [E]";

                if (startStage)
                {
                    Stage2Intro();
                }

                if (!activeStageDone)
                {
                    activeStageDone = true;
                    Config.currentScore += Mathf.FloorToInt(250 * Config.scoreMod);
                }
            }
            else if (Config.gameStage == 2 && Time.time > Config.stage2Time + Config.stage2Duration)
            {
                promptText.text = "Start Stage 3 [E]";

                if (startStage)
                {
                    Stage3Intro();
                }

                if (!activeStageDone)
                {
                    activeStageDone = true;
                    Config.currentScore += Mathf.FloorToInt(1000 * Config.scoreMod);
                }
            }
            else if (Config.gameStage == 3 && Time.time > Config.stage3Time + Config.stage3Duration && !Config.coffeeMachineBuilt)
            {
                promptText.text = "Requires Coffee Machine";
            }
            else if (Config.gameStage == 3 && Time.time > Config.stage3Time + Config.stage3Duration && Config.coffeeMachineBuilt)
            {
                promptText.text = "Start Stage 4 [E]";

                if (startStage)
                {
                    Stage4Intro();
                }

                if (!activeStageDone)
                {
                    activeStageDone = true;
                    Config.currentScore += Mathf.FloorToInt(3000 * Config.scoreMod);
                }
            }
            else if (Config.gameStage == 4 && Time.time > Config.stage4Time + Config.stage4Duration && Config.coffeeMachineBuilt)
            {
                promptText.text = "Drink Coffee [E]";

                if (startStage)
                {
                    StartCoroutine(WinGame());
                }

                if (!activeStageDone)
                {
                    activeStageDone = true;
                    Config.currentScore += Mathf.FloorToInt(7500 * Config.scoreMod);
                }
            }
            else
            {
                if (promptText.text.StartsWith("Plant") || promptText.text.StartsWith("Start Stage")  || promptText.text.StartsWith("Requires Coffee Machine") || promptText.text.StartsWith("Drink Coffee"))
                {
                    promptText.text = ""; // only sets to empty if player just left text radius
                }
            }
        }
        else if (promptText.text.StartsWith("Plant") || promptText.text.StartsWith("Start Stage")  || promptText.text.StartsWith("Requires Coffee Machine") || promptText.text.StartsWith("Drink Coffee"))
        {
            promptText.text = ""; // only sets to empty if player just left text radius
        }

        startStage = false;
    }

    void Stage1Intro()
    {
        stage1Intro.SetActive(true);
        input.SwitchCurrentActionMap("Menu");
    }

    void Stage2Intro()
    {
        stage2Intro.SetActive(true);
        input.SwitchCurrentActionMap("Menu");
    }

    void Stage3Intro()
    {
        stage3Intro.SetActive(true);
        input.SwitchCurrentActionMap("Menu");
    }

    void Stage4Intro()
    {
        stage4Intro.SetActive(true);
        input.SwitchCurrentActionMap("Menu");
    }
    
    public void Stage1Start()
    {
        Config.gameStage = 1;
        Config.stage1Time = Time.time;
        input.SwitchCurrentActionMap("Player");
        stage1Intro.SetActive(false);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);

        machineShop.Unlock();
        armory.Unlock();
        fabricator.Unlock();

        if (Config.difficultyLevel == Difficulty.Easy)
        {
            Config.scoreMod = 0.5f;
        }
        else if (Config.difficultyLevel == Difficulty.Normal)
        {
            Config.scoreMod = 1f;
        }
        else if (Config.difficultyLevel == Difficulty.Hard)
        {
            Config.scoreMod = 1.5f;
        }
        else if (Config.difficultyLevel == Difficulty.Demo)
        {
            Config.scoreMod = 0.25f;
        }

        activeStageDone = false;
        PlayAudio(succeedAudio);
    }

    public void Stage2Start()
    {
        Config.gameStage = 2;
        Config.stage2Time = Time.time;
        input.SwitchCurrentActionMap("Player");
        stage2Intro.SetActive(false);
        stage2Prefab.SetActive(true);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(false);
        buildingCanvas.transform.position += new Vector3(0, 0.5f, 0);

        activeStageDone = false;
        PlayAudio(succeedAudio);
    }

    public void Stage3Start()
    {
        Config.gameStage = 3;
        Config.stage3Time = Time.time;
        input.SwitchCurrentActionMap("Player");
        stage3Intro.SetActive(false);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(true);
        stage4Prefab.SetActive(false);
        buildingCanvas.transform.position += new Vector3(0, 1.25f, 0);

        coffeeMachine.Unlock();
        
        activeStageDone = false;
        PlayAudio(succeedAudio);
    }

    public void Stage4Start()
    {
        Config.gameStage = 4;
        Config.stage4Time = Time.time;
        input.SwitchCurrentActionMap("Player");
        stage4Intro.SetActive(false);
        stage2Prefab.SetActive(false);
        stage3Prefab.SetActive(false);
        stage4Prefab.SetActive(true);
        buildingCanvas.transform.position += new Vector3(0, 1.25f, 0);

        activeStageDone = false;
        PlayAudio(succeedAudio);
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
        damage = (float)Mathf.FloorToInt(damage);
        
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
        input.SwitchCurrentActionMap("Menu");
        promptText.text = "Congratulations!";
        Time.timeScale = 0.0001f;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies) // destroy all remaining enemies
        {
            Destroy(enemy);
        }

        GameObject[] ienemies = GameObject.FindGameObjectsWithTag("InvisibleEnemy");
        foreach (GameObject ienemy in ienemies) // destroy all remaining invisible enemies
        {
            Destroy(ienemy);
        }

        Config.currentScore += Mathf.FloorToInt(currentHp * 5 * Config.scoreMod);
        Config.currentScore += Mathf.FloorToInt(PlayerInventory.scrap * Config.scoreMod);
        Config.currentScore += Mathf.FloorToInt(PlayerInventory.electronics * Config.scoreMod);
        Config.currentScore += Mathf.FloorToInt(PlayerInventory.tech * Config.scoreMod);
        Config.UpdateHighScore();
        
        Config.winCount++;
        SaveLoad.SaveData();
        
        // win audio

        yield return new WaitForSeconds(0.0005f);

        Config.ResetConfigValues();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    void LoseGame()
    {
        input.SwitchCurrentActionMap("Menu");
        promptText.text = "The coffee has been lost...";
        PlayerSystem system = player.GetComponent<PlayerSystem>();
        system.DamagePlayer(Config.playerMaxHp * 10, "coffee_plant");
    }

    void PlayAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
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
