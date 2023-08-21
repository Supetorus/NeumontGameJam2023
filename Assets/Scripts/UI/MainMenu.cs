using UnityEngine;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Credits credits;
	private bool playingCredits = false;

	private void Awake()
	{
		credits.gameObject.SetActive(false);
	}

	private void Update()
	{
		if(playingCredits && Input.anyKeyDown)
		{
			StopCredits();
		}
	}

	public void Play()
	{
		GameManager.Instance.SceneLoader.LoadLevel("Game");
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void PlayCredits()
	{
		playingCredits = true;
		credits.gameObject.SetActive(true);
		credits.Begin();
	}

	private void StopCredits()
	{
		playingCredits = false;
		credits.gameObject.SetActive(false);
		credits.End();
	}
}
