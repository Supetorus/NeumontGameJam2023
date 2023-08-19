using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class WorldManager : MonoBehaviour
{
	private static readonly int TILES_X = 4;
	private static readonly float TILES_X_OFFSET = 5.0f - (TILES_X * 10.0f / 2.0f);
	private static readonly int TILES_X_EVEN = (TILES_X + 1) % 2 * 5;
	private static readonly int TILES_Y = 3;
	private static readonly float TILES_Y_OFFSET = 5.0f - (TILES_Y * 10.0f / 2.0f);
	private static readonly int TILES_Y_EVEN = (TILES_Y + 1) % 2 * 5;
	private static uint SEED;

	[SerializeField] private List<GameObject> tilePrefabs;

	private GameManager manager;
	private GameObject[,] tiles = new GameObject[TILES_X, TILES_Y];

	private Vector2Int playerTile = Vector2Int.zero;
	private Vector2Int prevPlayerTile = Vector2Int.zero;

	private int GetTile(Vector3 pos)
	{
		uint x = (uint)(((int)pos.x + TILES_X_EVEN) / 10);
		uint y = (uint)(((int)pos.y + TILES_Y_EVEN) / 10);
		return (int)((x ^ 2U * y + SEED) % (uint)tilePrefabs.Count);
	}

	void Start()
	{
		SEED = (uint)Random.Range(0, uint.MaxValue);
		manager = FindFirstObjectByType<GameManager>();

		for (int i = 0; i < TILES_X; ++i)
		{
			for (int j = 0; j < TILES_Y; ++j)
			{
				Vector3 newPos = new Vector3(i * 10.0f + TILES_X_OFFSET, j * 10.0f + TILES_Y_OFFSET, 0.0f);

				tiles[i, j] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}

		//TODO: Generate world
	}

	void Update()
	{
		Vector3 pos = manager.Player.transform.position;
		prevPlayerTile = playerTile;
		playerTile = new Vector2Int((int)((pos.x + 5.0f) / 10) - (pos.x < -5.0f ? 1 : 0), (int)((pos.y + 5.0f) / 10) - (pos.y < -5.0f ? 1 : 0));

		if (prevPlayerTile.x < playerTile.x)
		{
			for (int j = 0; j < TILES_Y; ++j)
			{
				Destroy(tiles[0, j]);
				for (int i = 0; i < TILES_X - 1; ++i) { tiles[i, j] = tiles[i + 1, j]; }

				Vector3 newPos = new Vector3((playerTile.x + TILES_X - 1) * 10.0f + TILES_X_OFFSET,
					(playerTile.y + j) * 10.0f + TILES_Y_OFFSET, 0.0f);

				tiles[TILES_X - 1, j] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}
		else if (prevPlayerTile.x > playerTile.x)
		{
			for (int j = 0; j < TILES_Y; ++j)
			{
				Destroy(tiles[TILES_X - 1, j]);
				for (int i = TILES_X - 1; i > 0; --i) { tiles[i, j] = tiles[i - 1, j]; }

				Vector3 newPos = new Vector3((playerTile.x - TILES_X + 1) * 10.0f - TILES_X_OFFSET,
					(playerTile.y + j) * 10.0f + TILES_Y_OFFSET, 0.0f);

				tiles[0, j] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}

		if (prevPlayerTile.y < playerTile.y)
		{
			for (int i = 0; i < TILES_X; ++i)
			{
				Destroy(tiles[i, 0]);
				for (int j = 0; j < TILES_Y - 1; ++j) { tiles[i, j] = tiles[i, j + 1]; }

				Vector3 newPos = new Vector3((playerTile.x + i) * 10.0f + TILES_X_OFFSET,
					(playerTile.y + TILES_Y - 1) * 10.0f + TILES_Y_OFFSET, 0.0f);

				tiles[i, TILES_Y - 1] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}
		else if (prevPlayerTile.y > playerTile.y)
		{
			for (int i = 0; i < TILES_X; ++i)
			{
				Destroy(tiles[i, TILES_Y - 1]);
				for (int j = TILES_Y - 1; j > 0; --j) { tiles[i, j] = tiles[i, j - 1]; }

				Vector3 newPos = new Vector3((playerTile.x + i) * 10.0f + TILES_X_OFFSET,
					(playerTile.y - TILES_Y + 1) * 10.0f - TILES_Y_OFFSET, 0.0f);

				tiles[i, 0] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}
	}
}
