using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.UI;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] Vector3 NextStartPos;
    [SerializeField] int NextSceneNumber;

    GameObject LoadingPage;

    AsyncOperation asyncLoad;

    int loadingCount;

    bool isLoading = false;

    private void Start()
    {
        LoadingPage = GameManager.instance.LoadingScreen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            loadingCount = 1;

            GameManager.instance.playerScript.playerSpawnPos = NextStartPos;
            PersistenceManager.instance.sceneNumber = NextSceneNumber;

            PersistenceManager.instance.SaveGame();

            LoadingPage.SetActive(true);
            StartCoroutine(LoadAsyncScene());
        }
    }

    IEnumerator LoadAsyncScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(NextSceneNumber);
       
        while (!asyncLoad.isDone)
        {
            asyncLoad.allowSceneActivation = loadingCount > 9;

            yield return new WaitForSecondsRealtime(0.3f);
            
            loadingCount++;

            LoadingPage.GetComponentInChildren<Slider>().value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
        }

        LoadingPage.SetActive(false);
    }

}
