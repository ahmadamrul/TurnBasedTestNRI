using System.Collections;
using UnityEngine;

public class ShowRoundText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SowRound());
    }

    // Update is called once per frame
    IEnumerator SowRound()
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(gameObject);
    }
}
