using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public TextMeshPro damageText;
    public float lifeTime = 1f;
    public float moveSpeed = 10f;

    private void Start()
    {
        Destroy(gameObject, lifeTime); // Hancurkan setelah 3 detik
    }

    private void Update()
    { // Gerakan ke atas perlahan
        transform.position += new Vector3(0, moveSpeed * Time.deltaTime, 0);
    }

    public void Setup(int damageAmount)
    {
        damageText.text = "-" + damageAmount.ToString();
    }

    public void DestroyPopup()
    {
        Destroy(gameObject);
    }
}
