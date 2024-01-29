using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField] float Time;
    void Start()
    {
        Destroy(gameObject, Time);
    }
}
