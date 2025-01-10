using UnityEngine;

public class CheatManager : MonoBehaviour
{
    Player player;
    public GameObject openBtn;
    public GameObject closeBtn;
    public GameObject CheatContainer;
    string playerTag = "Player";
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Player>();
    }

    public void OneKillHit()
    {
        player.damage = 10000;
        player.UpdateStats();
    }
    public void HealtUnlimited()
    {
        player.health = 10000;
        player.UpdateStats();

    }

    public void OpenCheat()
    {
        openBtn.SetActive(false);
        closeBtn.SetActive(true);
        CheatContainer.SetActive(true);
    }
    public void CloseCheat()
    {
        openBtn.SetActive(true);
        closeBtn.SetActive(false);
        CheatContainer.SetActive(false);
    }
}
