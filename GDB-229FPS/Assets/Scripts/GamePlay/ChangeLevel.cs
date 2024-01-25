using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] Vector3 NextStartPos;
    [SerializeField] int NextSceneNumber;

    //GameObject loadingPage;
    //TMP_Text loadingText;
    //Image loadingBar;

    //AsyncOperation asyncLoad;

    //int loadingCount;

    bool isLoading = false;

    //private void Start()
    //{
    //    loadingPage = GameManager.instance.loadingScreen;
    //    loadingText = GameManager.instance.loadingText;
    //    loadingBar = GameManager.instance.loadingBar;
    //}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            //loadingCount = 0;

            GameManager.instance.playerScript.playerSpawnPos = NextStartPos;
            PersistenceManager.instance.sceneNumber = NextSceneNumber;

            PersistenceManager.instance.SaveGame();

            PlayerPrefs.SetFloat("SpawnPosX", NextStartPos.x);
            PlayerPrefs.SetFloat("SpawnPosY", NextStartPos.y);
            PlayerPrefs.SetFloat("SpawnPosZ", NextStartPos.z);

            AudioManager.instance.PlayMusic(NextSceneNumber);
            StartCoroutine(LoadAsyncScene(NextSceneNumber));
        }
    }


    //IEnumerator LoadAsyncScene()
    //{
    //    asyncLoad = SceneManager.LoadSceneAsync(NextSceneNumber);

    //    loadingPage.SetActive(true);

    //    while (!asyncLoad.isDone)
    //    {
    //        asyncLoad.allowSceneActivation = loadingCount > 3;
    //        float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

    //        loadingBar.fillAmount = progress;
    //        loadingText.text = (progress*100).ToString("F0");

    //        yield return new WaitForSecondsRealtime(1f);
    //        loadingCount++;
    //    }

    //    isLoading = false;
    //    loadingPage.SetActive(false);
    //}

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

}
