using System.Collections;
using TMPro;

//using UnityEditor; this makes the editor unable to build a project please dont include/"use" these in the project
//using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public RawImage[] weaponImages;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] TMP_Text playButton;
    //public Vector3 rotationAxis= Vector3.up; //Changes Axis
    //public bool isSpinning = false;

    void Start()
    {
        StartSpinning(); //Rotates Images
        //AudioManager.instance.PlayMusic(0, false);
        GetComponentInChildren<Button>().Select();
        playButton.text = PersistenceManager.instance.savedGameExists ? "Continue Game" : "New Game";
        AudioManager.instance.PlayMusic(0);
    }

    void StartSpinning()
    {
        foreach (RawImage weaponImage in weaponImages)
        {
            StartCoroutine(SpinWeaponImages(weaponImage));
        }
    }

    IEnumerator SpinWeaponImages(RawImage weaponImage)
    {

        Vector3 randomRotationAxis = Random.onUnitSphere;
        //Random Axis

        while (true)
        {
            weaponImage.rectTransform.Rotate(randomRotationAxis,
            rotationSpeed * Time.deltaTime);

            yield return null;
            // Waits for 2nd Scene
        }
    }
}
