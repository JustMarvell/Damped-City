using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    public float spikeDamage = 99f;
    public float spikeKnockbackForce = 15f;
    public float spikeStunTime = 2f;
    public float spikeCooldown = .9f;
    public bool canAttack = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canAttack)
        {
            canAttack = false;
            collision.gameObject.GetComponent<CharacterController2D>().ApplyDamageADV(spikeDamage, transform.position, spikeKnockbackForce, spikeStunTime);
            Invoke(nameof(ResetAttack), spikeCooldown);
        }
    }

    public void ResetAttack()
    {
        canAttack = true;
    }
}
