using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public void Play()
	{
		//TODO: Scene loader
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
