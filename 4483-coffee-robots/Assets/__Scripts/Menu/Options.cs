using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Options : MonoBehaviour
{
    public Texture2D cursor;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text difficultyText;
    [SerializeField] private TMP_Text fpsText;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text winsText;
    [SerializeField] private TMP_Text fpsUI;
    [SerializeField] private TMP_Text enemyCountUI;

    [Header("Audio")]
    public AudioClip succeedAudio;
    public AudioClip failAudio;
    AudioSource source;

    PlayerInput input;
    private int demoCount;
    private float lastFpsDisplayUpdateTime;
    private float nextFpsDisplayUpdateTime;
    private int frameCount;
    private int enemyCount;

    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        source = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    void Start()
    {
        Config.GetSaveData();
        Config.ResetConfigValues();
        pauseMenu.SetActive(false);
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";
        currentScoreText.text = $"Current Score: {Config.currentScore.ToString()}";
        highScoreText.text = $"High Score: {Config.highScore.ToString()}";
        winsText.text = $"Wins: {Config.winCount.ToString()}";
        Cursor.SetCursor(cursor, new Vector2(650, 650), CursorMode.Auto);

        if (Config.showFps)
        {
            fpsText.text = "Enabled";
            fpsUI.gameObject.SetActive(true);
        }
        else
        {
            fpsText.text = "Disabled";
            fpsUI.gameObject.SetActive(false);
        }

        if (Config.showEnemyCount)
        {
            enemyCountText.text = "Enabled";
            enemyCountUI.gameObject.SetActive(true);
        }
        else
        {
            enemyCountText.text = "Disabled";
            enemyCountUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Config.showFps)
        {
            frameCount++;

            if (Time.time >= nextFpsDisplayUpdateTime)
            {
                int fps = (int)(frameCount / (Time.time - lastFpsDisplayUpdateTime));
                fpsUI.text = $"{fps.ToString()} FPS";
                frameCount = 0;
                lastFpsDisplayUpdateTime = Time.time;
                nextFpsDisplayUpdateTime = Time.time + 0.5f;
            }
        }

        if (Config.showEnemyCount && Config.gameStage >= 1 && Config.gameStage <= 4)
        {
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("InvisibleEnemy").Length;
            enemyCountUI.text = $"{enemyCount.ToString()} Enemies";
        }
        else
        {
            enemyCountUI.text = "";
        }
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
            fpsUI.gameObject.SetActive(false);
        }
        else
        {
            PlayAudio(succeedAudio);
            Config.showFps = true;
            fpsText.text = "Enabled";
            fpsUI.gameObject.SetActive(true);
        }
    }

    public void EnemyCount()
    {
        if (Config.showEnemyCount)
        {
            PlayAudio(failAudio);
            Config.showEnemyCount = false;
            enemyCountText.text = "Disabled";
            enemyCountUI.gameObject.SetActive(false);
        }
        else
        {
            PlayAudio(succeedAudio);
            Config.showEnemyCount = true;
            enemyCountText.text = "Enabled";
            enemyCountUI.gameObject.SetActive(true);
        }
    }

    public void OpenPauseMenu()
    {
        if (!pauseMenu.activeSelf)
        {
            PlayAudio(succeedAudio);
            pauseMenu.SetActive(true);
            input.SwitchCurrentActionMap("Menu");
            Time.timeScale = 0f;

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
            Time.timeScale = 1f;
            input.SwitchCurrentActionMap("Player");
            pauseMenu.SetActive(false);
            SaveLoad.SaveData();
        }
    }

    public void QuitToMainMenu()
    {
        PlayAudio(failAudio);
        SaveLoad.SaveData();
        SceneManager.LoadScene("Menu");
    }

    void PlayAudio(AudioClip clip)
    {
        source.clip = clip;
        source.Play();
    }
}
