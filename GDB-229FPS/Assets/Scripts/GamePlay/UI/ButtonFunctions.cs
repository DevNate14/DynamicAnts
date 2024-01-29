using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] AudioClip buttonSFX;
    int loadTime;

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            loadTime = 10;
        }
    }

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

    public void Delete()
    {
        float[] volSettings = { PlayerPrefs.GetFloat("GameVol"), PlayerPrefs.GetFloat("MusicVol"), PlayerPrefs.GetFloat("SFXVol") };
        int mouseSensitivity = PlayerPrefs.GetInt("MouseSensitivity", 300);
        int invertY = PlayerPrefs.GetInt("InvertY", 0);
        int times = PlayerPrefs.GetInt("times", 0);

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("SavedGameExists", 0);

        PlayerPrefs.SetFloat("GameVol", volSettings[0]);
        PlayerPrefs.SetFloat("MusicVol", volSettings[1]);
        PlayerPrefs.SetFloat("SFXVol", volSettings[2]);
        PlayerPrefs.SetInt("MouseSensitivity", mouseSensitivity);
        PlayerPrefs.SetInt("InvertY", invertY);
        PlayerPrefs.SetInt("times", times);

        PlayerPrefs.SetFloat("SpawnPosX", -262);
        PlayerPrefs.SetFloat("SpawnPosY", 10);
        PlayerPrefs.SetFloat("SpawnPosZ", 78);
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
        else
        {
            PlayerPrefs.SetInt("times", PlayerPrefs.GetInt("times", -1) + 1);
        }
        AudioManager.instance.PlayMusic(sceneNumber);
        PlayerPrefs.SetInt("SavedGameExists", 1);
        GameManager.instance.loadingScreen.SetActive(true);
        GameManager.instance.loadingReady.SetActive(false);
        loadTime = 0;
        StartCoroutine(LoadAsyncScene(sceneNumber));
    }

    IEnumerator LoadAsyncScene(int sceneNumber)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);
        asyncLoad.allowSceneActivation = false;
        bool completed = false;
            asyncLoad.completed += (AsyncOperation op) =>
        {
            completed = true;
        };

        while (!asyncLoad.isDone) //Progress UI
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Scales Progress to 0-1
            progress = Mathf.Clamp(progress, 0, loadTime / 9f);
            GameManager.instance.loadingBar.fillAmount = progress;

            if (progress >= 1F)
            {
                GameManager.instance.loadingText.text = "100";
            }
            else
            {
                GameManager.instance.loadingText.text = $"{(int)(progress * 100)}"; //Shows 0-100%
            }

            if (asyncLoad.progress >= 0.9f && !completed && loadTime >= 10)
            {
                asyncLoad.allowSceneActivation = true;
            }
            else if(asyncLoad.progress >= 0.9f && !completed)
            {
                GameManager.instance.loadingReady.SetActive(true);
            }
            loadTime++;
            yield return new WaitForSeconds(0.3f);
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
        if(SceneManager.GetActiveScene().buildIndex == sceneNumber)
        {
            SceneManager.LoadScene(sceneNumber);
        }
        AudioManager.instance.PlayMusic(sceneNumber);
        GameManager.instance.loadingScreen.SetActive(true);
        loadTime = 0;
        StartCoroutine(LoadAsyncScene(sceneNumber));
    }


    public void CreditsScene()
{
    int creditsSceneNumber = 4;

    ButtonSound();
    
    if (GameManager.instance.isPaused)
    {
        GameManager.instance.StateUnpaused();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    AudioManager.instance.PlayMusic(creditsSceneNumber);
    GameManager.instance.loadingScreen.SetActive(true);
    loadTime = 0;
    StartCoroutine(LoadAsyncScene(creditsSceneNumber));
}
}