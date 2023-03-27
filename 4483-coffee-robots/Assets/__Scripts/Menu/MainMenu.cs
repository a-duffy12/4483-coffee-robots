using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Texture2D cursor;

    [Header("GameObjects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private TMP_Text difficultyText;
    [SerializeField] private TMP_Text currentScoreText;
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text winsText;

    PlayerInput input;
    private int demoCount;
 
    void Awake()
    {
        input = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
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
    }

    public void Easy()
    {
        Config.difficultyLevel = Difficulty.Easy;
        Config.difficultyDamageMod = 0.5f;
        Config.difficultyMovementMod = 0.8f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";

        demoCount = 0;
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
    }

    public void Hard()
    {
        Config.difficultyLevel = Difficulty.Hard;
        Config.difficultyDamageMod = 1.5f;
        Config.difficultyMovementMod = 1.2f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";

        demoCount = 0;
    }

    public void OpenPauseMenu()
    {
        if (!pauseMenu.activeSelf)
        {
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
            SaveLoad.SaveData();
            pauseMenu.SetActive(false);
            titleMenu.SetActive(true);
        }
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        input.SwitchCurrentActionMap("Player");
        SceneManager.LoadScene("Valley"); // can change this for multiple maps
    }

    public void QuitGame()
    {
        SaveLoad.SaveData();
        Application.Quit();
    }
}
