using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.SceneManagement;
using TMPro;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] Vector3 NextStartPos;
    [SerializeField] int NextSceneNumber;

    [SerializeField] GameObject LoadingPage;

    int loadingCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.playerScript.playerSpawnPos = NextStartPos;
            PersistenceManager.instance.sceneNumber = NextSceneNumber;

            PersistenceManager.instance.SaveGame();

            Instantiate(LoadingPage);

            StartCoroutine(LoadAsyncScene());
        }
    }
    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(NextSceneNumber);

        while (!asyncLoad.isDone)
        {
            if(loadingCount % 3 == 0)
            {
                LoadingPage.GetComponentInChildren<TMP_Text>().text = "LOADING";
            }
            else
            {
                LoadingPage.GetComponentInChildren<TMP_Text>().text += ".";
            }

            loadingCount++;

            yield return null;
        }
    }
}
