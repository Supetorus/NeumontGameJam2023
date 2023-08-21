using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private static readonly float MAX_ENEMY_DISTANCE_X = 15.0f;
	private static readonly float MAX_ENEMY_DISTANCE_Y = 8.0f;

	[System.Serializable]
	struct EnemySpawn
	{
		public GameObject prefab;
		[Tooltip("The weight of spawn chance, influenced by every spawn weight"), Range(0.0f, 10.0f)]
		public float weight;
		[Tooltip("Increases the spawn weight over every ten kills"), Range(0.0f, 10.0f)]
		public float weightIncrease;
	}

	[SerializeField, Tooltip("The base number of enemies in the game at the start.")]
	private int baseMobCap = 20;
	[SerializeField, Tooltip("This is multiplied by the current score and added to the base enemy amount to calculate the mob cap.")]
	private float enemyCapMultiplier = 5;
	[SerializeField] private List<EnemySpawn> enemyPrefabs;

	private List<GameObject> spawnedEnemies = new List<GameObject>();
	private GameManager manager;
	private float spawnWeight = 0.0f;

	private void Start()
	{
		manager = FindFirstObjectByType<GameManager>();
	}

	private void Update()
	{
		int maxEnemies = baseMobCap + (int)(GameManager.Instance.Score / enemyCapMultiplier);

		spawnWeight = 0.0f;

		foreach (EnemySpawn spawn in enemyPrefabs)
		{
			spawnWeight += spawn.weight + GameManager.Instance.Score / 10.0f * spawn.weightIncrease;
		}

		Vector3 pos = manager.Player.transform.position;

		foreach (GameObject enemy in spawnedEnemies)
		{
			if (enemy.transform.position.x > pos.x + MAX_ENEMY_DISTANCE_X * 2.0f) { enemy.transform.position += Vector3.left * MAX_ENEMY_DISTANCE_X * 3.5f; }
			else if (enemy.transform.position.x < pos.x - MAX_ENEMY_DISTANCE_X * 2.0f) { enemy.transform.position += Vector3.right * MAX_ENEMY_DISTANCE_X * 3.5f; }

			if (enemy.transform.position.y > pos.y + MAX_ENEMY_DISTANCE_Y * 2.0f) { enemy.transform.position += Vector3.down * MAX_ENEMY_DISTANCE_Y * 3.5f; }
			else if (enemy.transform.position.y < pos.y - MAX_ENEMY_DISTANCE_Y * 2.0f) { enemy.transform.position += Vector3.up * MAX_ENEMY_DISTANCE_Y * 3.5f; }
		}

		if (spawnedEnemies.Count < maxEnemies)
		{
			switch (Random.Range(0, 4))
			{
				case 0:
				{
					spawnedEnemies.Add(Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
						new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X - 5.0f, -MAX_ENEMY_DISTANCE_X),
						pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity));
				}
				break;
				case 1:
				{
					spawnedEnemies.Add(Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
						new Vector3(pos.x + Random.Range(MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X + 5.0f),
						pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity));
				}
				break;
				case 2:
				{
					spawnedEnemies.Add(Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
						new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X),
						pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y - 5.0f, -MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity));
				}
				break;
				case 3:
				{
					spawnedEnemies.Add(Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
						new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X),
						pos.y + Random.Range(MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y + 5.0f), 0.0f), Quaternion.identity));
				}
				break;
			}
		}
	}

	public int GetWeightedSpawn()
	{
		float spawn = Random.Range(0.0f, spawnWeight);
		float weight = 0.0f;

		int i = 0;
		foreach(EnemySpawn enemy in enemyPrefabs)
		{
			float enemyWeight = enemy.weight + GameManager.Instance.Score / 10.0f * enemy.weightIncrease;

			if (spawn <= enemyWeight + weight)
			{
				return i;
			}

			++i;
			weight += enemyWeight;
		}

		return 0;
	}

	public void EnemyDeath(GameObject enemy)
	{
		spawnedEnemies.Remove(enemy);
		GameManager.Instance.AddScore();
	}

	public void Despawn(GameObject enemy)
	{
		spawnedEnemies.Remove(enemy);
	}
}
