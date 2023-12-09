using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Range // can add more later if needed 
{
    Melee = 5,
    Short = 10,
    Medium = 20,
    Long = 30
}

[CreateAssetMenu]
public class GunStatsSO : ScriptableObject
{
    [Header("----- Stats -----")]
    public new string name;
    public int damage, magSize, ammoCount, magAmmoCount;
    public float fireRate;
    public Range gunRange;

    [Header("----- assets -----")]
    public Texture image; // image for UI
    public GameObject bullet, model;
    public ParticleSystem muzzleEffect, hitEffect;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootVol;
}
