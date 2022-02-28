using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void ControlScene()
    {
        SceneManager.LoadScene("Controls");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
