using UnityEngine;

public class Death : MonoBehaviour
{
	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}

	public void MainMenu()
	{
		GameManager.Instance.GoToMainMenu();
	}
}
