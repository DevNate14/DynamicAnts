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
        ButtonSound();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameManager.instance.StateUnpaused();
        ButtonSound();
    }

    public void ShowDeleteWarning(int on)
    {
        GameManager.instance.deleteWarning.SetActive(on == 1);
        ButtonSound();
    }

    public void Respawn()
    {
        GameManager.instance.playerScript.RespawnPlayer();
        GameManager.instance.StateUnpaused();
        ButtonSound();
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
        AudioManager.instance.PlayMusic(sceneNumber);
        PlayerPrefs.SetInt("SavedGameExists", 1);
        StartCoroutine(LoadAsyncScene(sceneNumber));
    }

    IEnumerator LoadAsyncScene(int sceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);
        asyncLoad.allowSceneActivation = false;
        bool completed = false;

            asyncLoad.completed += (AsyncOperation op) =>
        {
            // Do something after Loading
            //Debug.Log("Completed loading of the new scene");
            completed = true;
            GameManager.instance.loadingScreen.SetActive(false);
        };

        while (!asyncLoad.isDone) //Progress UI
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Scales Progress to 0-1
            GameManager.instance.loadingBar.fillAmount = progress;

            if (progress >= 1F)
            {
                GameManager.instance.loadingText.text = "100%";
            }

            else
            {
                GameManager.instance.loadingText.text = $"{(int)(progress * 100)}%"; //Shows 0-100%
            }

            if (asyncLoad.progress >= 0.9f && !completed)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }

        //return null;
    }

    public void MainMenu(int sceneNumber)
    {
        ButtonSound();
        if (GameManager.instance.isPaused)
        {
            GameManager.instance.StateUnpaused();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        SceneManager.LoadScene(sceneNumber);
    }
}