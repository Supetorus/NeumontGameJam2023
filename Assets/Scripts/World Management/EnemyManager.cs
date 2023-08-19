using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private static readonly int MAX_ENEMIES = 20;
	private static readonly float MAX_ENEMY_DISTANCE_X = 15.0f;
	private static readonly float MAX_ENEMY_DISTANCE_Y = 8.0f;

	[SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

	private List<GameObject> enemies = new List<GameObject>();
	private GameManager manager;

	void Start()
	{
		manager = FindFirstObjectByType<GameManager>();
	}

	void Update()
	{
		int maxEnemies = (int)(MAX_ENEMIES + Time.realtimeSinceStartup / 12.0f);

		Vector3 pos = manager.Player.transform.position;

		foreach (GameObject enemy in enemies)
		{
			if (enemy.transform.position.x > pos.x + MAX_ENEMY_DISTANCE_X * 2.0f) { enemy.transform.position += Vector3.left * MAX_ENEMY_DISTANCE_X * 3.5f; }
			else if (enemy.transform.position.x < pos.x - MAX_ENEMY_DISTANCE_X * 2.0f) { enemy.transform.position += Vector3.right * MAX_ENEMY_DISTANCE_X * 3.5f; }

			if (enemy.transform.position.y > pos.y + MAX_ENEMY_DISTANCE_Y * 2.0f) { enemy.transform.position += Vector3.down * MAX_ENEMY_DISTANCE_Y * 3.5f; }
			else if (enemy.transform.position.y < pos.y - MAX_ENEMY_DISTANCE_Y * 2.0f) { enemy.transform.position += Vector3.up * MAX_ENEMY_DISTANCE_Y * 3.5f; }
		}

		if (enemies.Count < maxEnemies)
		{
			switch (Random.Range(0, 4))
			{
				case 0:
					{
						enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
							new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X - 5.0f, -MAX_ENEMY_DISTANCE_X),
							pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity));
					}
					break;
				case 1:
					{
						enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
							new Vector3(pos.x + Random.Range(MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X + 5.0f),
							pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity));
					}
					break;
				case 2:
					{
						enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
							new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X),
							pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y - 5.0f, -MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity));
					}
					break;
				case 3:
					{
						enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
							new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X),
							pos.y + Random.Range(MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y + 5.0f), 0.0f), Quaternion.identity));
					}
					break;
			}
		}
	}

	public void EnemyDeath(GameObject enemy)
	{
		enemies.Remove(enemy);
		GameManager.Instance.AddScore();
	}

	public void Despawn(GameObject enemy)
	{
		enemies.Remove(enemy);
	}
}
