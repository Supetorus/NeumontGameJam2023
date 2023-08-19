using UnityEngine;

public class MainMenu : MonoBehaviour
{
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
}
