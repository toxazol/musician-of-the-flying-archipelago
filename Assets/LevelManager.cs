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
    }
    public void OnCalibration()
    {
        Time.timeScale = 1.0f;
        menuUI.SetActive(false);
        calibrationUI.SetActive(true);
    }
    public void OnCraft()
    {
        if (!isCraft && player.GetComponent<PlayerController>().GetDistanceToHarmonoid() > 3.0f)
            return;

        isCraft = !isCraft;
        craftUI.SetActive(isCraft);
    }
    public void LoadDeathMenu()
    {
        gameOverlay.SetActive(false);
        deathUI.SetActive(true);
    }
    public void LoadEndMenu()
    {
        gameOverlay.SetActive(false);
        endUI.SetActive(true);
    }
}
