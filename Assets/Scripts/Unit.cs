using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public string nameUnit;
    public int health;
    public int damage;
    public int defense;
    public GameObject damagePopupPrefab;

    protected List<StatusEffect> activeEffects = new List<StatusEffect>();

    public TextMeshProUGUI playerStats;
    public TextMeshProUGUI buffText;
    public TextMeshProUGUI nameText;

    public GameObject enemyHealthBarGameObject;
    public GameObject playerHealthBarGameObject;

    public bool isDead = false;

    public GameObject debuffVfx;
    public GameObject buffVfx;
    public GameObject bleedingVfx;

    public GameObject uiBuff;
    public GameObject uiBleeding;
    public GameObject uiDebuff;
    private void Start()
    {
        if (bleedingVfx != null)
        {
            bleedingVfx.SetActive(false);
        }
        if (buffVfx != null)
        {
            buffVfx.SetActive(false);
        }
        if (debuffVfx != null)
        {
            debuffVfx.SetActive(false);
        }
        if (uiBuff != null)
        {
            uiBuff.SetActive(false);
        }
        if (uiBleeding != null)
        {
            uiBleeding.SetActive(false);
        }
        if (uiDebuff != null)
        {
            uiDebuff.SetActive(false);
        }

    }
    public void UpdateStats()
    {
        playerStats.text = "Health = " + health + "\nDamage = " + damage + "\nDefense = " + defense;
        UpdateBuffText();
    }

    public void UpdateBuffText()
    {
        buffText.text = "Active Buffs:\n";
        int currentTurn = BattleSystemManager.Instance.turnCount - 1;

        foreach (var effect in activeEffects)
        {
            int remainingTurns;

            if (effect.applyNextTurn)
            {
                if (currentTurn <= effect.endTurn - effect.duration)
                {
                    // Giliran pertama atau sebelum, durasi tidak berkurang
                    remainingTurns = effect.duration;
                }
                else
                {
                    // Giliran-giliran berikutnya, durasi berkurang
                    remainingTurns = effect.endTurn - currentTurn;
                }
            }
            else
            {
                remainingTurns = effect.endTurn - currentTurn;
            }

            // Hanya tampilkan jika remainingTurns lebih dari 0
            if (remainingTurns > 0)
            {
                buffText.text += $"{effect.type} ({remainingTurns} turns left)\n";
            }
        }
    }


    public bool TakeDamage(int damage, bool damageAfterTurn = false)
    {
        health -= damage;
        ShowDamagePopup(damage);
        Enemy.UpdateAllEnemyStats();
        if (enemyHealthBarGameObject != null)
        {
            enemyHealthBarGameObject.GetComponent<EnemyHealthUI>().UpdateHealthUI();
        }
        if (playerHealthBarGameObject != null)
        {
            playerHealthBarGameObject.GetComponent<PlayerHealthUI>().UpdateHealthUI();
        }
        if (health <= 0)
        {
            health = 0; isDead = true; Debug.Log($"{nameUnit} is dead.");
            if (damageAfterTurn)
            { // Hanya menandai musuh sebagai mati jika kerusakan karena bleeding 
                return false;
            }
            else
            { // Proses penghapusan musuh yang mati karena serangan biasa
                if (this is Enemy enemy)
                {
                    BattleSystemManager.Instance.RemoveEnemy(enemy);
                    Destroy(enemy.gameObject);
                    Destroy(playerStats);
                    Destroy(buffText);
                    Destroy(nameText);
                }
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    private void ShowDamagePopup(int damageAmount)
    {
        if (damagePopupPrefab != null && gameObject != null)
        {
            Vector3 offset = new Vector3(0, 1, 0);
            Vector3 spawnPosition = transform.position + offset;
            var popup = Instantiate(damagePopupPrefab, spawnPosition, Quaternion.identity);
            popup.GetComponent<DamagePopup>().Setup(damageAmount);
        }
    }

    public void AddStatusEffect(StatusEffect effect)
    {
        var existingEffect = activeEffects.Find(e => e.type == effect.type);
        if (existingEffect != null)
        {
            // Jika efek sudah ada, tambahkan durasinya
            existingEffect.endTurn += effect.duration;
            Debug.Log($"{nameUnit} already has {effect.type}, extending duration by {effect.duration} turns.");
        }

        else
        {
            // Jika efek belum ada, tambahkan efek baru
            activeEffects.Add(effect);
            ApplyImmediateEffect(effect);
            Debug.Log($"{nameUnit} is affected by {effect.type} for {effect.duration} turns.");
        }
        UpdateBuffText();

        if (effect.type == StatusEffect.EffectType.Bleeding)
        {
            if (uiBleeding != null)
            {
                uiBleeding.SetActive(true);
            }
        }
        //        Debug.Log("Buuf");
        if (effect.type == StatusEffect.EffectType.IncreaseDamage)
        {
            //  Debug.Log("Buuf");
            if (uiBuff != null)
            {
                uiBuff.SetActive(true);
            }
        }
        if (effect.type == StatusEffect.EffectType.ReduceDefense)
        {
            if (uiBleeding != null)
            {
                uiDebuff.SetActive(true);
            }
        }
        if (debuffVfx != null)
        {
            StartCoroutine(PlayDebuffVfx());
        }
        Enemy.UpdateAllEnemyStats();
    }

    private void ApplyImmediateEffect(StatusEffect effect)
    {
        // Terapkan efek langsung (jika ada yang membutuhkan aplikasi awal)
        switch (effect.type)
        {
            case StatusEffect.EffectType.IncreaseDamage:
                damage += effect.value;
                if (buffVfx != null)
                {
                    StartCoroutine(PlayBuffVfx());
                }

                break;
            case StatusEffect.EffectType.ReduceDefense:
                defense -= effect.value;

                break;
        }
        // Panggil UpdateAllEnemyStats untuk memperbarui semua musuh
        Enemy.UpdateAllEnemyStats();
    }

    private void RemoveEffect(StatusEffect effect)
    {
        // Hapus efek setelah selesai
        switch (effect.type)
        {
            case StatusEffect.EffectType.IncreaseDamage:
                damage -= effect.value;
                break;
            case StatusEffect.EffectType.ReduceDefense:
                defense += effect.value;
                break;
        }
        if (effect.type == StatusEffect.EffectType.Bleeding)
        {
            if (uiBleeding != null)
            {
                uiBleeding.SetActive(false);
            }
        }
        if (effect.type == StatusEffect.EffectType.IncreaseDamage)
        {
            if (uiBuff != null)
            {
                uiBuff.SetActive(false);
            }
        }
        if (effect.type == StatusEffect.EffectType.ReduceDefense)
        {
            if (uiBleeding != null)
            {
                uiDebuff.SetActive(false);
            }
        }

        Debug.Log($"{gameObject.name} effect remove {effect.type}.");
        // Panggil UpdateAllEnemyStats untuk memperbarui semua musuh
        Enemy.UpdateAllEnemyStats();
    }

    public void ProcessTurnEffects(int globalTurn)
    {
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            var effect = activeEffects[i];
            if (effect.type == StatusEffect.EffectType.Bleeding)
            {
                TakeDamage(effect.value, true); // Berikan damage bleeding 
                Debug.Log($"{gameObject.name} took {effect.value} damage due to bleeding.");
                if (SfxManager.instance != null)
                {
                    SfxManager.instance.EnemyDamaged();
                }

                if (bleedingVfx != null)
                {
                    StartCoroutine(PlayBleedingVfx());
                }
            }
            if (globalTurn >= effect.endTurn)
            {
                RemoveEffect(effect);
                activeEffects.RemoveAt(i);
            }
        }

        if (isDead)
        {
            // Musuh ditandai mati, tapi tidak langsung dihancurkan
            GetComponent<EnemyStateManager>().currentState = EnemyStateManager.EnemyState.Dead;
        }
    }
    public IEnumerator PlayBleedingVfx()
    {
        bleedingVfx.SetActive(true);
        yield return new WaitForSeconds(2f);
        bleedingVfx.SetActive(false);
    }
    public IEnumerator PlayBuffVfx()
    {
        buffVfx.SetActive(true);
        if (SfxManager.instance != null)
        {
            SfxManager.instance.SpellCastingBuff();
        }
        yield return new WaitForSeconds(2f);
        buffVfx.SetActive(false);
    }
    public IEnumerator PlayDebuffVfx()
    {
        debuffVfx.SetActive(true);
        if (SfxManager.instance != null)
        {
            SfxManager.instance.SpellCastingDebuff();
        }

        yield return new WaitForSeconds(2f);
        debuffVfx.SetActive(false);
    }
}

