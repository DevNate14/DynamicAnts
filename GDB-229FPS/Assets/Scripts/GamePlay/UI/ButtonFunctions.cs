using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] AudioClip buttonSFX;

    public void ButtonSound()
    {
        AudioManager.instance.PlaySFX(buttonSFX);
    }

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

    //------------------------- TITLE PAGE ---------------------------------------------

    public void Play(int sceneNumber)
    {
        //if(PersistenceManager.instance.savedGameExists)
        //{
        //    sceneNumber = PersistenceManager.instance.sceneNumber;
        //}

        SceneManager.LoadScene(sceneNumber);
    }

    public void MainMenu(int sceneNumber)
    {
        if (GameManager.instance.isPaused) {
            GameManager.instance.StateUnpaused();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        SceneManager.LoadScene(sceneNumber);
    }
}