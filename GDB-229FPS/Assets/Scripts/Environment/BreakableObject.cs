using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable, IPersist
{
    [SerializeField] int MaxHP;
    int CurrentHP;
    public bool Regen; //public just in case we want a regen disabling mechanic
    public float RegenWaitTime; //time after taking damage until regeneration starts
    public float RegenCD; // time between regeneration ticks
    float TimeLeft; //tells code how long it's been since previous two started
    public ParticleSystem BreakFX;
    public AudioSource Aud;
    public AudioClip Sound;
    void Start()
    {
        CurrentHP = MaxHP;
        AddToPersistenceManager();
        LoadState();
    }
    void Update()
    {
        if (Regen)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0)
            {
                Heal(1);
                TimeLeft = RegenCD;
            }
        }
    }
    public void Damage(int dmg)
    {
        CurrentHP -= dmg;
        if (Regen)
        {
            TimeLeft = RegenWaitTime;
        }
        if (CurrentHP <= 0)
        {
            Break();
        }
    }
    public void Heal(int hp)
    {
        if (CurrentHP < MaxHP)
        {
            if (MaxHP - hp > CurrentHP) // just so hp doesnt overflow
            {
                CurrentHP += hp;
            }
        }
    }
    public virtual void Break() //public just in case we want something able to instantly kill them
    {
        // maybe i wait until theres an animation for this
    }
    public void AddToPersistenceManager()
    {
        PersistenceManager.instance.AddToManager(this);
    }
    public void SaveState()
    {
        PlayerPrefs.SetInt(this.gameObject.GetInstanceID().ToString() + "CurrHP", CurrentHP);
    }
    public void LoadState()
    {
        Damage(MaxHP - PlayerPrefs.GetInt(this.gameObject.GetInstanceID().ToString() + "CurrHP", MaxHP));
    }
}
