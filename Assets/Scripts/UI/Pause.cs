using UnityEngine;

public class Pause : MonoBehaviour
{
	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void Resume()
	{
		GameManager.Instance.Unpause();
	}

	public void MainMenu()
	{
		GameManager.Instance.GoToMainMenu();
	}
}
