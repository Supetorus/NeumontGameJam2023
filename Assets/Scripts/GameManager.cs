using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	private SceneLoader sceneLoader;
	public SceneLoader SceneLoader
	{
		get { if (sceneLoader == null) { sceneLoader = new GameObject("SceneLoader").AddComponent<SceneLoader>(); DontDestroyOnLoad(sceneLoader); } return sceneLoader; }
		private set { sceneLoader = value; }
	}

	[SerializeField] private GameObject playerPrefab;
	public GameObject Player { get; private set; }

	private void Awake()
	{
		if(Instance == null) { Instance = this; DontDestroyOnLoad(this); }
	}

	public void OnSceneLoad(Scene scene, LoadSceneMode mode)
	{
		if(scene.name == "Game")
		{
			Player = Instantiate(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
		}
	}
}
