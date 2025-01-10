
using System.Collections;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public Enemy enemy;
    private EnemyAnimation enemyAnimation;
    public enum EnemyState
    {
        Idle,
        ChoosingAction,
        ExecutingAction,
        Dead
    }
    private bool isDefending;
    public EnemyState currentState = EnemyState.Idle;
    public GameObject shieldEnemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        enemyAnimation = GetComponent<EnemyAnimation>();
        shieldEnemy.SetActive(false);
    }
    public void StartTurn()
    {
        BattleSystemManager.Instance.textBattle.text = "<color=#FF0000>" + enemy.nameUnit + " Turn";

        StartCoroutine(WaitTurnEnemy());

    }
    private IEnumerator WaitTurnEnemy()
    {
        yield return new WaitForSeconds(1f);
        isDefending = false;
        shieldEnemy.SetActive(isDefending);
        currentState = EnemyState.ChoosingAction;

    }
    public void UpdateState()
    {
        switch (currentState)
        {
            case EnemyState.ChoosingAction:
                RandomAction();
                break;

            case EnemyState.ExecutingAction:
                break;
            case EnemyState.Dead:
                break;
        }
    }
    private void RandomAction()
    {
        currentState = EnemyState.ExecutingAction;
        int action = Random.Range(1, 3);
        if (action == 1)
        {
            StartCoroutine(AttackAction());
        }
        else if (action == 2)
        {
            StartCoroutine(DefendAction());
        }

    }
    private IEnumerator AttackAction()
    {
        if (enemy.isDead)
        {
            currentState = EnemyState.Dead; // Set ke state Dead jika musuh sudah mati 
            yield break;
        }
        BattleSystemManager.Instance.player.UpdateStats();
        Vector3 startPosition = transform.position;

        BattleSystemManager.Instance.textBattle.text = "<color=#FF0000>" + enemy.nameUnit + " Attack";
        Vector3 targetPosition = BattleSystemManager.Instance.playerStateManager.transform.position;
        targetPosition.z += 1.5f;
        float elapsedTime = 0;
        float duration = 1f;
        float speed = 2f;
        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;
            enemyAnimation.PlayAttacking();
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);
        elapsedTime = 0;
        if (SfxManager.instance != null)
        {
            SfxManager.instance.EnemyAttack();
        }
        bool isPlayerAlive = BattleSystemManager.Instance.playerStateManager.TakeDamage(enemy.damage);

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(targetPosition, startPosition, elapsedTime);
            elapsedTime += Time.deltaTime * speed;

            yield return null;
        }

        if (!isPlayerAlive)
        {
            BattleSystemManager.Instance.currentGameState = BattleSystemManager.GameState.EnemyWin;
            currentState = EnemyState.Idle;
        }
        StartCoroutine(EndTurn());
    }
    private IEnumerator DefendAction()
    {
        isDefending = true;
        shieldEnemy.SetActive(isDefending);
        BattleSystemManager.Instance.textBattle.text = "<color=#FF0000>" + enemy.nameUnit + " Defend";
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(EndTurn());
    }

    private IEnumerator EndTurn()
    {
        yield return new WaitForSeconds(2f);
        //Debug.Log("Enemy's turn ended.");
        currentState = EnemyState.Idle;
        BattleSystemManager.Instance.EndEnemyTurn();
    }


    public bool TakeDamage(int damage)
    {


        if (isDefending)
        {
            damage -= enemy.defense;
            if (damage <= 0)
            {
                damage = 0;
            }
        }
        if (damage > 0)
        {
            enemyAnimation.PlayGetAttack();
        }
        bool isAlive = enemy.TakeDamage(damage);
        if (!isAlive)
        {
            currentState = EnemyState.Dead;

            //Debug.Log("Enemy died during Player's turn.");
        }

        BattleSystemManager.Instance.player.UpdateStats();
        return isAlive;
    }

}
