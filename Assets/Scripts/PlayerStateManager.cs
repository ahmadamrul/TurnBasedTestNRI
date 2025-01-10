using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        ChoosingAction,
        ChoosingTarget,
        PlayerAction,
        ExecutePlayerAction,
        Dead
    }
    private enum PlayerActionState
    {
        Attack,
        Defend,
        ApplySkill
    }

    private Player player;
    public List<StatusEffectData> playerSkills;
    public bool isDefending = false;
    public PlayerState currentState = PlayerState.Idle;
    private PlayerActionState playerActionState;
    public GameObject playerContainer;

    public int selectedTargetIndex;
    public int selectedSkillIndex;

    private PlayerAnimation anim;

    private void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        player = GetComponent<Player>();
    }
    public void StartTurn()
    {
        currentState = PlayerState.ChoosingAction;
        BattleSystemManager.Instance.textBattle.text = "<color=#0003F1>Player Turn";
        isDefending = false;
        anim.PlayIsDefending(isDefending);
    }
    public void SelectTarget(GameObject target)
    {
        selectedTargetIndex = BattleSystemManager.Instance.enemies.IndexOf(target.GetComponent<Enemy>());
        if (selectedTargetIndex != -1)
        {
            if (playerActionState == PlayerActionState.Attack)
            {
                StartCoroutine(PlayerAttack());
            }
            else if (playerActionState == PlayerActionState.ApplySkill)
            {
                StartCoroutine(OnApplySkill(selectedSkillIndex, false));
            }
        }
    }
    public void UpdateState()
    {
        switch (currentState)
        {
            case PlayerState.ChoosingAction:
                playerContainer.SetActive(true);
                break;
            case PlayerState.ChoosingTarget:

                break;
            case PlayerState.PlayerAction:
                if (playerActionState == PlayerActionState.Attack)
                {
                    StartCoroutine(PlayerAttack());
                }
                else if (playerActionState == PlayerActionState.Defend)
                {
                    StartCoroutine(PlayerDefend());
                }
                break;
            case PlayerState.ExecutePlayerAction:
                playerContainer.SetActive(false);
                break;
        }
    }
    public void OnAttackButton()
    {
        if (currentState != PlayerState.ChoosingAction)
        {
            return;
        }
        BattleSystemManager.Instance.textBattle.text = "<color=#0003F1>Select an enemy to attack";
        playerContainer.SetActive(false);
        playerActionState = PlayerActionState.Attack;
        currentState = PlayerState.ChoosingTarget;
    }
    public void OnDefendButton()
    {
        if (currentState != PlayerState.ChoosingAction)
        {
            return;
        }
        playerActionState = PlayerActionState.Defend;
        currentState = PlayerState.PlayerAction;
    }
    private IEnumerator PlayerAttack()
    {
        currentState = PlayerState.ExecutePlayerAction;

        Enemy targetEnemy = BattleSystemManager.Instance.enemies[selectedTargetIndex];
        EnemyStateManager targetEnemyStateManager = BattleSystemManager.Instance.enemyStateManagers[selectedTargetIndex];

        player.UpdateStats();
        BattleSystemManager.Instance.textBattle.text = "<color=#0003F1>Player Attacking " + targetEnemy.nameUnit;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetEnemy.transform.position;

        Vector3 directionToTarget = (targetPosition - startPosition).normalized;
        float stopDistance = 2f;
        targetPosition -= directionToTarget * stopDistance;


        float elapsedTime = 0;
        float duration = 1f;
        float speed = 2f;

        // Simpan rotasi awal pemain
        Quaternion startRotation = transform.rotation;

        anim.PlayAttacking();
        AnimatorStateInfo stateInfo = anim.anim.GetCurrentAnimatorStateInfo(0);

        // Arahkan pemain ke target
        transform.LookAt(targetPosition);
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;

            yield return null;
        }
        yield return new WaitForSeconds(0.5f);
        elapsedTime = 0;
        if (SfxManager.instance != null)
        {
            SfxManager.instance.SlashPlayer();
        }
        bool isEnemyAlive = targetEnemyStateManager.TakeDamage(player.damage);
        yield return new WaitForSeconds(1f);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            yield return null;
        }

        if (!isEnemyAlive)
        {
            // Cek apakah semua musuh telah dikalahkan
            bool allEnemiesDefeated = true;
            foreach (var enemy in BattleSystemManager.Instance.enemies)
            {
                if (enemy.health > 0)
                {
                    allEnemiesDefeated = false;
                    break;
                }
            }
            if (allEnemiesDefeated)
            {
                BattleSystemManager.Instance.currentGameState = BattleSystemManager.GameState.PlayerWin;
                currentState = PlayerState.Idle;
                yield break;
            }
        }

        // Kembalikan rotasi awal pemain
        transform.rotation = startRotation;
        StartCoroutine(EndTurn());
    }

    private IEnumerator PlayerDefend()
    {
        currentState = PlayerState.ExecutePlayerAction;
        BattleSystemManager.Instance.textBattle.text = "<color=#0003F1>Player Defend";
        isDefending = true;
        yield return new WaitForSeconds(0.5f);
        anim.PlayIsDefending(isDefending);
        StartCoroutine(EndTurn());
    }
    public bool TakeDamage(int damage)
    {
        if (isDefending)
        {
            Debug.Log("Isdefending");
            damage -= player.defense;
            if (damage <= 0)
            {
                damage = 0;
            }
        }
        else if (!isDefending)
        {
            anim.PlayHit();
        }
        //        Debug.Log("Take Damage" + damage);
        bool isAlive = player.TakeDamage(damage);
        if (!isAlive)
        {
            currentState = PlayerState.Dead;
        }
        player.UpdateStats();
        return isAlive;
    }
    public void OnApplySkillButton(int skillIndex)
    {
        StatusEffectData skillData = playerSkills[skillIndex]; if (skillData.applyToPlayer)
        { // Langsung terapkan skill ke pemain 
            StartCoroutine(OnApplySkill(skillIndex, true));
        }
        else
        { // Masuk ke mode pemilihan target 
            playerContainer.SetActive(false);
            BattleSystemManager.Instance.textBattle.text = "<color=#0003F1>Select an enemy to attack";
            playerActionState = PlayerActionState.ApplySkill;
            selectedSkillIndex = skillIndex;
            currentState = PlayerState.ChoosingTarget;
        }
    }

    public IEnumerator OnApplySkill(int skillIndex, bool applyToPlayer)
    {
        if (!applyToPlayer && currentState != PlayerState.ChoosingTarget)
            yield break;
        anim.PlayIsCasting();
        int currentTurn = BattleSystemManager.Instance.turnCount;

        if (skillIndex < playerSkills.Count)
        {
            StatusEffect skillEffect = new StatusEffect(playerSkills[skillIndex], currentTurn);
            if (applyToPlayer)
            {
                BattleSystemManager.Instance.player.AddStatusEffect(skillEffect);
                BattleSystemManager.Instance.textBattle.text = "<color=#0003F1>Player using skill: " + playerSkills[skillIndex].skillName.ToString();
                player.UpdateStats();
            }
            else
            {
                Enemy targetEnemy = BattleSystemManager.Instance.enemies[selectedTargetIndex];
                targetEnemy.AddStatusEffect(skillEffect);
                BattleSystemManager.Instance.textBattle.text = "<color=#0003F1>Player using skill on : " + targetEnemy.nameUnit + " " + playerSkills[skillIndex].skillName.ToString();
                player.UpdateStats();
            }
            yield return new WaitForSeconds(0.5f);
            currentState = PlayerState.ExecutePlayerAction;
            StartCoroutine(EndTurn());
        }
        else
        {
            Debug.LogError("Invalid skill index.");
        }
    }
    private IEnumerator EndTurn()
    {

        yield return new WaitForSeconds(2f);
        currentState = PlayerState.Idle;
        BattleSystemManager.Instance.EndPlayerTurn();
    }

}



