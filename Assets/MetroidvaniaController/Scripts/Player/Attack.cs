using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
	public float dmgValue = 4;
	public GameObject throwableObject;
	public Transform attackCheck;
	public float attackRadius;
	public float attackCooldown = .25f;
	private Rigidbody2D m_Rigidbody2D;
	public Animator animator;
	public bool canAttack = true;
	public bool isTimeToCheck = false;

	//public GameObject cam;

	private CharacterController2D controller;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		controller = GetComponent<CharacterController2D>();
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (controller.PunchInput && canAttack)
		{
			if (controller.m_Grounded)
				m_Rigidbody2D.velocity = Vector2.zero;
			controller.canMove = false;
			canAttack = false;
			animator.SetBool("IsAttacking", true);
			StartCoroutine(AttackCooldown());
		}

		if (Input.GetKeyDown(KeyCode.V))
		{
			GameObject throwableWeapon = Instantiate(throwableObject, transform.position + new Vector3(transform.localScale.x * 0.5f, -0.2f), Quaternion.identity) as GameObject;
			Vector2 direction = new Vector2(transform.localScale.x, 0);
			throwableWeapon.GetComponent<ThrowableWeapon>().direction = direction;
			throwableWeapon.name = "ThrowableWeapon";
		}
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(attackCooldown);
		canAttack = true;
		controller.canMove = true;
	}

	public void DoDashDamage()
	{
		dmgValue = Mathf.Abs(dmgValue);
		Collider2D[] collidersEnemies = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);
		for (int i = 0; i < collidersEnemies.Length; i++)
		{
			if (collidersEnemies[i].gameObject.tag == "Enemy")
			{
				if (collidersEnemies[i].transform.position.x - transform.position.x < 0)
				{
					dmgValue = -dmgValue;
				}
				collidersEnemies[i].gameObject.SendMessage("ApplyDamage", dmgValue);
				//cam.GetComponent<CameraFollow>().ShakeCamera();
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
