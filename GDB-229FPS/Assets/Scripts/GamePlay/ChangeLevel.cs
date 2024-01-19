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

    int loadingCount;

    private void Start()
    {
        LoadingPage = GameManager.instance.LoadingScreen;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.playerSpawnPos = NextStartPos;
            PersistenceManager.instance.sceneNumber = NextSceneNumber;

            PersistenceManager.instance.SaveGame();

            LoadingPage.SetActive(true);

            StartCoroutine(LoadAsyncScene());
        }
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextSceneNumber);

        do
        {
            asyncLoad.allowSceneActivation = loadingCount >= 4;

            LoadingPage.GetComponentInChildren<Slider>().value = Mathf.Clamp(asyncLoad.progress, 0, loadingCount/4f);

            if (loadingCount % 4 == 0)
            {
                LoadingPage.GetComponentInChildren<TMP_Text>().text = "LOADING";
            }
            else
            {
                LoadingPage.GetComponentInChildren<TMP_Text>().text += ".";
            }

            loadingCount++;

            yield return new WaitForSeconds(0.25f);

        } while (!asyncLoad.isDone);

        AudioManager.instance.PlayMusic(NextSceneNumber);
        LoadingPage.SetActive(false);
    }

}
