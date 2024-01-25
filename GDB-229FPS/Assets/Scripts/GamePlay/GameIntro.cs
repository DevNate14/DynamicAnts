using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameIntro : MonoBehaviour
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBar;
    [SerializeField] TMP_Text loadingText;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;

    string[] messages = { "Hello", "You must be new here.", "Let me explain...", 
        "Although, maybe you aren't new here.", "It gets hard to remember.", "Or maybe you just don't want an explanation.", "Go ahead and press [ESCAPE] if so.", "   ", 
        "The most simple explanation is that I have a mission for you.", "Run through the simulation and test out my latest inventions.", 
        "There'll be weapons and ammo for you to pick up along the way.", "You'll need them if you want to survive.", 
        "There are pick-ups that will restore your HP to help you survive and checkpoints if you can't.",
        "Some parts of the simulation are more realistic than others.", "For example, objects you can interact with or break,", "as well as objects that can interact with and break you.",
        "I have three main areas for you to explore,", "which you can move between freely.",
        "Any questions?", "If so, I'm sure you'll figure it out after a few iterations.", "Don't worry, you'll do great here... eventually.", "Welcome to Ultra-Life", "Press [ESCAPE] to continue"};

    bool isPlayingMessage;
    int messageCurr = 0;

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            //AudioManager.instance.PlayMusic(1);
            loadingScreen.SetActive(true);
            StartCoroutine(LoadAsyncScene(1));
        }

        if(!isPlayingMessage && messageCurr < messages.Length)
        {
            StartCoroutine(TypeText(messages[messageCurr]));
        }
    }

    IEnumerator LoadAsyncScene(int sceneNumber)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNumber);
        asyncLoad.allowSceneActivation = false;
        bool completed = false;

        asyncLoad.completed += (AsyncOperation op) =>
        {
            completed = true;
            //loadingScreen.SetActive(false);
        };

        while (!asyncLoad.isDone) //Progress UI
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Scales Progress to 0-1
            loadingBar.fillAmount = progress;

            if (progress >= 1F)
            {
                loadingText.text = "100";
            }

            else
            {
                loadingText.text = $"{(int)(progress * 100)}"; //Shows 0-100%
            }

            if (asyncLoad.progress >= 0.9f && !completed)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator TypeText(string line)
    {
        isPlayingMessage = true;
        dialogueText.text = ""; //Clears Text
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(1F / 8.5f);
        }

        yield return new WaitForSeconds(1F);

        if (messageCurr != messages.Length - 1)
        { 
            dialogueText.text = "";
            isPlayingMessage = false;
            messageCurr++;
        }
    }

}
