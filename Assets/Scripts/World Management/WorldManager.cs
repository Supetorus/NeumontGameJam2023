using UnityEngine;

public class WorldManager : MonoBehaviour
{
	private static readonly uint TILES_X = 4;
	private static readonly float TILES_X_OFFSET = 5.0f - (TILES_X * 10.0f / 2.0f);
	private static readonly uint TILES_Y = 3;
	private static readonly float TILES_Y_OFFSET = 5.0f - (TILES_Y * 10.0f / 2.0f);

	[SerializeField] private GameObject tile;

	private GameManager manager;
	private GameObject[,] tiles = new GameObject[TILES_X, TILES_Y];

	private Vector2Int playerTile = Vector2Int.zero;
	private Vector2Int prevPlayerTile = Vector2Int.zero;

	void Start()
	{
		manager = FindFirstObjectByType<GameManager>();

		for (uint i = 0; i < TILES_X; ++i)
		{
			for (uint j = 0; j < TILES_Y; ++j)
			{
				tiles[i, j] = Instantiate(tile, new Vector3(i * 10.0f + TILES_X_OFFSET, j * 10.0f + TILES_Y_OFFSET, 0.0f), Quaternion.identity);
			}
		}

		//TODO: Generate world
	}

	void Update()
	{
		Vector3 pos = manager.Player.transform.position;
		prevPlayerTile = playerTile;
		playerTile = new Vector2Int((int)((pos.x + 5.0f) / 10) - (pos.x < -5.0f ? 1 : 0), (int)((pos.y + 5.0f) / 10) - (pos.y < -5.0f ? 1 : 0));
		
		if(prevPlayerTile != playerTile)
		{
			if(prevPlayerTile.x < playerTile.x)
			{
				for(int j = 0; j < TILES_Y; ++j)
				{
					Destroy(tiles[0, j]);
					for (int i = 0; i < TILES_X - 1; ++i) { tiles[i, j] = tiles[i + 1, j]; }

					tiles[TILES_X - 1, j] = Instantiate(tile, new Vector3((playerTile.x + TILES_X - 1) * 10.0f + TILES_X_OFFSET,
						(playerTile.y + j) * 10.0f + TILES_Y_OFFSET, 0.0f), Quaternion.identity);
				}
			}
			else if(prevPlayerTile.x > playerTile.x)
			{
				for (int j = 0; j < TILES_Y; ++j)
				{
					Destroy(tiles[TILES_X - 1, j]);
					for (int i = (int)TILES_X - 1; i > 0; --i) { tiles[i, j] = tiles[i - 1, j]; }

					tiles[0, j] = Instantiate(tile, new Vector3((playerTile.x - TILES_X + 1) * 10.0f - TILES_X_OFFSET,
						(playerTile.y + j) * 10.0f + TILES_Y_OFFSET, 0.0f), Quaternion.identity);
				}
			}

			if (prevPlayerTile.y < playerTile.y)
			{
				for (int i = 0; i < TILES_X; ++i)
				{
					Destroy(tiles[i, 0]);
					for (int j = 0; j < TILES_Y - 1; ++j) { tiles[i, j] = tiles[i, j + 1]; }

					tiles[i, TILES_Y - 1] = Instantiate(tile, new Vector3((playerTile.x + i) * 10.0f + TILES_X_OFFSET,
						(playerTile.y + TILES_Y - 1) * 10.0f + TILES_Y_OFFSET, 0.0f), Quaternion.identity);
				}
			}
			else if (prevPlayerTile.y > playerTile.y)
			{
				for (int i = 0; i < TILES_X; ++i)
				{
					Destroy(tiles[i, TILES_Y - 1]);
					for (int j = (int)TILES_Y - 1; j > 0; --j) { tiles[i, j] = tiles[i, j - 1]; }

					tiles[i, 0] = Instantiate(tile, new Vector3((playerTile.x + i) * 10.0f + TILES_X_OFFSET,
						(playerTile.y - TILES_Y + 1) * 10.0f - TILES_Y_OFFSET, 0.0f), Quaternion.identity);
				}
			}
		}
	}
}
