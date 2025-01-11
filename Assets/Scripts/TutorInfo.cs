using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorInfo : MonoBehaviour
{
    public Button startBattle;

    public void Start()
    {
        startBattle.onClick.AddListener(LoadSceneBattle);
    }
    void LoadSceneBattle()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
