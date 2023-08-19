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

	[Header("Renderer")]
	[SerializeField]
	private Sprite m_Sprite;

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

	private float m_Health;
	private Vector2 m_LookDirection = new Vector2(0, 0);

	[HideInInspector]
	public UnityEvent m_DeathEvent = new UnityEvent();
	[HideInInspector]
	public UnityEvent m_DamageEvent = new UnityEvent();

	// Start is called before the first frame update
	private void Start()
	{
		m_Health = m_MaxHealth;

		m_WeaponObject = new GameObject();
		m_WeaponObject.transform.parent = transform;
		m_WeaponSprite = m_WeaponObject.AddComponent<SpriteRenderer>();
		m_WeaponSprite.sprite = m_Sprite;

		// renderer
		m_Renderer = GetComponent<SpriteRenderer>();

		// movement / physics
		m_Collider = GetComponent<CircleCollider2D>();
		m_Rigidbody = GetComponent<Rigidbody2D>();

		m_Collider.radius = 0.5f;

		m_Rigidbody.bodyType = RigidbodyType2D.Kinematic;
		m_Rigidbody.interpolation = RigidbodyInterpolation2D.None;
		m_Rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
	}

	// Update is called once per frame
	private void Update()
	{
		float speed = m_IsRunning ? m_RunSpeed : m_WalkSpeed;

		m_WeaponObject.transform.localPosition = m_LookDirection * 0.5f;
		//m_WeaponObject.transform.rotation = Quaternion

		if (m_MovementDirection.sqrMagnitude >= 0.001f)
		{
			m_Renderer.flipX = Vector2.Dot(m_MovementDirection, Vector2.left) > 0;

			m_Rigidbody.velocity = m_MovementDirection * speed;
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

	public void Damage(float damage)
	{
		m_Health -= damage;
		m_Health = Mathf.Clamp(m_Health, 0, m_MaxHealth);

		GameObject bloodPartical = Instantiate(m_BloodPartical);
		Destroy(bloodPartical, 5);

		m_DamageEvent.Invoke();

		if(m_Health == 0)
			m_DeathEvent.Invoke();
	}

	public void Attack()
	{
		// TODO
	}

}
