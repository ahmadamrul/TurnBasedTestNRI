using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public GameObject healthBar;
    private Slider healthSlider;
    public GameObject parent;
    [SerializeField] Player player;

    void Start()
    {
        player = parent.GetComponent<Player>();
        healthSlider = healthBar.GetComponent<Slider>();
        int baseHP = player.health;

        healthSlider.maxValue = baseHP;
        healthSlider.value = player.health;

    }
    private void Update()
    {
        healthBar.transform.position = Camera.main.WorldToScreenPoint(parent.transform.position + Vector3.up * 2f);
    }
    public void UpdateHealthUI()
    {
        healthSlider.value = player.health;
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
