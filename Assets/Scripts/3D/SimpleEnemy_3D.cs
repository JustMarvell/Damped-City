using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SimpleEnemy_3D : MonoBehaviour
{
    // pke navmesh agent (WIP)

    private bool isHitted = false;

    [Header("Base Stats")]
    public float life = 10f;
    public bool IsInvincible = false;
    public float speed = 5f;

    [Header("Combat Stats")]
    public float meleeDist = 1.5f;
    public float baseDamage = 4;
    public float rangeDist = 5f;
    public float meleeAttackRadius = .9f;
    public float meleeAttackCooldown = .5f;

    public float knockbackForce = 15f;
    public float stunTime = .4f;
    public GameObject throwableObject;

    [Header("Constraints")]
    public GameObject target;
    public LayerMask targetLayer;
    public bool canAttack = true;
    public bool canThrow;
    public Transform attackCheck;
    private float distToPlayer;
    private float distToPlayerY;

    [Header("Debug")]
    public bool showAttackRadius = false;
    public bool showRangeDist = false;
    public bool showMeleeDist = false;

    private Animator animator;
    private Rigidbody rb;

    void Awake()
    {
        if (target == null)
            target = GameObject.FindGameObjectWithTag("Player");

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (life <= 0)
            StartCoroutine(DestroyEnemy());
        else if (target != null)
        {
            // dashing
            // W.I.P
            // kl ada yee

            // move logic + animation + other logic here
            // W.I.P
        }
    }

    public void ApplyDamage((float baseDamage, float baseKnockback) damageInfo)
    {
        if (!IsInvincible)
        {
            Vector3 direction = target.transform.forward;
            damageInfo.baseDamage = Mathf.Abs(damageInfo.baseDamage);

            animator.SetBool("Hit", true);

            life -= damageInfo.baseDamage;

            rb.velocity = Vector2.zero;
            rb.AddForce(direction * damageInfo.baseKnockback * 100f, ForceMode.Impulse);

            StartCoroutine(HitTime());
        }
    }

    IEnumerator HitTime()
    {
        IsInvincible = true;
        isHitted = true;
        yield return new WaitForSeconds(.1f);
        isHitted = false;
        IsInvincible = false;
    }

    IEnumerator WaitToAttack(float time)
    {
        canAttack = false;
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    IEnumerator DestroyEnemy()
    {
        Collider col = GetComponent<Collider>();
        col.enabled = false;

        rb.isKinematic = true;

        animator.SetBool("IsDead", true);

        yield return new WaitForSeconds(3f);

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        if (showMeleeDist)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, meleeDist);
        }

        if (attackCheck != null && showAttackRadius)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(attackCheck.position, meleeAttackRadius);
        }

        if (showRangeDist)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, rangeDist);
        }
    }
}
