using System.Collections.Generic;
using UnityEngine;

public class MenuEnemyManager : MonoBehaviour
{
	private static readonly int MAX_ENEMIES = 20;
	private static readonly float MAX_ENEMY_DISTANCE_X = 15.0f;
	private static readonly float MAX_ENEMY_DISTANCE_Y = 8.0f;

	[System.Serializable]
	struct EnemySpawn
	{
		public GameObject prefab;
		[Range(0.0f, 10.0f)]
		public float weight;
	}

	[SerializeField] private Transform target;
	[SerializeField] private List<EnemySpawn> enemyPrefabs;

	private List<GameObject> spawnedEnemies = new List<GameObject>();
	private float spawnWeight = 0.0f;

	private void Start()
	{
		foreach (EnemySpawn spawn in enemyPrefabs)
		{
			spawnWeight += spawn.weight;
		}
	}

	private void Update()
	{
		int maxEnemies = MAX_ENEMIES + GameManager.Instance.Score / 10;

		Vector3 pos = target.position;

		foreach (GameObject enemy in spawnedEnemies)
		{
			if (enemy.transform.position.x > pos.x + MAX_ENEMY_DISTANCE_X * 2.0f) { enemy.transform.position += Vector3.left * MAX_ENEMY_DISTANCE_X * 3.5f; }
			else if (enemy.transform.position.x < pos.x - MAX_ENEMY_DISTANCE_X * 2.0f) { enemy.transform.position += Vector3.right * MAX_ENEMY_DISTANCE_X * 3.5f; }

			if (enemy.transform.position.y > pos.y + MAX_ENEMY_DISTANCE_Y * 2.0f) { enemy.transform.position += Vector3.down * MAX_ENEMY_DISTANCE_Y * 3.5f; }
			else if (enemy.transform.position.y < pos.y - MAX_ENEMY_DISTANCE_Y * 2.0f) { enemy.transform.position += Vector3.up * MAX_ENEMY_DISTANCE_Y * 3.5f; }
		}

		if (spawnedEnemies.Count < maxEnemies)
		{
			GameObject go = null;

			switch (Random.Range(0, 4))
			{
				case 0:
					{
						go = Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
							new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X - 5.0f, -MAX_ENEMY_DISTANCE_X),
							pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity);
						spawnedEnemies.Add(go);
					}
					break;
				case 1:
					{
						go = Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
							new Vector3(pos.x + Random.Range(MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X + 5.0f),
							pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity);
						spawnedEnemies.Add(go);
					}
					break;
				case 2:
					{
						go = Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
						new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X),
						pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y - 5.0f, -MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity);
						spawnedEnemies.Add(go);
					}
					break;
				case 3:
					{
						go = Instantiate(enemyPrefabs[GetWeightedSpawn()].prefab,
						new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X),
						pos.y + Random.Range(MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y + 5.0f), 0.0f), Quaternion.identity);
						spawnedEnemies.Add(go);
					}
					break;
			}

			Destroy(go.GetComponentInChildren<Fillbar>().gameObject);
		}
	}

	public int GetWeightedSpawn()
	{
		float spawn = Random.Range(0.0f, spawnWeight);
		float weight = 0.0f;

		int i = 0;
		foreach (EnemySpawn enemy in enemyPrefabs)
		{
			if (spawn <= enemy.weight + weight)
			{
				return i;
			}

			++i;
			weight += enemy.weight;
		}

		return 0;
	}
}
