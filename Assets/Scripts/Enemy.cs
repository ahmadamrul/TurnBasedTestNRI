using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class Enemy : Unit
{
    public static List<Enemy> allEnemies = new List<Enemy>();

    private void Start()
    {
        allEnemies.Add(this);
        UpdateStats();
        UpdateNameText();
    }

    private void OnDestroy()
    {
        allEnemies.Remove(this);
    }

    public void UpdateNameText()
    {
        nameText.text = nameUnit;
    }

    public static void UpdateAllEnemyStats()
    {
        foreach (var enemy in allEnemies)
        {
            enemy.UpdateStats();
        }
    }
}
