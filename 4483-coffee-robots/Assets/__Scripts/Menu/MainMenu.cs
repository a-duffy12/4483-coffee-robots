using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject titleMenu;
    [SerializeField] private TMP_Text difficultyText;

    PlayerInput input;
 
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
    }

    public void Easy()
    {
        Config.difficultyLevel = Difficulty.Easy;
        Config.difficultyDamageMod = 0.5f;
        Config.difficultyMovementMod = 0.8f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";
    }

    public void Normal()
    {
        Config.difficultyLevel = Difficulty.Normal;
        Config.difficultyDamageMod = 1f;
        Config.difficultyMovementMod = 1f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";
    }

    public void Hard()
    {
        Config.difficultyLevel = Difficulty.Hard;
        Config.difficultyDamageMod = 1.5f;
        Config.difficultyMovementMod = 1.2f;
        difficultyText.text = $"Current Difficulty: {Config.difficultyLevel.ToString()}";
    }

    public void OpenPauseMenu()
    {
        if (!pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(true);
            titleMenu.SetActive(false);
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
