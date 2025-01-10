using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{

    public GameObject healthBar;
    private Slider healthSlider;
    public GameObject parent;
    Enemy enemyStateManager;

    void Start()
    {
        enemyStateManager = parent.GetComponent<Enemy>();
        healthSlider = healthBar.GetComponent<Slider>();
        int baseHP = enemyStateManager.health;
        healthSlider.maxValue = baseHP;
        healthSlider.value = enemyStateManager.health;

    }
    private void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(parent.transform.position + Vector3.up * 1);
    }
    public void UpdateHealthUI()
    {
        healthSlider.value = enemyStateManager.health;
        if (healthSlider.value <= 0)
        {
            Destroy(healthBar);
            //Destroy(gameObject); // Example enemy destruction
        }
    }
    void OnDestroy()
    {
        if (healthBar != null)
        {
            Destroy(healthBar);
        }
    }
}
