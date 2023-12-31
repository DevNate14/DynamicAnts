using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public RawImage[] weaponImages;
    [SerializeField] float rotationSpeed = 100f;
    //public Vector3 rotationAxis= Vector3.up; //Changes Axis
    //public bool isSpinning = false;

    void Start()
    {
        StartSpinning(); //Rotates Images
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
