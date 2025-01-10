using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public string nameScene;
    void Start()
    {
        StartCoroutine(LoadingSceneWait(nameScene));
    }

    private IEnumerator LoadingSceneWait(string nameScene)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(nameScene);
    }
}
