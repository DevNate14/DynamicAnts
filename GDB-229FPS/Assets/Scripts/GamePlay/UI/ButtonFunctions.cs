using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonFunctions : MonoBehaviour
{
    public void Resume()
    {
        GameManager.instance.StateUnpaused();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.StateUnpaused();
    }

    public void Respawn() 
    {
        GameManager.instance.playerScript.RespawnPlayer();
        GameManager.instance.StateUnpaused();
    }

    public void Quit()
    {
        Application.Quit();
    }

   
}