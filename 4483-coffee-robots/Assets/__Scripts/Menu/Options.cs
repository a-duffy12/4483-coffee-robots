using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    public Texture2D cursor;
    
    [Header("GameObjects")]
    [SerializeField] private GameObject pauseMenu;
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
        pauseMenu.SetActive(false);
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
            Time.timeScale = 1f;
            input.SwitchCurrentActionMap("Player");
            pauseMenu.SetActive(false);
            SaveLoad.SaveData();
        }
    }

    public void QuitToMainMenu()
    {
        SaveLoad.SaveData();
        SceneManager.LoadScene("Menu");
    }
}
