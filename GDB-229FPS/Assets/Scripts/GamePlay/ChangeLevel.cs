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

    int loadTime;

    bool isLoading = false;

    private void Update()
    {
        if (Input.anyKeyDown && isLoading)
        {
            loadTime = 10;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            loadTime = 0;

            GameManager.instance.playerScript.playerSpawnPos = NextStartPos;
            PersistenceManager.instance.sceneNumber = NextSceneNumber;

            PersistenceManager.instance.SaveGame();

            PlayerPrefs.SetFloat("SpawnPosX", NextStartPos.x);
            PlayerPrefs.SetFloat("SpawnPosY", NextStartPos.y);
            PlayerPrefs.SetFloat("SpawnPosZ", NextStartPos.z);

            AudioManager.instance.PlayMusic(NextSceneNumber);
            GameManager.instance.loadingScreen.SetActive(true);
            StartCoroutine(LoadAsyncScene(NextSceneNumber));
        }
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
            progress = Mathf.Clamp(progress, 0, loadTime / 10f);
            GameManager.instance.loadingBar.fillAmount = progress;

            if (progress >= 1F)
            {
                GameManager.instance.loadingText.text = "100%";
            }

            else
            {
                GameManager.instance.loadingText.text = $"{(int)(progress * 100)}%"; //Shows 0-100%
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
            yield return null;
        }

        //return null;
    }

}
