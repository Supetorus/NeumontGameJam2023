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

	private void Start()
	{
		currentAngle = Random.Range(0, 180);
		character = GetComponent<Character>();
		character.m_DamageEvent.AddListener(DamageTaken);
		player = FindObjectOfType<GameManager>().Player;

		character.m_ComponentFilter = new System.Type[] { typeof(PlayerController) };
	}

	private void Update()
	{
		Vector2 moveDirection;
		if (isAggro)
		{
			Vector2 toPlayer = player.transform.position - transform.position;
			moveDirection = flees ? -toPlayer : toPlayer;
			if (!flees && Vector3.Distance(transform.position, player.transform.position) < minAttackDistance) character.Attack();
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
}
