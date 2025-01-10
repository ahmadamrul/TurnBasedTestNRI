using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void PlayIsDefending(bool isDefending)
    {
        anim.SetBool("IsDefending", isDefending);
    }
    public void PlayAttacking()
    {
        anim.SetTrigger("IsAttack");
    }
    public void PlayIsCasting()
    {
        anim.SetTrigger("IsCasting");
    }
    public void PlayHit()
    {
        anim.SetTrigger("Hit");
    }
}
