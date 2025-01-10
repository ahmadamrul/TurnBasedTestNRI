using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager instance;
    [SerializeField] private AudioClip slashPlayer;
    [SerializeField] private AudioClip enemyAttack;
    [SerializeField] private AudioClip spellCastingBuff;
    [SerializeField] private AudioClip spellCastingDebuff;
    [SerializeField] private AudioClip enemyDamaged;

    [SerializeField] private AudioClip bowSound;
    [SerializeField] private AudioClip arrowHitSound;

    public AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        audioSource = GetComponent<AudioSource>();
    }

    public void SlashPlayer()
    {
        if (slashPlayer != null)
        {
            audioSource.PlayOneShot(slashPlayer);
        }
    }

    public void EnemyAttack()
    {
        if (enemyAttack != null)
        {
            audioSource.PlayOneShot(enemyAttack);
        }
    }

    public void SpellCastingBuff()
    {
        if (spellCastingBuff != null)
        {
            audioSource.PlayOneShot(spellCastingBuff);
        }
    }
    public void PlayHitBulletSound()
    {
        if (enemyDamaged != null)
        {
            audioSource.PlayOneShot(enemyDamaged);
        }
    }

    public void SpellCastingDebuff()
    {
        if (spellCastingDebuff != null)
        {
            audioSource.PlayOneShot(spellCastingDebuff);
        }
    }
    public void EnemyDamaged()
    {
        if (enemyDamaged != null)
        {
            audioSource.PlayOneShot(enemyDamaged);
        }
    }

    public void UpdateVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }
}
