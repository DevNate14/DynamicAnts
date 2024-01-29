using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOutro : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBar;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] GameObject loadingReady;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Image[] marks;
    string[] messages = { "Hello, you must be new here.", "Let me explain...",
        "Although, maybe you aren't new here. It gets hard to remember."};

    bool isPlayingMessage;
    int messageCurr = 0;
    bool isDone = false;
    bool isLoading = false;
    int loadTime;
    float pitchOrig;
    bool isChangingPitch = false;
    private void Start()
    {
        for(int i = 0; i < marks.Length && i < PlayerPrefs.GetInt("times", 1); i++)
        {
            marks[i].enabled = true;
        }
        pitchOrig = AudioManager.instance.musicSource.pitch;
        Delete();
    }
    void Update()
    {
        if (isLoading && Input.anyKeyDown)
        {
            loadTime = 10;
        }

        if (Input.GetButtonDown("Cancel") && !isLoading)
        {
            loadTime = 0;
            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsyncScene(4));
        }

        if (!isPlayingMessage && messageCurr < messages.Length)
        {
            StartCoroutine(TypeText(messages[messageCurr]));
        }

        if(isDone && !isLoading)
        {
            loadTime = 0;
            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsyncScene(4));
        }
        else if (!isChangingPitch)
        {
            StartCoroutine(MusicPitch());
        }
    }
    IEnumerator MusicPitch()
    {
        isChangingPitch = true;

        yield return new WaitForSeconds(1);

        AudioManager.instance.musicSource.pitch /= 1.1f;
        isChangingPitch = false;
    }
    IEnumerator LoadAsyncScene(int sceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);
        asyncLoad.allowSceneActivation = false;
        bool completed = false;
        isLoading = true;

        asyncLoad.completed += (AsyncOperation op) =>
        {
            completed = true;
        };

        while (!asyncLoad.isDone) //Progress UI
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Scales Progress to 0-1
            progress = Mathf.Clamp(progress, 0, loadTime / 9f);
            loadingBar.fillAmount = progress;
            if (progress >= 1F)
            {
                loadingText.text = "100";
            }
            else
            {
                loadingText.text = $"{(int)(progress * 100)}"; //Shows 0-100%
            }
            if (asyncLoad.progress >= 0.9f && !completed && loadTime >= 10)
            {
                AudioManager.instance.musicSource.pitch = pitchOrig;
                asyncLoad.allowSceneActivation = true;
            }
            else if (asyncLoad.progress >= 0.9f && !completed)
            {
                AudioManager.instance.musicSource.pitch = pitchOrig;
                AudioManager.instance.PlayMusic(4, false);
                loadingReady.SetActive(true);
            }
            loadTime++;
            yield return new WaitForSeconds(0.3f);
        }
    }
    IEnumerator TypeText(string line)
    {
        isPlayingMessage = true;
        dialogueText.text = ""; //Clears Text
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(.6f / 8.5f);
        }
        yield return new WaitForSeconds(1.5f);
        if (messageCurr != messages.Length - 1)
        { 
            dialogueText.text = "";
            isPlayingMessage = false;
            messageCurr++;
        }
        else
        {
            isDone = true;
        }
    }
    void Delete()
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
}
