using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	private static readonly int MAX_ENEMIES = 20;
	private static readonly float MAX_ENEMY_DISTANCE_X = 25.0f;
	private static readonly float MAX_ENEMY_DISTANCE_Y = 20.0f;

	[SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();

	private List<GameObject> enemies = new List<GameObject>();
	private GameManager manager;

	void Start()
	{
		manager = FindFirstObjectByType<GameManager>();
	}

    void Update()
    {
		Vector3 pos = manager.Player.transform.position;

		foreach(GameObject enemy in enemies)
		{
			if(enemy.transform.position.x > pos.x + MAX_ENEMY_DISTANCE_X) { enemy.transform.position += Vector3.left * MAX_ENEMY_DISTANCE_X * 2.0f; }
			else if(enemy.transform.position.x < pos.x - MAX_ENEMY_DISTANCE_X) { enemy.transform.position += Vector3.right * MAX_ENEMY_DISTANCE_X * 2.0f; }

			if (enemy.transform.position.y > pos.y + MAX_ENEMY_DISTANCE_Y) { enemy.transform.position += Vector3.down * MAX_ENEMY_DISTANCE_Y * 2.0f; }
			else if (enemy.transform.position.y < pos.y - MAX_ENEMY_DISTANCE_Y) { enemy.transform.position += Vector3.up * MAX_ENEMY_DISTANCE_Y * 2.0f; }
		}

		if (enemies.Count < MAX_ENEMIES)
		{
			enemies.Add(Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], 
				new Vector3(pos.x + Random.Range(-MAX_ENEMY_DISTANCE_X, MAX_ENEMY_DISTANCE_X), 
				pos.y + Random.Range(-MAX_ENEMY_DISTANCE_Y, MAX_ENEMY_DISTANCE_Y), 0.0f), Quaternion.identity));
		}
    }
}
