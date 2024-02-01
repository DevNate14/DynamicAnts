using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class CreditsScreen : MonoBehaviour
{
    int loadTime;
    bool isLoading;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (!isLoading)
            {
                isLoading = true;
                AudioManager.instance.PlayMusic(0);
                GameManager.instance.loadingScreen.SetActive(true);
                loadTime = 0;
                StartCoroutine(LoadAsyncScene(0));
            }
        }
        if(isLoading && Input.anyKeyDown)
        {
            loadTime = 10;
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
            else if (asyncLoad.progress >= 0.9f && !completed)
            {
                GameManager.instance.loadingReady.SetActive(true);
            }
            loadTime++;
            yield return new WaitForSecondsRealtime(0.3f);
        }

        //return null;
    }
}
