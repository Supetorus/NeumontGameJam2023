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
	[SerializeField, Tooltip("How much the direction can change while wandering, ie how wildly the ai wanders."), Range(0, 180)]
	private float directionChangeRange = 15;
	[SerializeField, Tooltip("Disable wandering for testing")]
	private bool doWander = true;

	private bool isAggro;
	private Character character;
	private GameObject player;
	private float currentAngle;
	private bool isDead;

	private static EnemyManager enemyManager;

	private void Start()
	{
		if(enemyManager == null) { FindFirstObjectByType<EnemyManager>(); }

		currentAngle = Random.Range(0, 180);
		character = GetComponent<Character>();
		character.m_DamageEvent.AddListener(DamageTaken);
		character.m_DeathEvent.AddListener(OnDeath);
		player = FindObjectOfType<GameManager>().Player;

		character.m_ComponentFilter = new System.Type[] { typeof(PlayerController) };
	}

	private void Update()
	{
		if (isDead) return;
		Vector2 moveDirection;
		float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
		if (isAggro)
		{
			Vector2 toPlayer = player.transform.position - transform.position;
			moveDirection = flees ? -toPlayer : toPlayer;

			if (!flees && distToPlayer < minAttackDistance) character.Attack();
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
	}

	private void DamageTaken()
	{
		isAggro = true;
		var nearby = Physics2D.OverlapCircleAll(transform.position, aggroAlertRadius);
		foreach (var collider in nearby)
		{
			if (collider.TryGetComponent<AIController>(out var aiController))
			{
				aiController.isAggro = true;
			}
		}
	}

	private void OnDeath()
	{
		enemyManager.EnemyDeath(gameObject);
		isDead = true;
		GetComponent<Collider2D>().enabled = false;
		character.Move(Vector2.zero);
		GetComponent<SpriteRenderer>().color = Color.red;
		StartCoroutine(DeathAnimation());
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
