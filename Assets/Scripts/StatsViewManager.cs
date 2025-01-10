using UnityEngine;
using UnityEngine.UI;

public class StatsViewManager : MonoBehaviour
{
    public GameObject openStatsInfo;
    Button opentatsInfoBtn;
    public GameObject closeStatsInfo;
    Button closestatsInfoBtn;

    public GameObject statsInfo;
    void Start()
    {

        statsInfo.SetActive(false);
        opentatsInfoBtn = openStatsInfo.GetComponent<Button>();
        opentatsInfoBtn.onClick.AddListener(OpenStatsInfo);

        closestatsInfoBtn = closeStatsInfo.GetComponent<Button>();
        closestatsInfoBtn.onClick.AddListener(CloseStatsInfo);
    }
    void OpenStatsInfo()
    {
        openStatsInfo.SetActive(false);
        statsInfo.SetActive(true);

    }
    void CloseStatsInfo()
    {
        openStatsInfo.SetActive(true);
        statsInfo.SetActive(false);

    }
}
