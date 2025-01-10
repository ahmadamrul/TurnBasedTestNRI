using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleSystemManager : MonoBehaviour
{
    public static BattleSystemManager Instance;

    public enum GameState
    {
        PlayerTurn,
        EnemyTurn,
        PlayerWin,
        EnemyWin
    }

    public GameState currentGameState;
    public PlayerStateManager playerStateManager;
    public EnemyStateManager enemyStateManager;

    public Enemy enemy;
    public Player player;

    public int turnCount = 1;

    public TextMeshProUGUI textBattle;
    public TextMeshProUGUI countTextTurn;
    public GameObject enemyPrefab; // Prefab musuh 
    public Transform[] spawnPositions;
    public List<Enemy> enemies; // Buat daftar musuh
    public List<EnemyStateManager> enemyStateManagers = new List<EnemyStateManager>();

    [SerializeField] private int currentEnemyIndex = 0;
    [SerializeField] private int enemyAmount;
    public TextMeshProUGUI[] enemyStatsComponents;
    public TextMeshProUGUI[] enemyBuffComponents;
    public TextMeshProUGUI[] enemyNameComponents;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentGameState = GameState.PlayerTurn;
        countTextTurn.text = "Turn\n" + turnCount;
        playerStateManager.StartTurn();
        for (int i = 0; i < enemyAmount; i++)
        {
            if (i < spawnPositions.Length) // Pastikan posisi spawn ada 
            {
                Enemy newEnemy = Instantiate(enemyPrefab, spawnPositions[i].position, Quaternion.identity).GetComponent<Enemy>();
                EnemyStateManager enemyStateManager = newEnemy.GetComponent<EnemyStateManager>();

                enemies.Add(newEnemy);
                enemyStateManagers.Add(enemyStateManager);
                newEnemy.nameUnit = "ENEMY " + (i + 1);
                newEnemy.gameObject.name = "Enemy " + (i + 1);
                newEnemy.playerStats = enemyStatsComponents[i];
                newEnemy.buffText = enemyBuffComponents[i];
                newEnemy.nameText = enemyNameComponents[i];
                newEnemy.transform.rotation = spawnPositions[i].rotation;
            }
        }
    }

    private void Update()
    {
        switch (currentGameState)
        {
            case GameState.PlayerTurn:
                playerStateManager.UpdateState();

                //textBattle.text = "Player Turn";
                break;

            case GameState.EnemyTurn:
                foreach (var enemy in enemies)
                {
                    enemy.GetComponent<EnemyStateManager>().UpdateState();
                }
                //textBattle.text = "Enemy Turn";
                break;
            case GameState.PlayerWin:
                textBattle.text = "Player Win";
                PlayerWin();
                break;
            case GameState.EnemyWin:
                textBattle.text = "Player Lose";
                EnemyWin();
                break;
        }
    }

    public void EndPlayerTurn()
    {
        currentGameState = GameState.EnemyTurn;
        player.ProcessTurnEffects(turnCount);
        currentEnemyIndex = 0; enemies[currentEnemyIndex].GetComponent<EnemyStateManager>().StartTurn();
    }
    public void EndEnemyTurn()
    {
        // Daftar musuh yang akan dihapus di akhir giliran
        List<Enemy> enemiesToRemove = new List<Enemy>();

        if (currentEnemyIndex < enemies.Count - 1)
        {
            currentEnemyIndex++;
            enemies[currentEnemyIndex].GetComponent<EnemyStateManager>().StartTurn();
        }
        else
        {
            currentEnemyIndex = 0;
            currentGameState = GameState.PlayerTurn;
            foreach (var enemy in enemies)
            {
                enemy.ProcessTurnEffects(turnCount);
                // Tandai musuh yang mati untuk dihapus
                if (enemy.isDead)
                {
                    enemiesToRemove.Add(enemy);
                }
            }
            turnCount++;
            player.UpdateStats();
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().UpdateStats();
            }
            countTextTurn.text = "Turn\n" + turnCount;
            playerStateManager.StartTurn();
        }

        // Hapus musuh yang sudah mati setelah semua iterasi selesai
        foreach (var enemy in enemiesToRemove)
        {
            enemies.Remove(enemy);
            Destroy(enemy.gameObject);
            RemoveEnemy(enemy);
            Destroy(enemy.playerStats);
            Destroy(enemy.buffText);
            Destroy(enemy.nameText);
        }
        if (enemies.Count == 0)
        {
            currentGameState = GameState.PlayerWin;
        }
    }



    public void PlayerWin()
    {
        Debug.Log("Player Win");
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Current Scene: {sceneName}");
        if (sceneName == "BattleScene")
        {
            SceneManager.LoadScene("LoadingScene2");
            GameManager.instance.IsRound2Unlocked = true;
            GameManager.instance.SavePlayerPerf();
        }
        else if (sceneName == "BattleScene2")
        {
            SceneManager.LoadScene("LoadingScene3");
            GameManager.instance.IsRound3Unlocked = true;
            GameManager.instance.SavePlayerPerf();
        }
        else
        {
            Debug.LogError("Unknown scene name");
            SceneManager.LoadScene("Menu");
        }
    }
    public void EnemyWin()
    {
        Debug.Log("Enemy Win");
    }
    public void RemoveEnemy(Enemy enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
        EnemyStateManager enemyStateManager = enemy.GetComponent<EnemyStateManager>();
        if (enemyStateManagers.Contains(enemyStateManager))
        {
            enemyStateManagers.Remove(enemyStateManager);
        }
    }
}
