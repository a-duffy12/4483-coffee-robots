using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class MainMenu : MonoBehaviour
{
    public Texture2D cursor;

    [Header("GameObjects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private TMP_Text difficultyText;
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text tutorialsText;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text winsText;
    [SerializeField] private TMP_Text splashText;

    [Header("Audio")]
    public AudioClip succeedAudio;
    public AudioClip failAudio;
    AudioSource source;

    PlayerInput input;
    private int demoCount;
    private List<string> splashTexts = new List<string> {
        "Press Q to reduce incoming damage",
        "Check out BOOM!",
        "CS4483 >>> sleep",
        "The Machete sucks no cap",
        "#*@! Brawlers",
        "Click 'Normal' 5 times for Demo mode!",
        "Players get 2x resources from kills",
        "Players deal 4x damage to Whales",
        "Purple healthbar = ENRAGED",
        "ENRAGED = 1.5x damage and speed",
        "3.14159265358979323846264338327950288419716939937510",
        "Sleep Protection is a must",
        "'Alt Damage I' one-shots Sentinels!",
        "Quick swap to deal more DPS",
        "Phantoms are invisible to Defenses",
        "Spikes are underrated",
        "Hard gives a higher score!"
    };
 
    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    void Start()
    {
        Config.GetSaveData();
        Config.ResetConfigValues();
        Time.timeScale = 0f;
        input.SwitchCurrentActionMap("Menu");
        pauseMenu.SetActive(false);
        titleMenu.SetActive(true);
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";
        currentScoreText.text = $"Current Score: {Config.currentScore.ToString()}";
        highScoreText.text = $"High Score: {Config.highScore.ToString()}";
        winsText.text = $"Wins: {Config.winCount.ToString()}";
        Cursor.SetCursor(cursor, new Vector2(650, 650), CursorMode.Auto);

        if (Config.showFps)
        {
            fpsText.text = "Enabled";
        }
        else
        {
            fpsText.text = "Disabled";
        }

        if (Config.showEnemyCount)
        {
            enemyCountText.text = "Enabled";
        }
        else
        {
            enemyCountText.text = "Disabled";
        }

        if (Config.showTutorials)
        {
            tutorialsText.text = "Enabled";
        }
        else
        {
            tutorialsText.text = "Disabled";
        }

        UpdateSplashText();
    }

    public void Easy()
    {
        Config.difficultyLevel = Difficulty.Easy;
        Config.difficultyDamageMod = 0.5f;
        Config.difficultyMovementMod = 0.8f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";

        demoCount = 0;
        PlayAudio(succeedAudio);
    }

    public void Normal()
    {
        Config.difficultyLevel = Difficulty.Normal;
        Config.difficultyDamageMod = 1f;
        Config.difficultyMovementMod = 1f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";

        demoCount++;

        if (demoCount >= 5) // hidden demo difficulty
        {
            Config.difficultyLevel = Difficulty.Demo;
            Config.difficultyDamageMod = 0.1f;
            Config.difficultyMovementMod = 1f;
            difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";
        }

        PlayAudio(succeedAudio);
    }

    public void Hard()
    {
        Config.difficultyLevel = Difficulty.Hard;
        Config.difficultyDamageMod = 1.5f;
        Config.difficultyMovementMod = 1.2f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";

        demoCount = 0;
        PlayAudio(succeedAudio);
    }

    public void Fps()
    {
        if (Config.showFps)
        {
            PlayAudio(failAudio);
            Config.showFps = false;
            fpsText.text = "Disabled";
        }
        else
        {
            PlayAudio(succeedAudio);
            Config.showFps = true;
            fpsText.text = "Enabled";
        }
    }

    public void EnemyCount()
    {
        if (Config.showEnemyCount)
        {
            PlayAudio(failAudio);
            Config.showEnemyCount = false;
            enemyCountText.text = "Disabled";
        }
        else
        {
            PlayAudio(succeedAudio);
            Config.showEnemyCount = true;
            enemyCountText.text = "Enabled";
        }
    }

    public void Tutorials()
    {
        if (Config.showTutorials)
        {
            PlayAudio(failAudio);
            Config.showTutorials = false;
            tutorialsText.text = "Disabled";
        }
        else
        {
            PlayAudio(succeedAudio);
            Config.showTutorials = true;
            tutorialsText.text = "Enabled";
        }
    }

    public void OpenPauseMenu()
    {
        if (!pauseMenu.activeSelf)
        {
            PlayAudio(succeedAudio);
            pauseMenu.SetActive(true);
            titleMenu.SetActive(false);

            currentScoreText.text = $"Current Score: {Config.currentScore.ToString()}";
            highScoreText.text = $"High Score: {Config.highScore.ToString()}";
            winsText.text = $"Wins: {Config.winCount.ToString()}";

            demoCount = 0;
        }
    }

    public void ClosePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            PlayAudio(failAudio);
            SaveLoad.SaveData();
            pauseMenu.SetActive(false);
            titleMenu.SetActive(true);

            UpdateSplashText();
        }
    }

    public void PlayGame()
    {
        PlayAudio(succeedAudio);
        Time.timeScale = 1f;
        input.SwitchCurrentActionMap("Player");
        SceneManager.LoadScene("Valley"); // can change this for multiple maps
    }

    public void QuitGame()
    {
        PlayAudio(failAudio);
        SaveLoad.SaveData();
        Application.Quit();
    }

    void PlayAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }

    void UpdateSplashText()
    {
        splashText.text = splashTexts[UnityEngine.Random.Range(0, splashTexts.Count)];
    }
}
