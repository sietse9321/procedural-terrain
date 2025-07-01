using Interfaces;
using UnityEngine;

public class SwordAttackCombo : MonoBehaviour, IAttackCombo
{
    [SerializeField] private Animator animator;
    [SerializeField] private int maxCombo = 2;
    [SerializeField] private float comboResetTime = 1.0f;

    [SerializeField] private int currentCombo = 0;
    private float _lastAttackTime = 0f;
    private bool _isAttacking = false;

    public bool CanAttack => !_isAttacking || (Time.time - _lastAttackTime < comboResetTime);

    void Update()
    {
        //reset combo if too much time passes
        if (_isAttacking && Time.time - _lastAttackTime > comboResetTime)
        {
            Debug.Log("reset combo");
            currentCombo = 0;
            _isAttacking = false;
            animator.SetInteger("AttackIndex", currentCombo);
            animator.SetBool("Attack", false);
        }
    }

    public void Attack()
    {
        if (!CanAttack) return;

        if (_isAttacking)
        {
            currentCombo = (currentCombo % maxCombo) + 1;
        }
        else
        {
            currentCombo = 1;
        }

        _lastAttackTime = Time.time;

        animator.SetInteger("AttackIndex", currentCombo);
        animator.SetBool("Attack", true);

        _isAttacking = true;
    }

    
    public void OnAttackAnimationEnd()
    {
        //add animation or something?
    }
}