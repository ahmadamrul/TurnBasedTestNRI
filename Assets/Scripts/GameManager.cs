using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private const string Round2Key = "isRound2Unlocked";
    private const string Round3Key = "isRound3Unlocked";
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public bool IsRound2Unlocked
    {
        get => PlayerPrefs.GetInt(Round2Key, 0) == 1;
        set => PlayerPrefs.SetInt(Round2Key, value ? 1 : 0);
    }
    public bool IsRound3Unlocked
    {
        get => PlayerPrefs.GetInt(Round3Key, 0) == 1;
        set => PlayerPrefs.SetInt(Round3Key, value ? 1 : 0);
    }

    public void SavePlayerPerf()
    {
        PlayerPrefs.Save();
    }
    private void OnApplicationQuit()
    {
        SavePlayerPerf();
    }
}
