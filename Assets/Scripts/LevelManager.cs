using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject endUI;
    [SerializeField] private GameObject calibrationUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject looper;
    [field: SerializeField] public float CraftOpenRange { get; private set; } = 3.0f;
    
    private  PlayerInput playerInput;
    private bool isPaused = false;
    private bool isCraft = false;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        gameOverlay.SetActive(true);
        deathUI.SetActive(false);
        menuUI.SetActive(false);
        craftUI.SetActive(false);

        playerInput = GetComponent<PlayerInput>();
    }
    public void ReloadCurentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void OnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0.0f : 1.0f;
        menuUI.SetActive(isPaused);
        var targetMap = isCraft ? "Craft" : "Player";
        playerInput.SwitchCurrentActionMap(isPaused ? "Menu" : targetMap);
    }
    public void OnCalibration()
    {
        Time.timeScale = 1.0f;
        menuUI.SetActive(false);
        calibrationUI.SetActive(true);
        // SwitchMenuInput();
    }
    public void OnCraft()
    {
        if (!isCraft && CheckPlayerFar())
            return;

        isCraft = !isCraft;
        craftUI.SetActive(isCraft);
        playerInput.SwitchCurrentActionMap(isCraft ? "Craft" : "Player");
    }

    bool CheckPlayerFar()
    {
        var playerControl = player.GetComponent<PlayerController>();
        return playerControl.GetDistanceToHarmonoid() > CraftOpenRange;
    }
    public void LoadDeathMenu()
    {
        gameOverlay.SetActive(false);
        looper.SetActive(false);
        deathUI.SetActive(true);
    }
    public void LoadEndMenu()
    {
        gameOverlay.SetActive(false);
        looper.SetActive(false);
        endUI.SetActive(true);
        playerInput.SwitchCurrentActionMap("Menu");
    }
}
