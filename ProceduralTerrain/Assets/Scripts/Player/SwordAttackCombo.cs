using UnityEngine;

public class SwordAttackCombo : MonoBehaviour, IAttackCombo
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxCombo = 2;
    [SerializeField] private float comboResetTime = 1.0f;

    [SerializeField] private int currentCombo = 0;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    public bool CanAttack => !isAttacking || (Time.time - lastAttackTime < comboResetTime);

    void Update()
    {
        //reset combo if too much time passes
        if (isAttacking && Time.time - lastAttackTime > comboResetTime)
        {
            Debug.Log("reset combo");
            currentCombo = 0;
            isAttacking = false;
            animator.SetInteger("AttackIndex", currentCombo);
            animator.SetBool("Attack", false);
        }
    }

    public void Attack()
    {
        if (!CanAttack) return;

        if (isAttacking)
        {
            currentCombo = (currentCombo % maxCombo) + 1;
        }
        else
        {
            currentCombo = 1;
        }

        lastAttackTime = Time.time;

        animator.SetInteger("AttackIndex", currentCombo);
        animator.SetBool("Attack", true);

        isAttacking = true;
    }

    
    public void OnAttackAnimationEnd()
    {
        //add animation or something?
    }
}