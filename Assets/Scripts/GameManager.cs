using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private GameObject playerPrefab;
	public GameObject Player { get; private set; }

    void Awake()
    {
        DontDestroyOnLoad(this);

		Player = Instantiate(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
    }
}
