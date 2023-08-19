using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	private SceneLoader sceneLoader;
	public SceneLoader SceneLoader
	{
		get
		{
			if (sceneLoader == null)
			{
				sceneLoader = FindFirstObjectByType<SceneLoader>();

				if (sceneLoader == null) { sceneLoader = new GameObject("SceneLoader").AddComponent<SceneLoader>(); DontDestroyOnLoad(sceneLoader); }
			}
			return sceneLoader;
		}
		private set { sceneLoader = value; }
	}

	private HUD hud;
	private Shop shop;
	private Pause pause;
	private Death death;
	public bool Paused { get; private set; }
	public bool InShop { get; private set; }

	[SerializeField] private GameObject playerPrefab;
	public GameObject Player { get; private set; }

	public int Score { get; private set; } = 0;

	private int scoreGoal = 5;

	private bool destroy = false;

	public GameManager()
	{
		if (Instance == null) { Instance = this; }
		else if(this != Instance) { destroy = true; }
	}

	private void Awake()
	{
		if (destroy) { DestroyImmediate(gameObject); }
		else { DontDestroyOnLoad(this); }
	}

	private void Update()
	{
		//TODO: Remove, for debug use
		if(Input.GetKeyDown(KeyCode.P))
		{
			OpenShop();
		}
		else if(Input.GetKeyDown(KeyCode.O))
		{
			CloseShop();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(Paused) { Unpause(); }
			else { Pause(); }
		}
	}

	public void OnSceneLoad(Scene scene, LoadSceneMode mode)
	{
		if(scene.name == "Game" && Player == null)
		{
			Player = Instantiate(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
			hud = FindFirstObjectByType<HUD>();
			shop = FindFirstObjectByType<Shop>();
			shop.gameObject.SetActive(false);
			InShop = false;
			pause = FindFirstObjectByType<Pause>();
			pause.gameObject.SetActive(false);
			Paused = false;
			death = FindFirstObjectByType<Death>();
			death.gameObject.SetActive(false);
		}
	}

	public void SetHealth(float percent)
	{
		hud.SetHealth(percent);
	}

	public void AddScore()
	{
		hud.SetScore(++Score);

		if(Score == scoreGoal) { OpenShop(); scoreGoal *= 2; }
	}

	public void OpenShop()
	{
		Time.timeScale = 0.0f;

		shop.gameObject.SetActive(true);

		InShop = true;
	}

	public void CloseShop()
	{
		shop.gameObject.SetActive(false);

		InShop = false;

		if(!InShop && !Paused) { Time.timeScale = 1.0f; }
	}

	public void Pause()
	{
		Time.timeScale = 0.0f;

		pause.gameObject.SetActive(true);

		Paused = true;
	}

	public void Unpause()
	{
		pause.gameObject.SetActive(false);

		Paused = false;

		if (!InShop && !Paused) { Time.timeScale = 1.0f; }
	}

	public void GoToMainMenu()
	{
		Paused = false;
		InShop = false;

		Time.timeScale = 1.0f;
		shop.gameObject.SetActive(true);
		pause.gameObject.SetActive(true);

		SceneLoader.LoadLevel("MainMenu");
	}

	public void OnDeath()
	{
		Time.timeScale = 0.0f;

		death.gameObject.SetActive(true);
	}
}
