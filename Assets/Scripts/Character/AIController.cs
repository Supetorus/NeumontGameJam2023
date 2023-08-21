using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class AIController : MonoBehaviour
{

	[SerializeField, Tooltip("Whether this AI should flee or attack")]
	private bool flees;
	[SerializeField, Tooltip("How far away from this AI other AI will become aggro")]
	private float aggroAlertRadius;
	[SerializeField, Tooltip("How close this character has to be to the player to attack")]
	private float minAttackDistance = 1;
	[SerializeField, Tooltip("How long the AI has to wait before it's first attack when it's in range")]
	private float firstAttackDelay = 0.1f;
	[SerializeField, Tooltip("How much the direction can change while wandering, ie how wildly the ai wanders."), Range(0, 180)]
	private float directionChangeRange = 15;
	[SerializeField, Tooltip("Disable wandering for testing")]
	private bool doWander = true;
	[SerializeField, Tooltip("The sound that plays when they are alerted")]
	private AudioClip alertSound;

	private bool isAggro;
	private bool IsAggro { get { return isAggro; } 
		set 
		{ 
			bool wasAggro = isAggro; 
			isAggro = value; 
			if (!wasAggro && isAggro && alertSound != null) { audio.PlayOneShot(alertSound); } 
		} 
	}
	private Character character;
	private GameObject player;
	private new AudioSource audio;
	private float currentAngle;
	private bool isDead;
	private float firstAttackTimer = 0;
	private float fleeTimer = 0;
	private static EnemyManager enemyManager;
	private const int maxDeadBodies = 100;
	private static Queue<AIController> deadPeople = new Queue<AIController>();

	private void Start()
	{
		if (enemyManager == null) { enemyManager = FindFirstObjectByType<EnemyManager>(); }

		currentAngle = Random.Range(0, 180);
		character = GetComponent<Character>();
		character.m_DamageEvent.AddListener(DamageTaken);
		character.m_DeathEvent.AddListener(OnDeath);
		player = GameManager.Instance.Player;
		audio = GetComponent<AudioSource>();

		character.m_ComponentFilter = new System.Type[] { typeof(PlayerController) };
	}

	private void Update()
	{
		character.SetWeaponEnabled(isAggro && !flees);
		if (isDead) return;
		Vector2 moveDirection;
		float distToPlayer = float.MaxValue;
		if (player != null) { distToPlayer = Vector3.Distance(transform.position, player.transform.position); }
		
		if (isAggro)
		{
			Vector2 toPlayer = player.transform.position - transform.position;
			moveDirection = flees ? -toPlayer : toPlayer;

			if (!flees && distToPlayer < minAttackDistance)
			{
				firstAttackTimer += Time.deltaTime;
				if (firstAttackTimer > firstAttackDelay)
				{
					character.Attack();
				}
			}
			else
			{
				firstAttackTimer = 0;
			}

			if(flees)
			{
				fleeTimer += Time.deltaTime;

				if(GetComponent<Renderer>().isVisible)
				{
					fleeTimer = 0.0f;
				}
				else if(fleeTimer >= 10.0f)
				{
					enemyManager.Despawn(gameObject);
					Destroy(gameObject);
				}
			}
		}
		else if (doWander)
		{
			currentAngle += Random.Range(-directionChangeRange, directionChangeRange) * Time.deltaTime;
			moveDirection = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle));
		}
		else
		{
			moveDirection = Vector2.zero;
		}
		character.SetSprint(isAggro);

		if (distToPlayer >= 1.0001)
			character.Move(moveDirection);
		character.SetLookDirection(moveDirection);
	}

	private void DamageTaken()
	{
		IsAggro = true;
		var nearby = Physics2D.OverlapCircleAll(transform.position, aggroAlertRadius);
		foreach (var collider in nearby)
		{
			if (collider.TryGetComponent<AIController>(out var aiController))
			{
				aiController.IsAggro = true;
			}
		}
	}

	private void OnDeath()
	{
		enemyManager.EnemyDeath(gameObject);
		isDead = true;
		GetComponent<Collider2D>().enabled = false;
		character.Move(Vector2.zero);
		var sprite = GetComponent<SpriteRenderer>();
		sprite.color = Color.red;
		sprite.sortingLayerName = "DeadCharacters";
		StartCoroutine(DeathAnimation());
		deadPeople.Enqueue(this);
		while (deadPeople.Count > maxDeadBodies)
		{
			Destroy(deadPeople.Dequeue().gameObject);
		}
	}

	private const float timeToDie = 0.5f;
	private IEnumerator DeathAnimation()
	{
		bool flip = System.Convert.ToBoolean(Random.Range(0, 1));
		Quaternion startRotation = transform.rotation;
		Quaternion targetRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + (flip ? -90 : 90));

		float elapsedTime = 0.0f;

		while (elapsedTime < timeToDie)
		{
			transform.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / timeToDie);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		transform.rotation = targetRotation;
	}
}
