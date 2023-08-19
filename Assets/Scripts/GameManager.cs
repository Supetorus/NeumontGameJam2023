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

	private HUD hud;
	private Shop shop;

	[SerializeField] private GameObject playerPrefab;
	public GameObject Player { get; private set; }

	private int score = 0;

	private void Awake()
	{
		if(Instance == null) { Instance = this; DontDestroyOnLoad(this); }
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.P))
		{
			OpenShop();
		}
		else if(Input.GetKeyDown(KeyCode.O))
		{
			CloseShop();
		}
	}

	public void OnSceneLoad(Scene scene, LoadSceneMode mode)
	{
		if(scene.name == "Game")
		{
			Player = Instantiate(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
			hud = FindFirstObjectByType<HUD>();
			shop = FindFirstObjectByType<Shop>();
			shop.gameObject.SetActive(false);
		}
	}

	public void SetHealth(float percent)
	{
		hud.SetHealth(percent);
	}

	public void AddScore()
	{
		hud.SetScore(++score);

		if(score == 10) { OpenShop(); }
	}

	public void OpenShop()
	{
		Time.timeScale = 0.0f;

		shop.gameObject.SetActive(true);
	}

	public void CloseShop()
	{
		Time.timeScale = 1.0f;

		shop.gameObject.SetActive(false);
	}
}
