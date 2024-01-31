using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneSwap : MonoBehaviour
{
 public void playGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void goRules()
    {
        SceneManager.LoadScene("Help");
    }

    public void goBack()
    {
        SceneManager.LoadScene("Menu");
    }
    public void quitGame()
    {
        Application.Quit();
        Debug.Log("Quit!");
    }
}
