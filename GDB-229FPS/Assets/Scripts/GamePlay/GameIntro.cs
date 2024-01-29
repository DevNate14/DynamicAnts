using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameIntro : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBar;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] GameObject loadingReady;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField] Image[] marks;

    string[] messages = { "Hello, you must be new here.", "Let me explain...", 
        "Although, maybe you aren't new here. It gets hard to remember.", "Or maybe you just don't want an explanation.", "If so, go ahead and press [ESCAPE].", 
        "The most simple explanation is that I have a mission for you.", "Run through the simulation and test out my latest inventions.", 
        "There'll be weapons and ammo for you to pick up along the way.", "You'll need them if you want to survive.", 
        "There are also pick-ups that will restore your HP", "and checkpoints if you don't survive.",
        "Some parts of the simulation are more realistic than others.", "For example, objects you can interact with or break,", "as well as objects that can interact with and break you.",
        "I have three main areas for you to explore,", "which you can move between freely.", "Make sure to find all the secrets I've hidden for you.", 
        "Any questions?", "If so, I'm sure you'll figure it out after a few iterations.", "Don't worry, you'll do great here... eventually.", "Welcome to Ultra-Life", "Press [ESCAPE] to continue"};

    bool isPlayingMessage;
    int messageCurr = 0;
    int loadTime;
    bool isLoading = false;
    private void Start()
    {
        for(int i = 0; i < marks.Length && i < PlayerPrefs.GetInt("times", 0); i++)
        {
            marks[i].enabled = true;
        }
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel") && !isLoading)
        {
            loadTime = 0;
            AudioManager.instance.PlayMusic(1);
            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsyncScene(1));
        }

        if(!isPlayingMessage && messageCurr < messages.Length)
        {
            StartCoroutine(TypeText(messages[messageCurr]));
        }

        if (isLoading && Input.anyKeyDown)
        {
            loadTime = 10;
        }
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
            //loadingScreen.SetActive(false);
        };

        while (!asyncLoad.isDone) //Progress UI
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Scales Progress to 0-1
            progress = Mathf.Clamp(progress, 0, loadTime / 10f);
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
                asyncLoad.allowSceneActivation = true;
            }
            else if (asyncLoad.progress >= 0.9f && !completed)
            {
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
    }

}
