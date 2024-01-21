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

    GameObject loadingPage;
    TMP_Text loadingText;
    Image loadingBar;

    AsyncOperation asyncLoad;

    int loadingCount;

    bool isLoading = false;

    private void Start()
    {
        loadingPage = GameManager.instance.loadingScreen;
        loadingText = GameManager.instance.loadingText;
        loadingBar = GameManager.instance.loadingBar;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isLoading)
        {
            isLoading = true;
            loadingCount = 0;

            GameManager.instance.playerScript.playerSpawnPos = NextStartPos;
            PersistenceManager.instance.sceneNumber = NextSceneNumber;

            PersistenceManager.instance.SaveGame();

            StartCoroutine(LoadAsyncScene());
        }
    }


    IEnumerator LoadAsyncScene()
    {
        asyncLoad = SceneManager.LoadSceneAsync(NextSceneNumber);

        loadingPage.SetActive(true);

        while (!asyncLoad.isDone)
        {
            asyncLoad.allowSceneActivation = loadingCount > 3;
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            loadingBar.fillAmount = progress;
            loadingText.text = progress.ToString();

            yield return new WaitForSecondsRealtime(1f);
            loadingCount++;
        }

        isLoading = false;
        loadingPage.SetActive(false);
    }

}
