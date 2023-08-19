using System.Collections.Generic;
using UnityEngine;

public class MenuWorldManager : MonoBehaviour
{
	private static readonly float TILE_SIZE = 20.0f;
	private static readonly float HALF_TILE_SIZE = TILE_SIZE / 2.0f;

	private static readonly int TILES_X = 4;
	private static readonly float TILES_X_OFFSET = HALF_TILE_SIZE - (TILES_X * HALF_TILE_SIZE);
	private static readonly int TILES_X_EVEN = (TILES_X + 1) % 2 * 5;
	private static readonly int TILES_Y = 3;
	private static readonly float TILES_Y_OFFSET = HALF_TILE_SIZE - (TILES_Y * HALF_TILE_SIZE);
	private static readonly int TILES_Y_EVEN = (TILES_Y + 1) % 2 * 5;
	private static uint SEED;

	[SerializeField] private Transform target;
	[SerializeField] private List<GameObject> tilePrefabs;

	private GameObject[,] tiles = new GameObject[TILES_X, TILES_Y];

	private Vector2Int playerTile = Vector2Int.zero;
	private Vector2Int prevPlayerTile = Vector2Int.zero;

	private int GetTile(Vector3 pos)
	{
		uint x = (uint)(((int)pos.x + TILES_X_EVEN) / TILE_SIZE);
		uint y = (uint)(((int)pos.y + TILES_Y_EVEN) / TILE_SIZE);
		return (int)((x ^ 2U * y + SEED) % (uint)tilePrefabs.Count);
	}

	void Start()
	{
		SEED = (uint)Random.Range(0, uint.MaxValue);

		for (int i = 0; i < TILES_X; ++i)
		{
			for (int j = 0; j < TILES_Y; ++j)
			{
				Vector3 newPos = new Vector3(i * TILE_SIZE + TILES_X_OFFSET, j * TILE_SIZE + TILES_Y_OFFSET, 0.0f);

				tiles[i, j] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}
	}

	void Update()
	{
		Vector3 pos = target.position;
		prevPlayerTile = playerTile;
		playerTile = new Vector2Int((int)((pos.x + HALF_TILE_SIZE) / TILE_SIZE - (pos.x < -HALF_TILE_SIZE ? 1 : 0)), 
			(int)((pos.y + HALF_TILE_SIZE) / TILE_SIZE - (pos.y < -HALF_TILE_SIZE ? 1 : 0)));

		if (prevPlayerTile.x < playerTile.x)
		{
			for (int j = 0; j < TILES_Y; ++j)
			{
				Destroy(tiles[0, j]);
				for (int i = 0; i < TILES_X - 1; ++i) { tiles[i, j] = tiles[i + 1, j]; }

				Vector3 newPos = new Vector3((playerTile.x + TILES_X - 1) * TILE_SIZE + TILES_X_OFFSET,
					(playerTile.y + j) * TILE_SIZE + TILES_Y_OFFSET, 0.0f);

				tiles[TILES_X - 1, j] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}
		else if (prevPlayerTile.x > playerTile.x)
		{
			for (int j = 0; j < TILES_Y; ++j)
			{
				Destroy(tiles[TILES_X - 1, j]);
				for (int i = TILES_X - 1; i > 0; --i) { tiles[i, j] = tiles[i - 1, j]; }

				Vector3 newPos = new Vector3((playerTile.x - TILES_X + 1) * TILE_SIZE - TILES_X_OFFSET,
					(playerTile.y + j) * TILE_SIZE + TILES_Y_OFFSET, 0.0f);

				tiles[0, j] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}

		if (prevPlayerTile.y < playerTile.y)
		{
			for (int i = 0; i < TILES_X; ++i)
			{
				Destroy(tiles[i, 0]);
				for (int j = 0; j < TILES_Y - 1; ++j) { tiles[i, j] = tiles[i, j + 1]; }

				Vector3 newPos = new Vector3((playerTile.x + i) * TILE_SIZE + TILES_X_OFFSET,
					(playerTile.y + TILES_Y - 1) * TILE_SIZE + TILES_Y_OFFSET, 0.0f);

				tiles[i, TILES_Y - 1] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}
		else if (prevPlayerTile.y > playerTile.y)
		{
			for (int i = 0; i < TILES_X; ++i)
			{
				Destroy(tiles[i, TILES_Y - 1]);
				for (int j = TILES_Y - 1; j > 0; --j) { tiles[i, j] = tiles[i, j - 1]; }

				Vector3 newPos = new Vector3((playerTile.x + i) * TILE_SIZE + TILES_X_OFFSET,
					(playerTile.y - TILES_Y + 1) * TILE_SIZE - TILES_Y_OFFSET, 0.0f);

				tiles[i, 0] = Instantiate(tilePrefabs[GetTile(newPos)], newPos, Quaternion.identity);
			}
		}
	}
}
