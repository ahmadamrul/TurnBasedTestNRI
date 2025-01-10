using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void PlayGetAttack()
    {
        if (anim != null)
        {
            anim.SetTrigger("Hit");
        }

    }
    public void PlayAttacking()
    {
        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }
    }
}
