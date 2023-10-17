using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void NewGame()
    {
        SceneManager.LoadScene("World");
    }
    public void LoadGame()
    {
        // TODO
        SceneManager.LoadScene("World");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Settings()
    {
        // TODO
    }
}
