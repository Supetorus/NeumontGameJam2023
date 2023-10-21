using System.Collections;
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
	public bool InGame { get; private set; }

	[SerializeField] private GameObject playerPrefab;
	public GameObject Player { get; private set; }

	public int Score { get; private set; } = 0;
	[SerializeField, Tooltip("How much the score must increase by to get to the next shop screen")]
	private float shopScoreMultiplier = 2;

	private int scoreGoal = 0;

	private bool destroy = false;

	public GameManager()
	{
		if (Instance == null) { Instance = this; }
		else if (this != Instance) { destroy = true; }
	}

	private void Awake()
	{
		if (destroy) { DestroyImmediate(gameObject); }
		else { DontDestroyOnLoad(this); }

		InGame = false;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && InGame)
		{
			if (Paused) { Unpause(); }
			else { Pause(); }
		}
	}

	public void OnSceneLoad(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Game" && Player == null)
		{
			Player = Instantiate(playerPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
			hud = FindFirstObjectByType<HUD>();
			scoreGoal = 5;
			hud.SetScore(Score);
			hud.SetGoal(scoreGoal);
			shop = FindFirstObjectByType<Shop>();
			shop.gameObject.SetActive(false);
			InShop = false;
			pause = FindFirstObjectByType<Pause>();
			pause.gameObject.SetActive(false);
			Paused = false;
			death = FindFirstObjectByType<Death>();
			death.gameObject.SetActive(false);

			Cursor.lockState = CursorLockMode.Confined;
			InGame = true;
		}
	}

	public void SetHealth(float percent)
	{
		hud.SetHealth(percent);
	}

	public void AddScore()
	{
		hud.SetScore(++Score);

		if(Score == scoreGoal)
		{
			OpenShop();
			scoreGoal = (int)(scoreGoal * shopScoreMultiplier);
			hud.SetGoal(scoreGoal);
		}
	}

	public void OpenShop()
	{
		Time.timeScale = 0.0f;

		//shop.gameObject.SetActive(true);
		StartCoroutine(ShowShop());
		Cursor.lockState = CursorLockMode.None;

		InShop = true;
	}

	private IEnumerator ShowShop()
	{
		shop.gameObject.SetActive(true);
		float duration = 0.5f;
		var rect = shop.transform.GetChild(0).GetComponent<RectTransform>();
		float elapsedTime = 0f;

		while (elapsedTime < duration)
		{
			float t = elapsedTime / duration;
			float scale = Mathf.Lerp(0, 1, t);
			rect.localScale = Vector3.one * scale;

			elapsedTime += 0.05f;
			yield return new WaitForSecondsRealtime(0.05f);
		}
		rect.localScale = Vector3.one;
	}

	public void CloseShop()
	{
		shop.gameObject.SetActive(false);

		InShop = false;

		if (!InShop && !Paused) { Time.timeScale = 1.0f; Cursor.lockState = CursorLockMode.Confined; }
	}

	public void Pause()
	{
		Time.timeScale = 0.0f;

		pause.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;

		Paused = true;
	}

	public void Unpause()
	{
		pause.gameObject.SetActive(false);

		Paused = false;

		if (!InShop && !Paused) { Time.timeScale = 1.0f; Cursor.lockState = CursorLockMode.Confined; }
	}

	public void GoToMainMenu()
	{
		Paused = false;
		InShop = false;
		InGame = false;
		Score = 0;
		scoreGoal = 5;

		Time.timeScale = 1.0f;
		//shop.gameObject.SetActive(true);
		//pause.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.Confined;

		SceneLoader.LoadLevel("MainMenu");
	}

	public void OnDeath()
	{
		Time.timeScale = 0.0f;

		death.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
	}
}
