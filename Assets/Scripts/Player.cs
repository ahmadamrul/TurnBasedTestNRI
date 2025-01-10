using TMPro;
using UnityEngine;

public class Player : Unit
{


    private void Start()
    {
        playerStats.text = "Health = " + health + "\nDamage = " + damage + "\nDefense = " + defense;
        UpdateBuffText();
    }

}