using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private GameObject gameOverlay;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject player;

    private PlayerInput playerInput;
    private bool isPaused = false;
    private bool isCraft = false;

    // Start is called before the first frame update
    void Start()
    {
        gameOverlay.SetActive(true);
        deathUI.SetActive(false);
        menuUI.SetActive(false);

        playerInput = player.GetComponent<PlayerInput>();
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
        playerInput.enabled = !isPaused;
        menuUI.SetActive(isPaused);
    }
    public void OnCraftMenu()
    {
        isCraft = !isCraft;
        playerInput.enabled = !isCraft;
        menuUI.SetActive(isCraft);
    }
    public void LoadDeathMenu()
    {
        gameOverlay.SetActive(false);
        deathUI.SetActive(true);
    }
}
