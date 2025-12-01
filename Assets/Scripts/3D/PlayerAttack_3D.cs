using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class PlayerAttack_3D : MonoBehaviour
{
    [Header("Base Stats")]
    public float baseDamage = 5f;
    public float attackRadius;
    public float attackCooldown = .5f;
    public float attackKnockback = 1f;

    [Header("Checks")]
    public Transform attackCheck;
    public bool canAttack = true;
    public bool isTimeToCheck = false;
    public LayerMask enemyLayer;

    [Header("Other")]
    public Animator animator;

    private PlayerMover controller;

    void Awake()
    {
        controller = GetComponent<PlayerMover>();
    }

    void Update()
    {
        // // bole ba gerak pas attack
        // if (canAttack)
        // {   
        //     Debug.Log("Damage");
        //     canAttack = false;
        //     animator.SetBool("IsAttacking", true);
        //     StartCoroutine(AttackCooldown());
        // }
    }

    void FixedUpdate()
    {
        if (Physics.CheckSphere(attackCheck.position, attackRadius, enemyLayer))
        {
            if (!animator.GetBool("IsAttacking"))
            {
                animator.SetBool("IsAttacking", true);
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void DoDamage()
    {
        if (canAttack)
        {
            Debug.Log("Do Damage");
            canAttack = false;
            // animator.SetBool("IsAttacking", true);
            StartCoroutine(AttackCooldown());

            // disini nanti tambah logic untuk scale damage (senjata, etc)
            baseDamage = Mathf.Abs(baseDamage);

            Collider[] enemyColliders = Physics.OverlapSphere(attackCheck.position, attackRadius, enemyLayer);
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (enemyColliders[i].transform.position.x - transform.position.x < 0)
                {
                    baseDamage = -baseDamage;
                }
                enemyColliders[i].gameObject.SendMessage("ApplyDamage", (baseDamage, attackKnockback));
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
        }
    }
}
