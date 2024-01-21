using System.Collections;
using System.Threading;
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
        if (PersistenceManager.instance.savedGameExists)
        {
            sceneNumber = PersistenceManager.instance.sceneNumber;
        }

        StartCoroutine(LoadAsyncScene(sceneNumber));
    }

    IEnumerator LoadAsyncScene(int sceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);
        asyncLoad.allowSceneActivation = true; 
        bool completed = false;
        asyncLoad.completed += (AsyncOperation op)=>
        {
            // do something after completed loading
            Debug.Log("Completed loading of the new scene");
            completed = true;
            GameManager.instance.loadingScreen.SetActive(false);
        };
        //int loadingCount = 0;

        // while(!completed)  // still loading
        // //Updates Loading Progress UI
        // {
        //     Debug.Log("Starting Progress Loop");
        //     float progress = Mathf.Clamp01(asyncLoad.progress);
        //     GameManager.instance.loadingBar.fillAmount = progress;
        //     //GameManager.instance.loadingText.text = progress.ToString();
        //     GameManager.instance.loadingText.text = $"{(int)(progress * 100)}%";
        //     Debug.Log("Progress is " + progress);
        //     yield return new WaitForSeconds(0.1F);
        // }
        // Debug.Log("Finished displaying progress.");
        // GameManager.instance.loadingScreen.SetActive(false);
        // yield return null;

        return null;
    }

    public void MainMenu(int sceneNumber)
    {
        if (GameManager.instance.isPaused)
        {
            GameManager.instance.StateUnpaused();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        SceneManager.LoadScene(sceneNumber);
    }
}