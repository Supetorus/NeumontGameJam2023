using UnityEditor;
using UnityEngine;

public class Credits : MonoBehaviour
{
	[SerializeField] private GameObject text;

	private RectTransform textPos;
	private bool running = false;
	private float speed = 100.0f;

	private void Awake()
	{
		textPos = text.GetComponent<RectTransform>();
	}

	void Update()
	{
		if(running)
		{
			speed += Time.deltaTime;
			textPos.position += Vector3.up * Time.deltaTime * speed;

			if(textPos.position.y >= 1000)
			{
				textPos.position = new Vector3(550, -100, 0);
			}
		}
	}

	public void Begin()
	{
		running = true;

		textPos.position = new Vector3(550, 300, 0);
		speed = 100.0f;
	}

	public void End()
	{
		running = false;
	}
}
