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
        int loadingCount = 0;
        GameManager.instance.loadingScreen.SetActive(true);

        while (!asyncLoad.isDone)
        {
            //asyncLoad.allowSceneActivation = loadingCount > 3; 
            //Transition Happens too Quickly - May not Load
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9F);
            //Debug.Log("Loading Progress: " + progress);

            GameManager.instance.loadingBar.fillAmount = progress;
            GameManager.instance.loadingText.text = progress.ToString();

            yield return new WaitForSecondsRealtime(5F);
            loadingCount++;
        }

        GameManager.instance.loadingScreen.SetActive(false);

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