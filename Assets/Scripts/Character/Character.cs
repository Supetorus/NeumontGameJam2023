using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
	private SpriteRenderer m_Renderer;
	private Rigidbody2D m_Rigidbody;
	private CircleCollider2D m_Collider;

	private GameObject m_WeaponObject;
	private SpriteRenderer m_WeaponSprite;

	private GameObject m_SweepObject;
	private SpriteRenderer m_SweepSprite;

	public Weapon m_Weapon;

	[Header("Movement")]
	[SerializeField]
	private float m_WalkSpeed = 2;
	[SerializeField]
	private float m_RunSpeed = 4;

	private bool m_IsRunning = false;

	private Vector2 m_MovementDirection = new Vector2(0, 0);

	[Header("Combat")]
	[SerializeField]
	private float m_MaxHealth = 100;
	[SerializeField]
	private GameObject m_BloodPartical;
	[SerializeField]
	private float m_KnockbackScale = 1;

	private float m_Health;
	private Vector2 m_LookDirection = new Vector2(0, 0);

	[HideInInspector]
	public UnityEvent m_DeathEvent = new UnityEvent();
	[HideInInspector]
	public UnityEvent m_DamageEvent = new UnityEvent();

	private float m_NextAttackTime;

	private float m_DisableSweepSpritTime;

	// Start is called before the first frame update
	private void Start()
	{
		m_Health = m_MaxHealth;

		m_WeaponObject = new GameObject();
		m_WeaponObject.transform.parent = transform;
		m_WeaponSprite = m_WeaponObject.AddComponent<SpriteRenderer>();
		m_WeaponSprite.sprite = m_Weapon.WeaponSprite;

		m_SweepObject = new GameObject();
		m_SweepObject.transform.parent = transform;
		m_SweepSprite = m_SweepObject.AddComponent<SpriteRenderer>();
		m_SweepSprite.sprite = m_Weapon.SweepSprite;
		m_SweepObject.SetActive(false);

		// renderer
		m_Renderer = GetComponent<SpriteRenderer>();

		// movement / physics
		m_Collider = GetComponent<CircleCollider2D>();
		m_Rigidbody = GetComponent<Rigidbody2D>();

		m_Collider.radius = 0.5f;

		m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;
		m_Rigidbody.interpolation = RigidbodyInterpolation2D.None;
		m_Rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
		m_Rigidbody.freezeRotation = true;
		m_Rigidbody.gravityScale = 0;
	}

	// Update is called once per frame
	private void Update()
	{
		float speed = m_IsRunning ? m_RunSpeed : m_WalkSpeed;


		float weaponAngle = Vector2.SignedAngle(Vector2.right, m_LookDirection);
		m_SweepObject.transform.localPosition = m_LookDirection * 0.5f;
		m_SweepObject.transform.rotation = Quaternion.AngleAxis(weaponAngle, Vector3.forward);

		if (Time.time >= m_DisableSweepSpritTime)
			m_SweepObject.SetActive(false);

		weaponAngle += m_Weapon.SweepingAngle * 0.5f * (m_WeaponSprite.flipX ? -1 : 1);
		m_WeaponObject.transform.localPosition = new Vector2(Mathf.Cos(weaponAngle * Mathf.Deg2Rad), Mathf.Sin(weaponAngle * Mathf.Deg2Rad)) * 0.5f;
		m_WeaponObject.transform.rotation = Quaternion.AngleAxis(weaponAngle, Vector3.forward);

		if (m_MovementDirection.sqrMagnitude >= 0.001f)
		{
			m_Renderer.flipX = Vector2.Dot(m_MovementDirection, Vector2.left) > 0;

			m_Rigidbody.velocity = Vector2.Lerp(m_Rigidbody.velocity, m_MovementDirection * speed, 0.2f);
		}
		else
			m_Rigidbody.velocity = Vector2.zero;

	}

	public void SetSprint(bool sprint)
	{
		m_IsRunning = sprint;
	}

	public void Move(Vector2 direction)
	{
		m_MovementDirection = direction.normalized;
	}

	public void SetLookDirection(Vector2 direction)
	{
		m_LookDirection = direction.normalized;
	}

	public void Damage(float damage, Vector2 knockback)
	{
		m_Health -= damage;
		m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);

		m_Rigidbody.AddForce(knockback * m_KnockbackScale, ForceMode2D.Impulse);

		GameObject bloodPartical = Instantiate(m_BloodPartical, transform);
		Destroy(bloodPartical, 5);

		m_DamageEvent.Invoke();

		if(m_Health == 0)
			m_DeathEvent.Invoke();
	}

	public void Attack()
	{
		if (Time.time >= m_NextAttackTime)
		{
			m_NextAttackTime = Time.time + m_Weapon.AttackSpeed;

			m_WeaponSprite.flipX = !m_WeaponSprite.flipX;
			m_SweepObject.SetActive(true);
			m_DisableSweepSpritTime = Time.time + 0.1f;

			Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, m_Weapon.AttackRange);

			foreach (var collision in collisions)
			{
				Character charComp = collision.GetComponent<Character>();
				if (charComp != null && charComp != this)
				{
					Vector2 dir = (collision.transform.position - transform.position).normalized;

					float angle = Vector2.Angle(m_LookDirection, dir);
					if(angle <= m_Weapon.SweepingAngle/2)
						charComp.Damage(m_Weapon.Damage, m_Weapon.Knockback * dir);
				}
			}
		}
	}

}
