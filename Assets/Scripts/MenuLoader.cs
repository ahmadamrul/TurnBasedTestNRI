using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLoader : MonoBehaviour
{
    public Button roundTwo;
    public Button roundThree;
    public GameObject startBtn;
    public GameObject settingBtn;
    public GameObject exitBtn;
    public GameObject settingContainer;
    public GameObject levelSelectContainer;
    public GameObject canvasMenu;
    public GameObject closeBtn;

    void Start()
    {
        levelSelectContainer.SetActive(false);
        startBtn.SetActive(true);
        settingBtn.SetActive(true);
        exitBtn.SetActive(true);
        closeBtn.SetActive(false);


        settingContainer = FindInactiveObjectByName("SettingManager");

        if (settingContainer != null)
        {

            settingContainer.SetActive(false);
        }
        canvasMenu.SetActive(true);
        roundTwo.interactable = GameManager.instance.IsRound2Unlocked;
        roundThree.interactable = GameManager.instance.IsRound3Unlocked;
    }
    public void SceneLoaderName(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void StartButtonClicked()
    {
        levelSelectContainer.SetActive(true);
        startBtn.SetActive(false);
        settingBtn.SetActive(false);
        exitBtn.SetActive(false);

        closeBtn.SetActive(false);
    }
    public void SettingButtonClicked()
    {

        closeBtn.SetActive(true);
        levelSelectContainer.SetActive(false);
        startBtn.SetActive(false);
        settingBtn.SetActive(false);
        exitBtn.SetActive(false);
        canvasMenu.SetActive(false);

        settingContainer.SetActive(true);
    }
    GameObject FindInactiveObjectByName(string name)
    {
        Transform[] allObjects = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform obj in allObjects)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == name)
            {
                return obj.gameObject;
            }
        }
        return null;
    }
    public void SettingButtonClose()
    {

        closeBtn.SetActive(false);
        levelSelectContainer.SetActive(false);
        startBtn.SetActive(true);
        settingBtn.SetActive(true);
        exitBtn.SetActive(true);
        canvasMenu.SetActive(true);

        settingContainer.SetActive(false);
    }
    public void GoExit()
    {
        Application.Quit();
    }
}
