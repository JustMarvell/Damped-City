using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimableLadder : MonoBehaviour
{
    public bool isClimbing;

    public float climbingSpeed = 5f;

    PlayerMovement pm;
    CharacterController2D cc;
    Rigidbody2D rb;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            pm = coll.gameObject.GetComponent<PlayerMovement>();
            cc = coll.gameObject.GetComponent<CharacterController2D>();
            rb = coll.gameObject.GetComponent<Rigidbody2D>();

            rb.gravityScale = 0;

            pm.enabled = false;
            cc.enabled = false;

            isClimbing = true;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        isClimbing = false;

        rb.gravityScale = 5;

        pm.enabled = true;
        cc.enabled = true;
    }
}
