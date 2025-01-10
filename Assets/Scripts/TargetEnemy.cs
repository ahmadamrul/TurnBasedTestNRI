using TMPro;
using UnityEngine;

public class TargetEnemy : MonoBehaviour
{
    PlayerStateManager playerStateManager;
    public GameObject targetMark;
    public TextMeshPro nameText;
    private string playerTag = "Player";
    private void Start()
    {
        nameText.text = gameObject.GetComponent<Enemy>().nameUnit.ToString();
        if (playerStateManager == null)
        {
            playerStateManager = GameObject.FindGameObjectWithTag(playerTag).GetComponent<PlayerStateManager>();
        }
    }
    private void OnMouseEnter()
    {
        if (playerStateManager.currentState == PlayerStateManager.PlayerState.ChoosingTarget)
        {
            targetMark.SetActive(true);
        }

    }
    private void OnMouseExit()
    {
        targetMark.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (playerStateManager.currentState == PlayerStateManager.PlayerState.ChoosingTarget)
        {
            if (BattleSystemManager.Instance.currentGameState == BattleSystemManager.GameState.PlayerTurn)
            {
                BattleSystemManager.Instance.playerStateManager.SelectTarget(gameObject);
                targetMark.SetActive(false);
            }
        }
    }
}


